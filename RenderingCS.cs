// #define USE_CUDA
#define USE_HMD 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using System.ComponentModel;


public class RenderingCS : MonoBehaviour
{
#if USE_CUDA
    private const string NATIVE_LIBRARY_NAME = "cudaTest";
    [DllImport(NATIVE_LIBRARY_NAME)]
    private static extern int CudaParallelFor(byte[] IMG, byte[] LF, double Alloc_Angle_s, double times, int Y, int POS_Y, int POS_X, int LFUW, int DATAW, int WIDTH, int HEIGHT, int out_w, int dir, int LF_Len);
#endif

    public static int WIDTH = 1024;
    public static int HEIGHT = 512;
    public static int DATAW = 50;
    public static int LFUW = 100;

    private GameObject LFS = null; // 3D 구의 LF Space
    private Texture2D tex;

    private static byte[] COL1;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL2;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL3;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL4;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL5;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL6;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL7;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL8;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL9;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL10;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL11;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL12;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL13;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL14;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL15;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL16;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL17;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] COL18;// = new byte[HEIGHT * WIDTH * LFUW * 3];

    private static byte[] ROW1;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW2;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW3;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW4;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW5;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW6;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW7;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW8;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW9;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW10;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW11;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW12;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW13;// = new byte[HEIGHT * WIDTH * LFUW * 3];
    private static byte[] ROW14;// = new byte[HEIGHT * WIDTH * LFUW * 3];

    private static string COL1_FILE = "Assets/Resources/1024x512/COL1.LF";
    private static string COL2_FILE = "Assets/Resources/1024x512/COL2.LF";
    private static string COL3_FILE = "Assets/Resources/1024x512/COL3.LF";
    private static string COL4_FILE = "Assets/Resources/1024x512/COL4.LF";
    private static string COL5_FILE = "Assets/Resources/1024x512/COL5.LF";
    private static string COL6_FILE = "Assets/Resources/1024x512/COL6.LF";
    private static string COL7_FILE = "Assets/Resources/1024x512/COL7.LF";
    private static string COL8_FILE = "Assets/Resources/1024x512/COL8.LF";
    private static string COL9_FILE = "Assets/Resources/1024x512/COL9.LF";
    private static string COL10_FILE = "Assets/Resources/1024x512/COL10.LF";
    private static string COL11_FILE = "Assets/Resources/1024x512/COL11.LF";
    private static string COL12_FILE = "Assets/Resources/1024x512/COL12.LF";
    private static string COL13_FILE = "Assets/Resources/1024x512/COL13.LF";
    private static string COL14_FILE = "Assets/Resources/1024x512/COL14.LF";
    private static string COL15_FILE = "Assets/Resources/1024x512/COL15.LF";
    private static string COL16_FILE = "Assets/Resources/1024x512/COL16.LF";
    private static string COL17_FILE = "Assets/Resources/1024x512/COL17.LF";
    private static string COL18_FILE = "Assets/Resources/1024x512/COL18.LF";
    private static string ROW1_FILE = "Assets/Resources/1024x512/ROW1.LF";
    private static string ROW2_FILE = "Assets/Resources/1024x512/ROW2.LF";
    private static string ROW3_FILE = "Assets/Resources/1024x512/ROW3.LF";
    private static string ROW4_FILE = "Assets/Resources/1024x512/ROW4.LF";
    private static string ROW5_FILE = "Assets/Resources/1024x512/ROW5.LF";
    private static string ROW6_FILE = "Assets/Resources/1024x512/ROW6.LF";
    private static string ROW7_FILE = "Assets/Resources/1024x512/ROW7.LF";
    private static string ROW8_FILE = "Assets/Resources/1024x512/ROW8.LF";
    private static string ROW9_FILE = "Assets/Resources/1024x512/ROW9.LF";
    private static string ROW10_FILE = "Assets/Resources/1024x512/ROW10.LF";
    private static string ROW11_FILE = "Assets/Resources/1024x512/ROW11.LF";
    private static string ROW12_FILE = "Assets/Resources/1024x512/ROW12.LF";
    private static string ROW13_FILE = "Assets/Resources/1024x512/ROW13.LF";
    private static string ROW14_FILE = "Assets/Resources/1024x512/ROW14.LF";

    private static bool COL1_FLAG = false;
    private static bool COL2_FLAG = false;
    private static bool COL3_FLAG = false;
    private static bool COL4_FLAG = false;
    private static bool COL5_FLAG = false;
    private static bool COL6_FLAG = false;
    private static bool COL7_FLAG = false;
    private static bool COL8_FLAG = false;
    private static bool COL9_FLAG = false;
    private static bool COL10_FLAG = false;
    private static bool COL11_FLAG = false;
    private static bool COL12_FLAG = false;
    private static bool COL13_FLAG = false;
    private static bool COL14_FLAG = false;
    private static bool COL15_FLAG = false;
    private static bool COL16_FLAG = false;
    private static bool COL17_FLAG = false;
    private static bool COL18_FLAG = false;

    private static bool ROW1_FLAG = false;
    private static bool ROW2_FLAG = false;
    private static bool ROW3_FLAG = false;
    private static bool ROW4_FLAG = false;
    private static bool ROW5_FLAG = false;
    private static bool ROW6_FLAG = false;
    private static bool ROW7_FLAG = false;
    private static bool ROW8_FLAG = false;
    private static bool ROW9_FLAG = false;
    private static bool ROW10_FLAG = false;
    private static bool ROW11_FLAG = false;
    private static bool ROW12_FLAG = false;
    private static bool ROW13_FLAG = false;
    private static bool ROW14_FLAG = false;

    private static byte[] LF_F = null;
    private static byte[] LF_R = null;
    private static byte[] LF_B = null;
    private static byte[] LF_L = null;

    private static int f, GLO_X, GLO_Y, POS_X, POS_Y, REGION, Loop, InKey, PREV_REGION, ROR;
    private static double times, k;

    private static int upperLimit_gloX = 200;
    private static int lowerLimit_gloX = 0;
    private static int upperLimit_gloY = 600;
    private static int lowerLimit_gloY = 0;

    private static int i = 0;

    private static System.Diagnostics.Stopwatch sw_LoadLF = new System.Diagnostics.Stopwatch();
    private static System.Diagnostics.Stopwatch sw_LFUpdate = new System.Diagnostics.Stopwatch();
    private static System.Diagnostics.Stopwatch sw_Rendering = new System.Diagnostics.Stopwatch();
    private static System.Diagnostics.Stopwatch sw_MainFunc = new System.Diagnostics.Stopwatch();
    private static System.Diagnostics.Stopwatch sw_ParallelFor = new System.Diagnostics.Stopwatch();
    private static Stopwatch sw_GC = new Stopwatch();
    private static long ParallelFor_us = 0;

    int w, d, s;
    public static int REGION_G;

    private static int rotationY;
    private static int componentY, componentX;
    private static int stride = 2;

    private static Vector3 hmdPosition;
    private static GameObject hmd;

    private static float prevHmdPosX, prevHmdPosY;
    private static float diffHmdPosX, diffHmdPosY;
    // Start is called before the first frame update
    void Start()
    {
#if USE_HMD
        hmd = GameObject.FindWithTag("sydHMD");
        hmd.transform.position = transform.position;
        prevHmdPosX = hmd.transform.position.x;
        prevHmdPosY = hmd.transform.position.y;
#endif
        LFS = GameObject.Find("LFSpace"); // LF
        LFS.GetComponent<Renderer>().enabled = true;
        LFS.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Pano360Shader");

        // LFS.GetComponent<Renderer>().material.mainTexture = tex;

        f = WIDTH / 8;
        GLO_X = 50;
        GLO_Y = 50;

        POS_X = 0;
        POS_Y = 0;
        REGION = 0;

        times = 270.0;
        k = 0.30;

        REGION = 0;
        POS_X = GLO_X - 50;
        POS_Y = GLO_Y - 50;

        LFUpdate(REGION);

        Loop = 1;
        InKey = 0;
        PREV_REGION = REGION;
        ROR = 0;
        w = d = s = 0;
        sw_LoadLF.Reset();
        sw_LFUpdate.Reset();
        sw_Rendering.Reset();
        sw_MainFunc.Reset();
        sw_ParallelFor.Reset();
        sw_GC.Reset();
    }

    // Update is called once per frame
    void Update()
    {
#if USE_HMD
        transform.position = hmd.transform.position;

        diffHmdPosX = (transform.position.x - prevHmdPosX); 
        diffHmdPosY = (transform.position.y - prevHmdPosY);

        

        GLO_Y -= (int)Mathf.Round((diffHmdPosX / Mathf.Sqrt(Mathf.Pow(diffHmdPosX, 2) + Mathf.Pow(diffHmdPosY, 2)) * 2), 2 );  // need a scaling factor
        GLO_X += (int)Mathf.Round((diffHmdPosY / Mathf.Sqrt(Mathf.Pow(diffHmdPosX, 2) + Mathf.Pow(diffHmdPosY, 2)) * 2), 2 );  // need a scaling factor

        UnityEngine.Debug.Log("DiffVec2 : (" + diffHmdPosX.ToString() + ", " + diffHmdPosY + ")");

        GLO_X = Mathf.Clamp(GLO_X, lowerLimit_gloX, upperLimit_gloX);
        GLO_Y = Mathf.Clamp(GLO_Y, lowerLimit_gloY, upperLimit_gloY);

        UnityEngine.Debug.Log("(" + GLO_X.ToString() + ", " + GLO_Y.ToString() + ")");
        SetRegion();
        MainFunc(ref tex, ref LFS);

        prevHmdPosX = hmd.transform.position.x;
        prevHmdPosY = hmd.transform.position.y;
#else

        rotationY = (int)Mathf.Round(CameraController.getRotationY()) % 360;
        componentY = (int)Mathf.Round(stride * Mathf.Sin(rotationY * Mathf.Deg2Rad));
        componentX = -1 * (int)Mathf.Round(stride * Mathf.Cos(rotationY * Mathf.Deg2Rad));

        if (Input.GetKey(KeyCode.A))
        {
            GLO_Y += componentX;
            GLO_X -= componentY;
            GLO_Y = Mathf.Clamp(GLO_Y, lowerLimit_gloY, upperLimit_gloY);
            GLO_X = Mathf.Clamp(GLO_X, lowerLimit_gloX, upperLimit_gloX);
            SetRegion();
            MainFunc(ref tex, ref LFS);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            GLO_Y -= componentX;
            GLO_X += componentY;
            GLO_Y = Mathf.Clamp(GLO_Y, lowerLimit_gloY, upperLimit_gloY);
            GLO_X = Mathf.Clamp(GLO_X, lowerLimit_gloX, upperLimit_gloX);
            SetRegion();
            MainFunc(ref tex, ref LFS);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            GLO_Y += componentY;
            GLO_X += componentX;
            GLO_Y = Mathf.Clamp(GLO_Y, lowerLimit_gloY, upperLimit_gloY);
            GLO_X = Mathf.Clamp(GLO_X, lowerLimit_gloX, upperLimit_gloX);
            SetRegion();
            MainFunc(ref tex, ref LFS);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            GLO_Y -= componentY;
            GLO_X -= componentX;
            GLO_Y = Mathf.Clamp(GLO_Y, lowerLimit_gloY, upperLimit_gloY);
            GLO_X = Mathf.Clamp(GLO_X, lowerLimit_gloX, upperLimit_gloX);
            SetRegion();
            MainFunc(ref tex, ref LFS);
        }

#endif
        // UnityEngine.Debug.Log("View Position (X, Y) = (" + GLO_X + ", " + GLO_Y + ") -> (componentX, componentY) = ( " + componentX + ", " + componentY + ")" + "rotY : " + rotationY + "[" + i++);

        long LoadLF_us = sw_LoadLF.ElapsedTicks / 10;
        long LFUpdate_us = sw_LFUpdate.ElapsedTicks / 10;
        long Rendering_us = sw_Rendering.ElapsedTicks / 10;
        long MainFunc_us = sw_MainFunc.ElapsedTicks / 10;
        long ParallelFor_us = sw_ParallelFor.ElapsedTicks / 10;

        //using (System.IO.StreamWriter log_LoadLF = new System.IO.StreamWriter("C:/myUnity/timelog/LoadLF.txt", true))
        //    log_LoadLF.WriteLine(LoadLF_us.ToString());
        //
        //using (System.IO.StreamWriter log_LFUpdate = new System.IO.StreamWriter("C:/myUnity/timelog/LFUpdate.txt", true))
        //    log_LFUpdate.WriteLine(LFUpdate_us.ToString());
        //
        //using (System.IO.StreamWriter log_Rendering = new System.IO.StreamWriter("C:/myUnity/timelog/Rendering.txt", true))
        //    log_Rendering.WriteLine(Rendering_us.ToString());
        //
        //using (System.IO.StreamWriter log_MainFunc = new System.IO.StreamWriter("C:/myUnity/timelog/MainFunc.txt", true))
        //    log_MainFunc.WriteLine(MainFunc_us.ToString());
        //using (System.IO.StreamWriter log_ParallelFor = new System.IO.StreamWriter("C:/myUnity/timelog/ParallelFor.txt", true))
        //    log_ParallelFor.WriteLine(ParallelFor_us.ToString());
        //
        //using (System.IO.StreamWriter log_GC = new System.IO.StreamWriter("C:/myUnity/timelog/GC.txt", true))
        //    log_GC.WriteLine((sw_GC.ElapsedTicks / 10).ToString());
        //
        sw_LoadLF.Reset();
        sw_LFUpdate.Reset();
        sw_GC.Reset();
    }

    public static void sub_LFUpdate_ROW(int R1, int R2, int R3, int R4, int R5, int R6, int R7, int R8, int R9, int R10, int R11, int R12, int R13, int R14)
    {
        if (R1 == 1)
        {
            if (!ROW1_FLAG)
            {
                ROW1 = LoadLF(ROW1_FILE, LFUW);
            }
            ROW1_FLAG = true;
        }
        else
        {
            if (ROW1_FLAG)
                ROW1 = null;
            ROW1_FLAG = false;
        }

        if (R2 == 1)
        {
            if (!ROW2_FLAG)
            {
                ROW2 = LoadLF(ROW2_FILE, LFUW);
            }
            ROW2_FLAG = true;
        }
        else
        {
            if (ROW2_FLAG)
                ROW2 = null;
            ROW2_FLAG = false;
        }

        if (R3 == 1)
        {
            if (!ROW3_FLAG)
            {
                ROW3 = LoadLF(ROW3_FILE, LFUW);
            }
            ROW3_FLAG = true;
        }
        else
        {
            if (ROW3_FLAG)
                ROW3 = null;
            ROW3_FLAG = false;
        }

        if (R4 == 1)
        {
            if (!ROW4_FLAG)
            {
                ROW4 = LoadLF(ROW4_FILE, LFUW);
            }
            ROW4_FLAG = true;
        }
        else
        {
            if (ROW4_FLAG)
                ROW4 = null;
            ROW4_FLAG = false;
        }

        if (R5 == 1)
        {
            if (!ROW5_FLAG)
            {
                ROW5 = LoadLF(ROW5_FILE, LFUW);
            }
            ROW5_FLAG = true;
        }
        else
        {
            if (ROW5_FLAG)
                ROW5 = null;
            ROW5_FLAG = false;
        }

        if (R6 == 1)
        {
            if (!ROW6_FLAG)
            {
                ROW6 = LoadLF(ROW6_FILE, LFUW);
            }
            ROW6_FLAG = true;
        }
        else
        {
            if (ROW6_FLAG)
                ROW6 = null;
            ROW6_FLAG = false;
        }

        if (R7 == 1)
        {
            if (!ROW7_FLAG)
            {
                ROW7 = LoadLF(ROW7_FILE, LFUW);
            }
            ROW7_FLAG = true;
        }
        else
        {
            if (ROW7_FLAG)
                ROW7 = null;
            ROW7_FLAG = false;
        }

        if (R8 == 1)
        {
            if (!ROW8_FLAG)
            {
                ROW8 = LoadLF(ROW8_FILE, LFUW);
            }
            ROW8_FLAG = true;
        }
        else
        {
            if (ROW8_FLAG)
                ROW8 = null;
            ROW8_FLAG = false;
        }

        if (R9 == 1)
        {
            if (!ROW9_FLAG)
            {
                ROW9 = LoadLF(ROW9_FILE, LFUW);
            }
            ROW9_FLAG = true;
        }
        else
        {
            if (ROW9_FLAG)
                ROW9 = null;
            ROW9_FLAG = false;
        }

        if (R10 == 1)
        {
            if (!ROW10_FLAG)
            {
                ROW10 = LoadLF(ROW10_FILE, LFUW);
            }
            ROW10_FLAG = true;
        }
        else
        {
            if (ROW10_FLAG)
                ROW10 = null;
            ROW10_FLAG = false;
        }

        if (R11 == 1)
        {
            if (!ROW11_FLAG)
            {
                ROW11 = LoadLF(ROW11_FILE, LFUW);
            }
            ROW11_FLAG = true;
        }
        else
        {
            if (ROW11_FLAG)
                ROW11 = null;
            ROW11_FLAG = false;
        }

        if (R12 == 1)
        {
            if (!ROW12_FLAG)
            {
                ROW12 = LoadLF(ROW12_FILE, LFUW);
            }
            ROW12_FLAG = true;
        }
        else
        {
            if (ROW12_FLAG)
                ROW12 = null;
            ROW12_FLAG = false;
        }

        if (R13 == 1)
        {
            if (!ROW13_FLAG)
            {
                ROW13 = LoadLF(ROW13_FILE, LFUW);
            }
            ROW13_FLAG = true;
        }
        else
        {
            if (ROW13_FLAG)
                ROW13 = null;
            ROW13_FLAG = false;
        }

        if (R14 == 1)
        {
            if (!ROW14_FLAG)
            {
                ROW14 = LoadLF(ROW14_FILE, LFUW);
            }
            ROW14_FLAG = true;
        }
        else
        {
            if (ROW14_FLAG)
                ROW14 = null;
            ROW14_FLAG = false;
        }
    }

    public static void sub_LFUpdate_COL(int C1, int C2, int C3, int C4, int C5, int C6, int C7, int C8, int C9, int C10, int C11, int C12, int C13, int C14, int C15, int C16, int C17, int C18)
    {
        if (C1 == 1)
        {
            if (!COL1_FLAG)
            {
                COL1 = LoadLF(COL1_FILE, LFUW);
            }
            COL1_FLAG = true;
        }
        else
        {
            if (COL1_FLAG)
                COL1 = null;
            COL1_FLAG = false;
        }

        if (C2 == 1)
        {
            if (!COL2_FLAG)
            {
                COL2 = LoadLF(COL2_FILE, LFUW);
            }
            COL2_FLAG = true;
        }
        else
        {
            if (COL2_FLAG)
                COL2 = null;
            COL2_FLAG = false;
        }

        if (C3 == 1)
        {
            if (!COL3_FLAG)
            {
                COL3 = LoadLF(COL3_FILE, LFUW);
            }
            COL3_FLAG = true;
        }
        else
        {
            if (COL3_FLAG)
                COL3 = null;
            COL3_FLAG = false;
        }

        if (C4 == 1)
        {
            if (!COL4_FLAG)
            {
                COL4 = LoadLF(COL4_FILE, LFUW);
            }
            COL4_FLAG = true;
        }
        else
        {
            if (COL4_FLAG)
                COL4 = null;
            COL4_FLAG = false;
        }

        if (C5 == 1)
        {
            if (!COL5_FLAG)
            {
                COL5 = LoadLF(COL5_FILE, LFUW);
            }
            COL5_FLAG = true;
        }
        else
        {
            if (COL5_FLAG)
                COL5 = null;
            COL5_FLAG = false;
        }

        if (C6 == 1)
        {
            if (!COL6_FLAG)
            {
                COL6 = LoadLF(COL6_FILE, LFUW);
            }
            COL6_FLAG = true;
        }
        else
        {
            if (COL6_FLAG)
                COL6 = null;
            COL6_FLAG = false;
        }

        if (C7 == 1)
        {
            if (!COL7_FLAG)
            {
                COL7 = LoadLF(COL7_FILE, LFUW);
            }
            COL7_FLAG = true;
        }
        else
        {
            if (COL7_FLAG)
                COL7 = null;
            COL7_FLAG = false;
        }

        if (C8 == 1)
        {
            if (!COL8_FLAG)
            {
                COL8 = LoadLF(COL8_FILE, LFUW);
            }
            COL8_FLAG = true;
        }
        else
        {
            if (COL8_FLAG)
                COL8 = null;
            COL8_FLAG = false;
        }

        if (C9 == 1)
        {
            if (!COL9_FLAG)
            {
                COL9 = LoadLF(COL9_FILE, LFUW);
            }
            COL9_FLAG = true;
        }
        else
        {
            if (COL9_FLAG)
                COL9 = null;
            COL9_FLAG = false;
        }

        if (C10 == 1)
        {
            if (!COL10_FLAG)
            {
                COL10 = LoadLF(COL10_FILE, LFUW);
            }
            COL10_FLAG = true;
        }
        else
        {
            if (COL10_FLAG)
                COL10 = null;
            COL10_FLAG = false;
        }

        if (C11 == 1)
        {
            if (!COL11_FLAG)
            {
                COL11 = LoadLF(COL11_FILE, LFUW);
            }
            COL11_FLAG = true;
        }
        else
        {
            if (COL11_FLAG)
                COL11 = null;
            COL11_FLAG = false;
        }

        if (C12 == 1)
        {
            if (!COL12_FLAG)
            {
                COL12 = LoadLF(COL12_FILE, LFUW);
            }
            COL12_FLAG = true;
        }
        else
        {
            if (COL12_FLAG)
                COL12 = null;
            COL12_FLAG = false;
        }

        if (C13 == 1)
        {
            if (!COL13_FLAG)
            {
                COL13 = LoadLF(COL13_FILE, LFUW);
            }
            COL13_FLAG = true;
        }
        else
        {
            if (COL13_FLAG)
                COL13 = null;
            COL13_FLAG = false;
        }

        if (C14 == 1)
        {
            if (!COL14_FLAG)
            {
                COL14 = LoadLF(COL14_FILE, LFUW);
            }
            COL14_FLAG = true;
        }
        else
        {
            if (COL14_FLAG)
                COL14 = null;
            COL14_FLAG = false;
        }

        if (C15 == 1)
        {
            if (!COL15_FLAG)
            {
                COL15 = LoadLF(COL15_FILE, LFUW);
            }
            COL15_FLAG = true;
        }
        else
        {
            if (COL15_FLAG)
                COL15 = null;
            COL15_FLAG = false;
        }

        if (C16 == 1)
        {
            if (!COL16_FLAG)
            {
                COL16 = LoadLF(COL16_FILE, LFUW);
            }
            COL16_FLAG = true;
        }
        else
        {
            if (COL16_FLAG)
                COL16 = null;
            COL16_FLAG = false;
        }

        if (C17 == 1)
        {
            if (!COL17_FLAG)
            {
                COL17 = LoadLF(COL17_FILE, LFUW);
            }
            COL17_FLAG = true;
        }
        else
        {
            if (COL17_FLAG)
                COL17 = null;
            COL17_FLAG = false;
        }

        if (C18 == 1)
        {
            if (!COL18_FLAG)
            {
                COL18 = LoadLF(COL18_FILE, LFUW);
            }
            COL18_FLAG = true;
        }
        else
        {
            if (COL18_FLAG)
                COL18 = null;
            COL18_FLAG = false;
        }
    }


    public static void LFUpdate(int REGION)
    {
        sw_LFUpdate.Reset();
        sw_LFUpdate.Start();
        if (REGION == 0)
        {
            sub_LFUpdate_ROW(1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0); // 1, 2, 3, 4, 5, 6
            sub_LFUpdate_COL(0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1); // 5, 6, 11, 12, 17, 18
        }
        else if (REGION == 1)
        {
            sub_LFUpdate_ROW(1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0);
            sub_LFUpdate_COL(0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1);
        }
        else if (REGION == 2)
        {
            sub_LFUpdate_ROW(1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0);
            sub_LFUpdate_COL(0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1);
        }
        else if (REGION == 3)
        {
            sub_LFUpdate_ROW(1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0);
            sub_LFUpdate_COL(0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1);
        }
        else if (REGION == 4)
        {
            sub_LFUpdate_ROW(0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0);
            sub_LFUpdate_COL(0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0);
        }
        else if (REGION == 5)
        {
            sub_LFUpdate_ROW(0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0);
            sub_LFUpdate_COL(0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0);
        }
        else if (REGION == 6)
        {
            sub_LFUpdate_ROW(0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0);
            sub_LFUpdate_COL(0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0);
        }
        else if (REGION == 7)
        {
            sub_LFUpdate_ROW(0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0);
            sub_LFUpdate_COL(0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0);
        }
        else if (REGION == 8)
        {
            sub_LFUpdate_ROW(0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1);
            sub_LFUpdate_COL(1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0);
        }
        else if (REGION == 9)
        {
            sub_LFUpdate_ROW(0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1);
            sub_LFUpdate_COL(1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0);
        }
        else if (REGION == 10)
        {
            sub_LFUpdate_ROW(0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1);
            sub_LFUpdate_COL(1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0);
        }
        else if (REGION == 11)
        {
            sub_LFUpdate_ROW(0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1);
            sub_LFUpdate_COL(1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0);
        }

        //auto end_u = std::chrono::system_clock::now();
        //std::chrono::microseconds delta_u = std::chrono::duration_cast<std::chrono::microseconds>(end_u - start_u);

        //printf("UPDATE : %d\n", delta_u.count());

        sw_GC.Restart();
        //System.GC.Collect();
        sw_GC.Stop();

        sw_LFUpdate.Stop();

        UnityEngine.Debug.Log("- LF Update : " + sw_LFUpdate.ElapsedTicks / 10 + " us");
        UnityEngine.Debug.Log("LOAD DONE !!");
    }

    public static byte[] LoadLF(string inFile, int N)
    {

        sw_LoadLF.Reset();
        sw_LoadLF.Start();
        byte[] imageData = System.IO.File.ReadAllBytes(inFile);
        sw_LoadLF.Stop();
        UnityEngine.Debug.Log("LoadLF : " + sw_LoadLF.ElapsedTicks / 10 + " us");

        return imageData;
    }

    public static byte inter8(byte[] LF, double P_r, int P_1, int P_2, double U_r, int U_1, int U_2, double H_r, int H_1, int H_2, int c)
    {
        if (P_r == -1 || U_r == -1 || H_r == -1)
        {
            return 0;
        }
        else
        {
            if (P_r == 1) P_1 = 0;
            else if (P_r == 0) P_2 = 0;

            if (U_r == 1) U_1 = 0;
            else if (U_r == 0) U_2 = 0;

            if (H_r == 1) H_1 = 0;
            else if (H_r == 0) H_2 = 0;


            if (c == 0)
            {
                return (byte)(
                    ((1.0 - P_r) *
                    ((1.0 - U_r) * ((1.0 - H_r) * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 0] + H_r * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_2 * 3 + 0]) +
                            ((U_r) * ((1.0 - H_r) * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_1 * 3 + 0] + H_r * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_2 * 3 + 0])))) +
                        ((P_r) *
                    ((1.0 - U_r) * ((1.0 - H_r) * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 0] + H_r * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_2 * 3 + 0]) +
                          ((U_r) * ((1.0 - H_r) * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_1 * 3 + 0] + H_r * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_2 * 3 + 0])))));
            }
            else if (c == 1)
            {
                return (byte)(
                    ((1.0 - P_r) *
                    ((1.0 - U_r) * ((1.0 - H_r) * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 1] + H_r * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_2 * 3 + 1]) +
                          ((U_r) * ((1.0 - H_r) * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_1 * 3 + 1] + H_r * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_2 * 3 + 1])))) +
                        ((P_r) *
                    ((1.0 - U_r) * ((1.0 - H_r) * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 1] + H_r * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_2 * 3 + 1]) +
                          ((U_r) * ((1.0 - H_r) * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_1 * 3 + 1] + H_r * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_2 * 3 + 1])))));
            }
            else
            {
                return (byte)(
                    ((1.0 - P_r) *
                    ((1.0 - U_r) * ((1.0 - H_r) * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 2] + H_r * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_2 * 3 + 2]) +
                          ((U_r) * ((1.0 - H_r) * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_1 * 3 + 2] + H_r * LF[(P_1) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_2 * 3 + 2])))) +
                        ((P_r) *
                    ((1.0 - U_r) * ((1.0 - H_r) * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 2] + H_r * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_2 * 3 + 2]) +
                          ((U_r) * ((1.0 - H_r) * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_1 * 3 + 2] + H_r * LF[(P_2) * (HEIGHT * WIDTH * 3) + U_2 * (HEIGHT * 3) + H_2 * 3 + 2])))));

            }
        }
    }

    public static byte[] RenderingLF_SinglePixel(byte[] LF, int POS_X, int POS_Y, double f, double times, int order, int dir, ref int outputWidthLen, ref int a_FrontIdx)
    {
        double Alloc_Angle_s = 0.0;
        double Alloc_Angle_e = 0.0;

        Alloc_Angle_s = Math.Atan2((-1.0 * LFUW / 2 - POS_X), (LFUW / 2 - POS_Y));
        Alloc_Angle_e = Math.Atan2((1.0 * LFUW / 2 - POS_X), (LFUW / 2 - POS_Y));

        if (POS_Y == 50)
        {
            Alloc_Angle_s = -1 * Math.PI / 2;
            Alloc_Angle_e = Math.PI / 2;
        }

        //Alloc_Angle_s = Alloc_Angle_s - 0.174533; //ABOUT 10 DEGREES
        //Alloc_Angle_e = Alloc_Angle_e + 0.174533; //ABOUT 10 DEGREES

        //if (Alloc_Angle_s < -1 * PI / 2)	Alloc_Angle_s = -1 * PI / 2;
        //if (Alloc_Angle_e >		 PI / 2)	Alloc_Angle_e =		 PI / 2;

        int Y = LFUW / 2;
        int X_S = -1 * LFUW / 2;
        int X_E = LFUW / 2;

        a_FrontIdx = 0;

        int out_w = 0;
        for (double a = Alloc_Angle_s; a < Alloc_Angle_e; a = a + 0.0025)
        {
            double P = (double)(Y - POS_Y) * Math.Tan(a) + POS_X;
            if (P >= X_S && P < X_E)
            {
                out_w++;
                if (a <= 0) a_FrontIdx++;
            }
        } // a = 0일때의 값을 밖으로 뽑아내서, 그 부분까지 크롭 후 Left's Right로


        outputWidthLen = out_w; // arg "outputWidthLen" is used to deliver the modified width to the Texture2D (082219)

        //out_w = (int)((Alloc_Angle_e - Alloc_Angle_s) / 0.0025);

        byte[] IMG = new byte[HEIGHT * outputWidthLen * 3];

        //Mat IMG = new Mat(HEIGHT, out_w, MatType.CV_8UC3);
        //Image IMG = new Bitmap(out_w, HEIGHT);
        //Bitmap bmp = IMG as Bitmap;

        sw_ParallelFor.Restart();
#if USE_CUDA
        int errorCode = CudaParallelFor(IMG, LF, Alloc_Angle_s, times, Y, POS_Y, POS_X, LFUW, DATAW, WIDTH, HEIGHT, out_w, dir, LF.Length);
        UnityEngine.Debug.Log("\t\t\tERROR CODE " + errorCode.ToString());
#else
        Parallel.For(0, out_w, (w) =>
        //for (int w = 0; w < out_w; w++)
        {
            //Console.WriteLine("{0}: {1}", Thread.CurrentThread.ManagedThreadId, w);
            double a = Alloc_Angle_s + (0.0025 * (double)w);
            double P = (double)(Y - POS_Y) * Math.Tan(a) + POS_X;
            double b = Math.Sqrt(2.0) * LFUW;
            double N_dist = Math.Sqrt((double)((P - POS_X) * (P - POS_X) + (Y - POS_Y) * (Y - POS_Y))) / b;

            P = P / 2;
            int P_1 = (int)(Math.Round(P + (DATAW / 2)));
            if (dir == 3 || dir == 4) P_1 = DATAW - P_1 - 1;

            double U = a * (180.0 / Math.PI) * (1.0 / 180.0) * WIDTH / 2 + WIDTH / 2; ;
            int U_1 = (int)(Math.Round(U));

            if (dir == 2) U_1 = U_1 + WIDTH / 4;
            if (dir == 3) U_1 = U_1 + WIDTH / 2;
            if (dir == 4) U_1 = U_1 - WIDTH / 4;

            if (U_1 >= WIDTH) U_1 = U_1 - WIDTH;
            else if (U_1 < 0) U_1 = U_1 + WIDTH;

            if (P_1 >= DATAW) P_1 = DATAW - 1;
            else if (P_1 < 0) P_1 = 0;

            if (U_1 >= WIDTH) U_1 = WIDTH - 1;
            else if (U_1 < 0) U_1 = 0;

            int N_off = (int)(Math.Floor(times * N_dist + 0.5)) >> 1;
            double N_H_r = (double)(HEIGHT + N_off) / HEIGHT;

            for (int h = 0; h < HEIGHT; h++)
            {
                double h_n = (h - HEIGHT / 2) * N_H_r + HEIGHT / 2;

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

                int H_1 = (int)(Math.Round(h_n));
                if (H_1 >= HEIGHT) H_1 = HEIGHT - 1;
                else if (H_1 < 0) H_1 = 0;

                byte PEL_0 = LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 0]; // b
                byte PEL_1 = LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 1]; // g
                byte PEL_2 = LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 2]; // r

                IMG[(HEIGHT - h - 1) * (out_w * 3) + w * 3 + 2] = PEL_0; // r
                IMG[(HEIGHT - h - 1) * (out_w * 3) + w * 3 + 1] = PEL_1; // g
                IMG[(HEIGHT - h - 1) * (out_w * 3) + w * 3 + 0] = PEL_2; // b 
                // Unity use the big endian color format -> 0xRR 0xGG 0xBB ... 
                // Unity use the Bottom-Left origin in 2D coor. (h -> HEIGHT - h) (082219)
            }
        }
        );
#endif
        sw_ParallelFor.Stop();

        return IMG;
    }

    public static byte[] RenderingLF_SinglePixel(byte[] LF, int POS_X, int POS_Y, double f, double times, int order, int dir, ref int outputWidthLen)
    {
        double Alloc_Angle_s = 0.0;
        double Alloc_Angle_e = 0.0;

        Alloc_Angle_s = Math.Atan2((-1.0 * LFUW / 2 - POS_X), (LFUW / 2 - POS_Y));
        Alloc_Angle_e = Math.Atan2((1.0 * LFUW / 2 - POS_X), (LFUW / 2 - POS_Y));

        if (POS_Y == 50)
        {
            Alloc_Angle_s = -1 * Math.PI / 2;
            Alloc_Angle_e = Math.PI / 2;
        }

        //Alloc_Angle_s = Alloc_Angle_s - 0.174533; //ABOUT 10 DEGREES
        //Alloc_Angle_e = Alloc_Angle_e + 0.174533; //ABOUT 10 DEGREES

        //if (Alloc_Angle_s < -1 * PI / 2)	Alloc_Angle_s = -1 * PI / 2;
        //if (Alloc_Angle_e >		 PI / 2)	Alloc_Angle_e =		 PI / 2;

        int Y = LFUW / 2;
        int X_S = -1 * LFUW / 2;
        int X_E = LFUW / 2;


        int out_w = 0;
        for (double a = Alloc_Angle_s; a < Alloc_Angle_e; a = a + 0.0025)
        {
            double P = (double)(Y - POS_Y) * Math.Tan(a) + POS_X;
            if (P >= X_S && P < X_E)
                out_w++;
        } // a = 0일때의 값을 밖으로 뽑아내서, 그 부분까지 크롭 후 Left's Right로

        outputWidthLen = out_w; // arg "outputWidthLen" is used to deliver the modified width to the Texture2D (082219)

        //out_w = (int)((Alloc_Angle_e - Alloc_Angle_s) / 0.0025);

        byte[] IMG = new byte[HEIGHT * outputWidthLen * 3];

        //Mat IMG = new Mat(HEIGHT, out_w, MatType.CV_8UC3);
        //Image IMG = new Bitmap(out_w, HEIGHT);
        //Bitmap bmp = IMG as Bitmap;

        sw_ParallelFor.Restart();
#if USE_CUDA
        int errorCode = CudaParallelFor(IMG, LF, Alloc_Angle_s, times, Y, POS_Y, POS_X, LFUW, DATAW, WIDTH, HEIGHT, out_w, dir, LF.Length);
        UnityEngine.Debug.Log("\t\t\tERROR CODE " + errorCode.ToString());
#else
        Parallel.For(0, out_w, (w) =>
        //for (int w = 0; w < out_w; w++)
        {
            //Console.WriteLine("{0}: {1}", Thread.CurrentThread.ManagedThreadId, w);
            double a = Alloc_Angle_s + (0.0025 * (double)w);
            double P = (double)(Y - POS_Y) * Math.Tan(a) + POS_X;
            double b = Math.Sqrt(2.0) * LFUW;
            double N_dist = Math.Sqrt((double)((P - POS_X) * (P - POS_X) + (Y - POS_Y) * (Y - POS_Y))) / b;

            P = P / 2;
            int P_1 = (int)(Math.Round(P + (DATAW / 2)));
            if (dir == 3 || dir == 4) P_1 = DATAW - P_1 - 1;

            double U = a * (180.0 / Math.PI) * (1.0 / 180.0) * WIDTH / 2 + WIDTH / 2; ;
            int U_1 = (int)(Math.Round(U));

            if (dir == 2) U_1 = U_1 + WIDTH / 4;
            if (dir == 3) U_1 = U_1 + WIDTH / 2;
            if (dir == 4) U_1 = U_1 - WIDTH / 4;

            if (U_1 >= WIDTH) U_1 = U_1 - WIDTH;
            else if (U_1 < 0) U_1 = U_1 + WIDTH;

            if (P_1 >= DATAW) P_1 = DATAW - 1;
            else if (P_1 < 0) P_1 = 0;

            if (U_1 >= WIDTH) U_1 = WIDTH - 1;
            else if (U_1 < 0) U_1 = 0;

            int N_off = (int)(Math.Floor(times * N_dist + 0.5)) >> 1;
            double N_H_r = (double)(HEIGHT + N_off) / HEIGHT;

            for (int h = 0; h < HEIGHT; h++)
            {
                double h_n = (h - HEIGHT / 2) * N_H_r + HEIGHT / 2;

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

                int H_1 = (int)(Math.Round(h_n));
                if (H_1 >= HEIGHT) H_1 = HEIGHT - 1;
                else if (H_1 < 0) H_1 = 0;

                byte PEL_0 = LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 0]; // b
                byte PEL_1 = LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 1]; // g
                byte PEL_2 = LF[(P_1) * (HEIGHT * WIDTH * 3) + U_1 * (HEIGHT * 3) + H_1 * 3 + 2]; // r

                IMG[(HEIGHT - h - 1) * (out_w * 3) + w * 3 + 2] = PEL_0; // r
                IMG[(HEIGHT - h - 1) * (out_w * 3) + w * 3 + 1] = PEL_1; // g
                IMG[(HEIGHT - h - 1) * (out_w * 3) + w * 3 + 0] = PEL_2; // b 
                // Unity use the big endian color format -> 0xRR 0xGG 0xBB ... 
                // Unity use the Bottom-Left origin in 2D coor. (h -> HEIGHT - h) (082219)
            }
        }
        );
#endif
        sw_ParallelFor.Stop();

        return IMG;
    }

    public static void SetRegion()
    {
        if (GLO_X >= 0 && GLO_X <= 100 && GLO_Y >= 0 && GLO_Y <= 100)
        {
            LF_F = ROW3;
            LF_R = COL12;
            LF_B = ROW1;
            LF_L = COL18;

            POS_X = GLO_X - 50;
            POS_Y = GLO_Y - 50;

            REGION = 0;
        }
        else if (GLO_X > 100 && GLO_X <= 200 && GLO_Y >= 0 && GLO_Y <= 100)
        {
            LF_F = ROW4;
            LF_R = COL6;
            LF_B = ROW2;
            LF_L = COL12;

            POS_X = GLO_X - 150;
            POS_Y = GLO_Y - 50;

            REGION = 1;
        }
        else if (GLO_X >= 0 && GLO_X <= 100 && GLO_Y > 100 && GLO_Y <= 200)
        {
            LF_F = ROW5;
            LF_R = COL11;
            LF_B = ROW3;
            LF_L = COL17;

            POS_X = GLO_X - 50;
            POS_Y = GLO_Y - 150;

            REGION = 2;
        }
        else if (GLO_X > 100 && GLO_X <= 200 && GLO_Y > 100 && GLO_Y <= 200)
        {
            LF_F = ROW6;
            LF_R = COL5;
            LF_B = ROW4;
            LF_L = COL11;

            POS_X = GLO_X - 150;
            POS_Y = GLO_Y - 150;

            REGION = 3;
        }
        else if (GLO_X >= 0 && GLO_X <= 100 && GLO_Y > 200 && GLO_Y <= 300)
        {
            LF_F = ROW7;
            LF_R = COL10;
            LF_B = ROW5;
            LF_L = COL16;

            POS_X = GLO_X - 50;
            POS_Y = GLO_Y - 250;

            REGION = 4;
        }
        else if (GLO_X > 100 && GLO_X <= 200 && GLO_Y > 200 && GLO_Y <= 300)
        {
            LF_F = ROW8;
            LF_R = COL4;
            LF_B = ROW6;
            LF_L = COL10;

            POS_X = GLO_X - 150;
            POS_Y = GLO_Y - 250;

            REGION = 5;
        }
        else if (GLO_X >= 0 && GLO_X <= 100 && GLO_Y > 300 && GLO_Y <= 400)
        {
            LF_F = ROW9;
            LF_R = COL9;
            LF_B = ROW7;
            LF_L = COL15;

            POS_X = GLO_X - 50;
            POS_Y = GLO_Y - 350;

            REGION = 6;
        }
        else if (GLO_X > 100 && GLO_X <= 200 && GLO_Y > 300 && GLO_Y <= 400)
        {
            LF_F = ROW10;
            LF_R = COL3;
            LF_B = ROW8;
            LF_L = COL9;

            POS_X = GLO_X - 150;
            POS_Y = GLO_Y - 350;

            REGION = 7;
        }
        else if (GLO_X >= 0 && GLO_X <= 100 && GLO_Y > 400 && GLO_Y <= 500)
        {
            LF_F = ROW11;
            LF_R = COL8;
            LF_B = ROW9;
            LF_L = COL14;

            POS_X = GLO_X - 50;
            POS_Y = GLO_Y - 450;

            REGION = 8;
        }
        else if (GLO_X > 100 && GLO_X <= 200 && GLO_Y > 400 && GLO_Y <= 500)
        {
            LF_F = ROW12;
            LF_R = COL2;
            LF_B = ROW10;
            LF_L = COL8;

            POS_X = GLO_X - 150;
            POS_Y = GLO_Y - 450;

            REGION = 9;
        }
        else if (GLO_X >= 0 && GLO_X <= 100 && GLO_Y > 500 && GLO_Y <= 600)
        {
            LF_F = ROW13;
            LF_R = COL7;
            LF_B = ROW11;
            LF_L = COL13;

            POS_X = GLO_X - 50;
            POS_Y = GLO_Y - 550;

            REGION = 10;
        }
        else if (GLO_X > 100 && GLO_X <= 200 && GLO_Y > 500 && GLO_Y <= 600)
        {
            LF_F = ROW14;
            LF_R = COL1;
            LF_B = ROW12;
            LF_L = COL7;

            POS_X = GLO_X - 150;
            POS_Y = GLO_Y - 550;

            REGION = 11;
        }
        else
        {
            GLO_Y = Mathf.Clamp(GLO_Y, lowerLimit_gloY, upperLimit_gloY);
            GLO_X = Mathf.Clamp(GLO_X, lowerLimit_gloX, upperLimit_gloX);
            //print("out of range\n");
            // REGION = PREV_REGION;
        }

    }

    public static void MainFunc(ref Texture2D tex2D, ref GameObject gameobj)
    {
        sw_MainFunc.Reset();
        sw_MainFunc.Start();
        if (REGION != PREV_REGION)
        {
            //Task task = new Task(() => LFUpdate_prevcase(REGION));
            Task task = new Task(() => LFUpdate(REGION));
            task.Start();

        }
        // LFUpdate(REGION);
        PREV_REGION = REGION;

        int POS_F_X = POS_X; int POS_F_Y = POS_Y;
        int POS_R_X = -1 * POS_Y; int POS_R_Y = POS_X;
        int POS_B_X = -1 * POS_X; int POS_B_Y = -1 * POS_Y;
        int POS_L_X = POS_Y; int POS_L_Y = -1 * POS_X;

        int outWidthLen_F = 0;
        int outWidthLen_R = 0;
        int outWidthLen_B = 0;
        int outWidthLen_L = 0;
        int outWidLen = 0;

        int frontDivIdx = 0;

        sw_Rendering.Reset();
        sw_ParallelFor.Reset();
        sw_Rendering.Start();

        byte[] OUT_F = RenderingLF_SinglePixel(LF_F, POS_F_X, POS_F_Y, f, times, 0, 1, ref outWidthLen_F, ref frontDivIdx);
        // byte[] OUT_F = RenderingLF_SinglePixel(LF_F, POS_F_X, POS_F_Y, f, times, 0, 1, ref outWidthLen_F);
        byte[] OUT_R = RenderingLF_SinglePixel(LF_R, POS_R_X, POS_R_Y, f, times, 0, 2, ref outWidthLen_R);
        byte[] OUT_B = RenderingLF_SinglePixel(LF_B, POS_B_X, POS_B_Y, f, times, 0, 3, ref outWidthLen_B);
        byte[] OUT_L = RenderingLF_SinglePixel(LF_L, POS_L_X, POS_L_Y, f, times, 0, 4, ref outWidthLen_L);

        sw_Rendering.Stop();

        outWidLen = outWidthLen_F + outWidthLen_R + outWidthLen_B + outWidthLen_L;



        byte[] panoBGR = new byte[HEIGHT * outWidLen * 3]; // 4 frames

        int offset = 0;

        for (int h = 0; h < HEIGHT; h++)
        {
            // Buffer.BlockCopy(OUT_F, h * outWidthLen_F * 3, panoBGR, offset, outWidthLen_F * 3);
            // offset += outWidthLen_F * 3;
            // Buffer.BlockCopy(OUT_R, h * outWidthLen_R * 3, panoBGR, offset, outWidthLen_R * 3);
            // offset += outWidthLen_R * 3;
            // Buffer.BlockCopy(OUT_B, h * outWidthLen_B * 3, panoBGR, offset, outWidthLen_B * 3);
            // offset += outWidthLen_B * 3;
            // Buffer.BlockCopy(OUT_L, h * outWidthLen_L * 3, panoBGR, offset, outWidthLen_L * 3);
            // offset += outWidthLen_L * 3;

            //  move Front's Left to the Left's Right side
            //  int rightHalfOffset_F = ((outWidthLen_F - frontDivIdx) * 3) + (h * outWidthLen_F * 3);
            //  int rightHalfSize_F = ((h + 1) * outWidthLen_F * 3) - rightHalfOffset_F;
            //  int leftHalfOffset_F = h * outWidthLen_F * 3;
            //  int leftHalfSize_F = outWidthLen_F * 3 - rightHalfSize_F;
            // 
            // Buffer.BlockCopy(OUT_F, rightHalfOffset_F, panoBGR, offset, rightHalfSize_F);
            // offset += rightHalfSize_F;
            // Buffer.BlockCopy(OUT_R, h * outWidthLen_R * 3, panoBGR, offset, outWidthLen_R * 3);
            // offset += outWidthLen_R * 3;
            // Buffer.BlockCopy(OUT_B, h * outWidthLen_B * 3, panoBGR, offset, outWidthLen_B * 3);
            // offset += outWidthLen_B * 3;
            // Buffer.BlockCopy(OUT_L, h * outWidthLen_L * 3, panoBGR, offset, outWidthLen_L * 3);
            // offset += outWidthLen_L * 3;
            // Buffer.BlockCopy(OUT_F, leftHalfOffset_F, panoBGR, offset, leftHalfSize_F);
            // offset += leftHalfSize_F;
            int leftOffset_F = h * outWidthLen_F * 3;
            int leftSize_F = (frontDivIdx) * 3;

            int rightOffset_F = leftOffset_F + leftSize_F;
            int rightSize_F = outWidthLen_F * 3 - leftSize_F;


            Buffer.BlockCopy(OUT_F, rightOffset_F, panoBGR, offset, rightSize_F);
            offset += rightSize_F;
            Buffer.BlockCopy(OUT_R, h * outWidthLen_R * 3, panoBGR, offset, outWidthLen_R * 3);
            offset += outWidthLen_R * 3;
            Buffer.BlockCopy(OUT_B, h * outWidthLen_B * 3, panoBGR, offset, outWidthLen_B * 3);
            offset += outWidthLen_B * 3;
            Buffer.BlockCopy(OUT_L, h * outWidthLen_L * 3, panoBGR, offset, outWidthLen_L * 3);
            offset += outWidthLen_L * 3;
            Buffer.BlockCopy(OUT_F, leftOffset_F, panoBGR, offset, leftSize_F);
            offset += leftSize_F;
        }

        tex2D = new Texture2D(outWidLen, HEIGHT, TextureFormat.RGB24, false);
        tex2D.LoadRawTextureData(panoBGR);
        tex2D.Apply();
        gameobj.GetComponent<Renderer>().material.mainTexture = tex2D;

        sw_MainFunc.Stop();
        // UnityEngine.Debug.Log(" - - - MainFunc : " + sw_MainFunc.ElapsedTicks / 10 + " us");
    }

    static void DisplayMemory()
    {
        using (System.IO.StreamWriter log_mUse = new System.IO.StreamWriter("C:/myUnity/timelog/MemoryUsage.txt", true))
            log_mUse.WriteLine((GC.GetTotalMemory(true)).ToString());

    }
}


