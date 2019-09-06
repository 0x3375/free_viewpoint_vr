using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FiveDoF_Cpp : MonoBehaviour
{
    private const string NATIVE_LIBRARY_NAME = "test";
    [DllImport(NATIVE_LIBRARY_NAME)]
    private static extern int nativeSetNumOfFrames(int n);
    [DllImport(NATIVE_LIBRARY_NAME)]
    private static extern void nativeSetPath(string dir);
    [DllImport(NATIVE_LIBRARY_NAME)]
    private static extern void nativeGetLFSpace(IntPtr data, int idx);
    
    string path = "S2/";
    public int maxImgIdx = 100;
    public int width = 2048;
    public int height = 1024;
    
    private int curPos;

    private Texture2D tex;
    private Color32[] pixel32;
    
    private GCHandle pixelHandle;
    private IntPtr pixelPtr;

    GameObject LFS = null; // 3D 구의 LF Space

    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
    private int i = 0;

    void Start() // 플레이버튼 누를 시 최초 1회만 실행
    {
        LFS = GameObject.Find("LFSpace"); // LF
        LFS.GetComponent<Renderer>().enabled = true;
        LFS.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Pano360Shader"); // 구에 입혀진 텍스쳐를 렌더링하기 위해 쉐이더 로드

        Debug.Log(nativeSetNumOfFrames(maxImgIdx)); // NO ERROR HERE
        nativeSetPath(path);

        curPos = maxImgIdx / 2;
        
        InitTexture();
        LFS.GetComponent<Renderer>().material.mainTexture = tex;
        MatToTexture2D(curPos);
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) // 우측
        {
            // sw.Start();
            curPos = curPos + 1;
            if (curPos > maxImgIdx) curPos = maxImgIdx;
            MatToTexture2D(curPos);
            // sw.Stop();
            // Debug.Log(sw.ElapsedTicks / 10 + "us (" + i + ")");
            // i = i + 1;
        }

        if (Input.GetKey(KeyCode.D)) // 좌측
        {
            //sw.Start();
            curPos = curPos - 1;
            if (curPos < 1) curPos = 1;
            MatToTexture2D(curPos);
            //sw.Stop();
            //Debug.Log(sw.ElapsedTicks / 10 + "us (" + i + ")");
            //i = i + 1;
        }

        sw.Reset();
    }
    
    void InitTexture()
    { 
        tex = new Texture2D(width, height, TextureFormat.BGRA32, false);
        pixel32 = tex.GetPixels32();
        //Pin pixel32 array
        pixelHandle = GCHandle.Alloc(pixel32, GCHandleType.Pinned);
        //Get the pinned address
        pixelPtr = pixelHandle.AddrOfPinnedObject();
    }
   
    void MatToTexture2D(int currentPosition)
    {
        //Convert Mat to Texture2D
        sw.Start();
        nativeGetLFSpace(pixelPtr, currentPosition);
        sw.Stop();
        Debug.Log(sw.ElapsedTicks / 10 + "us (" + i + ")");
        i = i + 1;
        //Update the Texture2D with array updated in C++
        tex.SetPixels32(pixel32);
        tex.Apply();

        LFS.GetComponent<Renderer>().material.mainTexture = tex;
    }
   
    void OnApplicationQuit()
    {
        //Free handle
        pixelHandle.Free();
    }

}

