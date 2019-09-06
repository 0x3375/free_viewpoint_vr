using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FiveDoF : MonoBehaviour
{
    /* C++ DLL 사용 안함*/

    //private const string NATIVE_LIBRARY_NAME = "C:/myUnity/5DOF/Assets/5DoF";
    //[DllImport(NATIVE_LIBRARY_NAME)]
    //private static extern int nativeMakeLFSpace(string dir, int height = 1024, int width = 2048);
    //
    //[DllImport(NATIVE_LIBRARY_NAME)]
    //private static extern int nativeUpdatePicture(int posX = 50); 
    //
    //// Start is called before the first frame update

    GameObject LFS = null; // 3D 구의 LF Space
    Vector3 v = new Vector3(0.1f, 0.0f, 0.0f); // 포지션 좌우 이동시 피연산 벡터

    int currentPos; // 디폴트 포지션 (0050.jpg)
    public string directoryName = "S2/";
    public int maxImgIdx = 500;

    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
    
    int i = 0;

    TextAsset bindata = new TextAsset();
    Texture2D tex = new Texture2D(1024,512, TextureFormat.RGB24, false);

    void Start() // 플레이버튼 누를 시 최초 1회만 실행
    {
        LFS = GameObject.Find("LFSpace"); // LF
        LFS.GetComponent<Renderer>().enabled = true; 
        LFS.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Pano360Shader"); // 구에 입혀진 텍스쳐를 렌더링하기 위해 쉐이더 로드
        currentPos = maxImgIdx / 2;
        LFS.GetComponent<Renderer>().material.mainTexture = Resources.Load(directoryName + currentPos.ToString("D4")) as Texture2D; //구에 입힐 최초 이미지
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A)) // 우측
        {
            sw.Start();
            currentPos = currentPos + 1;
            if (currentPos > maxImgIdx) currentPos = maxImgIdx;
            LFS.GetComponent<Renderer>().material.mainTexture = Resources.Load(directoryName + currentPos.ToString("D4")) as Texture2D; // CurrentPos를 스트링화한 후 Path로 설정, 로드
            sw.Stop();
            Debug.Log(sw.ElapsedTicks / 10 + "us (" + i + ")");
            i = i + 1;
        }
        if (Input.GetKey(KeyCode.D)) // 좌측
        {
            sw.Start();
            currentPos = currentPos - 1;
            if (currentPos < 1) currentPos = 1;
            LFS.GetComponent<Renderer>().material.mainTexture = Resources.Load(directoryName + currentPos.ToString("D4")) as Texture2D; // CurrentPos를 스트링화한 후 Path로 설정, 로드
            sw.Stop();
            Debug.Log(sw.ElapsedTicks / 10 + "us (" + i + ")");
            i = i + 1;
        }
        sw.Reset();
        
        if (Input.GetKey(KeyCode.W)) // 전진
        {
            currentPos = currentPos + 1; 
            if (currentPos > maxImgIdx) currentPos = maxImgIdx;
        }
        if (Input.GetKey(KeyCode.S)) // 후진
        {
            currentPos = currentPos - 1;
            if (currentPos < 1) currentPos = 1;
        }
        // Debug.Log(currentPos);
    }
}
