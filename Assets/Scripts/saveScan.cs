﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class saveScan : MonoBehaviour
{
    [SerializeField] private Canvas display;
    [SerializeField] private RenderTexture texture360;

    private Texture2D texture180;
     

    public void save()
    {
        byte[] bytes;

        if (display.transform.GetChild(0).gameObject.activeSelf)
        {
            bytes = texture180.EncodeToPNG();
        }
        else
        {
            bytes = toTexture2D(texture360).EncodeToPNG();
        }


        string filePath = Application.persistentDataPath + "/laserScans";

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        string filename = string.Format("{0}/snap_{1}.png",
            filePath,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

        File.WriteAllBytes(filename, bytes);

        display.enabled = false;
    }

    private Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex;
        if(display.transform.GetChild(0).gameObject.activeSelf)
            tex = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
        else
            tex = new Texture2D(1024, 1024, TextureFormat.RGB24, false);

        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }


    public void close()
    {
        //Debug.Log("test");
        display.enabled = false;
    }

    public void saveTexture180(Texture2D input)
    {
        texture180 = input;
    }
}
