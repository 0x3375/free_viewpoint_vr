#include <cuda_runtime.h>
#include <iostream>
#include "cuda_runtime.h"
#include "cuda.h"
#include "device_launch_parameters.h"
#include <cmath>
#include <chrono>

#define USE_ZERO_COPY

class StopWatch {
public:
	void Start() {
		t0 = std::chrono::high_resolution_clock::now();
	}
	double Stop() {
		return std::chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::high_resolution_clock::now().time_since_epoch() - t0.time_since_epoch()).count();
	}

private:
	std::chrono::high_resolution_clock::time_point t0;
};


__global__ void ParallelFor(unsigned char* IMG, unsigned char* LF, double Alloc_Angle_s, double times, int Y, int POS_Y, int POS_X, int LFUW, int DATAW, int WIDTH, int HEIGHT, int out_w, int dir)
{
	int tw = blockIdx.x; // blockIdx.x = (int)[0, (out_w - 1)]
	int th = threadIdx.y; // threadIdx = (int)[0, (HEIGHT - 1)]

	//Console.WriteLine("{0}: {1}", Thread.CurrentThread.ManagedThreadId, w);
	double a = Alloc_Angle_s + (0.0025 * (double)tw);
	double P = (double)(Y - POS_Y) * tan(a) + POS_X;
	double b = sqrt(2.0) * LFUW;
	double N_dist = sqrt((double)((P - POS_X) * (P - POS_X) + (Y - POS_Y) * (Y - POS_Y))) / b;

	P = P / 2;
	int P_1 = (int)(round(P + (DATAW / 2)));
	if (dir == 3 || dir == 4) P_1 = DATAW - P_1 - 1;
	
	double U = a * (180.0 / 3.14159265358979323846) * (1.0 / 180.0) * (WIDTH / 2) + (WIDTH / 2);
	int U_1 = (int)(round(U));

	if (dir == 2) U_1 = U_1 + WIDTH / 4;
	if (dir == 3) U_1 = U_1 + WIDTH / 2;
	if (dir == 4) U_1 = U_1 - WIDTH / 4;

	if (U_1 >= (WIDTH)) U_1 = U_1 - WIDTH;
	else if (U_1 < 0) U_1 = U_1 + WIDTH;

	if (P_1 >= DATAW) P_1 = DATAW - 1;
	else if (P_1 < 0) P_1 = 0;

	if (U_1 >= WIDTH) U_1 = WIDTH - 1;
	else if (U_1 < 0) U_1 = 0;

	int N_off = (int)(floor(times * N_dist + 0.5)) >> 1;
	double N_H_r = (double)(HEIGHT + N_off) / HEIGHT;


	double h_n = (th - HEIGHT / 2) * N_H_r + HEIGHT / 2;

	int U_1_n = 0;
	if (h_n < 0)
	{
		U_1_n = U_1 + WIDTH / 2;
		if (U_1_n > WIDTH - 1) U_1_n = U_1 - WIDTH / 2;

		h_n = (-1 * h_n) - 1;
	}
	else if (h_n > HEIGHT - 1)
	{
		U_1_n = U_1 + WIDTH / 2;
		if (U_1_n > WIDTH - 1) U_1_n = U_1 - WIDTH / 2;

		h_n = HEIGHT - ((h_n - HEIGHT) - 1);
	}
	else
	{
		U_1_n = U_1;
	}

	int H_1 = (int)(round(h_n));
	if (H_1 >= HEIGHT) H_1 = HEIGHT - 1;
	else if (H_1 < 0) H_1 = 0;

	unsigned char PEL_0 = LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 0]; // b
	unsigned char PEL_1 = LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 1]; // g
	unsigned char PEL_2 = LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 2]; // r

	IMG[(HEIGHT - th - 1) * (out_w * 3) + tw * 3 + 2] = PEL_0; // r
	IMG[(HEIGHT - th - 1) * (out_w * 3) + tw * 3 + 1] = PEL_1; // g
	IMG[(HEIGHT - th - 1) * (out_w * 3) + tw * 3 + 0] = PEL_2; // b // height를 먼저 채우는 코드
	// Unity use the big endian color format -> 0xRR 0xGG 0xBB ...
	// Unity use the Bottom-Left origin in 2D coor. (h -> HEIGHT - h) (082219)
}

extern "C" __declspec(dllexport) int CudaParallelFor(unsigned char* h_outResult, unsigned char* h_inLightField, double Alloc_Angle_s, double times, int Y, int POS_Y, int POS_X, int LFUW, int DATAW, int WIDTH, int HEIGHT, int out_w, int dir, int inLightField_Len)
{
	// outResult[HEIGHT * out_W * 3]
	// inLightField[

	unsigned char* h_tmp, *d_tmp;

	

	StopWatch sw_inLightFieldMalloc, sw_ResultMalloc, sw_copyH2D, sw_For, sw_copyD2H, sw_freeinLightField, sw_freeResult;
	cudaError_t errorCode;

	dim3 threadsPerBlock(1, HEIGHT); // 1x1024 모양의 스레드가 한 블록을 구성
	dim3 blocksPerGrid(out_w, 1); // 이 블록들이 out_w의 너비만큼 한 그리드를 구성

	int alignedLen_I = (((int)(inLightField_Len + 4095) / 4096) * 4096) * sizeof(unsigned char);
	int alignedLen_O = (((int)((HEIGHT * out_w * 3) + 4095) / 4096) * 4096) * sizeof(unsigned char);
	
	unsigned char* d_outResult; // output
	unsigned char* d_inLightField;

	cudaSetDeviceFlags(cudaDeviceMapHost);

	/******************************/
	errorCode = cudaHostAlloc((void**)&h_tmp, inLightField_Len, cudaHostAllocMapped);
	if (errorCode != cudaSuccess) return errorCode;
	cudaMemcpy(h_tmp, h_inLightField, inLightField_Len, cudaMemcpyHostToHost);
	errorCode = cudaHostAlloc((void**)&d_tmp, (HEIGHT * out_w * 3), cudaHostAllocMapped);
	if (errorCode != cudaSuccess) return errorCode;
	cudaMemcpy(d_tmp, h_outResult, (HEIGHT * out_w * 3), cudaMemcpyHostToHost);

	ParallelFor << <blocksPerGrid, threadsPerBlock >> > (d_tmp, h_tmp, Alloc_Angle_s, times, Y, POS_Y, POS_X, LFUW, DATAW, WIDTH, HEIGHT, out_w, dir);
	cudaMemcpy(h_outResult, d_tmp, (HEIGHT * out_w * 3), cudaMemcpyDeviceToHost);
	cudaDeviceSynchronize();
	errorCode = cudaGetLastError();
	if (errorCode != cudaSuccess) return errorCode;

	cudaFreeHost(d_tmp);
	cudaFreeHost(h_tmp);
	/******************************/
	/*
	sw_ResultMalloc.Start();
	errorCode = cudaHostRegister(h_outResult, alignedLen_O, cudaHostRegisterMapped);
	// errorCode = cudaMalloc((void**)&d_outResult, HEIGHT * out_w * 3 * sizeof(unsigned char));
	double us_resultMalloc = sw_ResultMalloc.Stop() / 1000;
	if (errorCode != cudaSuccess) return errorCode;

	sw_inLightFieldMalloc.Start();
	errorCode = cudaHostRegister(h_inLightField, alignedLen_I, cudaHostRegisterMapped);
	double us_inLightFieldMalloc = sw_inLightFieldMalloc.Stop() / 1000;
	if (errorCode != cudaSuccess) return errorCode;

	sw_copyH2D.Start();
	errorCode = cudaHostGetDevicePointer((void**)&d_outResult, h_outResult, 0);
	errorCode = cudaHostGetDevicePointer((void**)&d_inLightField, h_inLightField, 0);
	double us_copyH2D = sw_copyH2D.Stop() / 1000;
	if (errorCode != cudaSuccess) return errorCode;

	sw_For.Start();
	ParallelFor << <blocksPerGrid, threadsPerBlock >> > (d_outResult, d_inLightField, Alloc_Angle_s, times, Y, POS_Y, POS_X, LFUW, DATAW, WIDTH, HEIGHT, out_w, dir);
	cudaDeviceSynchronize();
	cudaSetDeviceFlags(cudaDeviceScheduleBlockingSync);
	
	double us_for = sw_For.Stop() / 1000;

	errorCode = cudaGetLastError();
	if (errorCode != cudaSuccess) return errorCode;

	//sw_copyD2H.Start();
	//errorCode = cudaMemcpy(outResult, d_outResult, HEIGHT * out_w * 3 * sizeof(unsigned char), cudaMemcpyDeviceToHost);
	//double us_copyD2H = sw_copyD2H.Stop() / 1000;
	//if (errorCode != cudaSuccess) return errorCode;

	// sw_freeResult.Start();
	// errorCode = cudaFree(d_outResult);
	// double us_freeResult = sw_freeResult.Stop() / 1000;
	// if (errorCode != cudaSuccess) return errorCode;

	// sw_freeinLightField.Start();
	// errorCode = cudaFree(d_inLightField);
	// double us_freeinLightField = sw_freeinLightField.Stop() / 1000;
	// if (errorCode != cudaSuccess) return errorCode;

	cudaHostUnregister(h_inLightField);
	cudaHostUnregister(h_outResult);

	double us_total = us_for + us_copyH2D + us_inLightFieldMalloc + us_resultMalloc;// +us_freeinLightField + us_freeResult + us_copyD2H;

	// fprintf(fp, "ResultMalloc\tinLightFieldMalloc\tmemCpyH2D\tFor\tmemCpyD2H\tfreeResult\tfreeinLightField\tTOTAL\n");
	FILE* fp = fopen("C:/myUnity/timelog/cuda.txt", "a");
	fprintf(fp, "%f\t%f\t%f\t%f\t%f\t%f\t%f\t%f\n", us_resultMalloc, us_inLightFieldMalloc, us_copyH2D, us_for, us_total);//us_freeinLightField, us_freeResult, us_copyD2H);
	fclose(fp);
	*/
	return 0;
}


/*
__global__ void addcuda(int* a, int *b, int* c)
{
	 c[blockIdx.x * 1024 + threadIdx.y] = 1; // [0, 2047] * 1024 + [0, 1023]

	// blockDim : CUDA Block의 크기, Threads의 갯수 (threadsPerBlock와 같은 값) 
	// blockIdx : 블록 인덱스 ( 0 ~ blocksPerGrid-1 )
	// ThreadsIdx : Threads 인덱스 ( 0 ~ threadsPerBlock-1 )
}

__host__ void cudatest(int a, int b, int c)
{
	int WID = 2048; int HEI = 1024;
	dim3 threadsPerBlock(1, 1024); // # of threads, MAX : X * Y <= 1024 // 블록당 1024개 쓰레드를 돌림
	dim3 blocksPerGrid(WID / threadsPerBlock.x, HEI / threadsPerBlock.y); // # 블록이 몇번 재사용되어야 하는가? = 한 그리드는 몇 블록으로 구성되는가?)

	int* d_a;
	int* d_b;
	int* d_c;

	int* arr = (int*)malloc(sizeof(int) * 2048 * 1024);
	
	memset(arr, 0, sizeof(int) * 2048 * 1024);
	

	cudaMalloc((void**)&d_a, sizeof(int));
	cudaMalloc((void**)&d_b, sizeof(int));

	cudaMalloc((void**)&d_c, 2048 * 1024 * sizeof(int));


	
	cudaMemcpy(d_a, &a, sizeof(int), cudaMemcpyHostToDevice);
	cudaMemcpy(d_b, &b, sizeof(int), cudaMemcpyHostToDevice);
	cudaMemcpy(d_c, arr, sizeof(int) * 2048 * 1024, cudaMemcpyHostToDevice);


	addcuda << <blocksPerGrid, threadsPerBlock >> > (d_a, d_b, d_c); // 각각 Block 갯수와 Threads 갯수
	cudaDeviceSynchronize();
	cudaMemcpy(arr, d_c, sizeof(int) * 2048 * 1024, cudaMemcpyDeviceToHost);

	for (int i = 0; i < 2048*1024; i++)
	{ 
		if (arr[i] != 1) printf(" NOT 1 \n");
	}

	cudaFree(d_a); cudaFree(d_b); cudaFree(d_c);  free(arr);// 
}

int main()
{

	return 0;
}

*/

