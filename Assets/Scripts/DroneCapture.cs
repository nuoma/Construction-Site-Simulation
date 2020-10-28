using System.Collections;
using System.IO;
using UnityEngine;

//Reference: forum.unity.com/threads/how-to-save-manually-save-a-png-of-a-camera-view.506269/ 

public class DroneCapture : MonoBehaviour
{

    string filePath;
    string fileName;
    public Camera Camera;
    public bool capture;
    [SerializeField] RenderTexture rt;
    [SerializeField] Camera cam;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/DroneImages";

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
    }
    private void LateUpdate()
    {
        if (capture)
        {
            Capture();
        }
    }

    public void Capture()
    {
        fileName = string.Format("{0}/{1}IMG_{2}.jpg",
    filePath, gameObject.transform.parent.gameObject.transform.parent.gameObject.name,
    System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

        //mod from https://gamedev.stackexchange.com/questions/184785/saving-png-from-render-texture-results-in-much-darker-image
        //rt is render  texture
        RenderTexture mRt = new RenderTexture(rt.width, rt.height, rt.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        mRt.antiAliasing = rt.antiAliasing;

        var tex = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);
        cam.targetTexture = mRt;
        cam.Render();
        RenderTexture.active = mRt;

        tex.ReadPixels(new Rect(0, 0, mRt.width, mRt.height), 0, 0);
        tex.Apply();

        
        File.WriteAllBytes(fileName, tex.EncodeToPNG());
        Debug.Log("Saved file to: " + filePath);

        DestroyImmediate(tex);

        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;

        DestroyImmediate(mRt);

        /*
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = Camera.targetTexture;

        Camera.Render();

        Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height, TextureFormat.ARGB32, false, true);
        image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToJPG();
        Destroy(image);

        //File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + fileCounter + ".png", bytes);
        File.WriteAllBytes(fileName, bytes);

        capture = false;
        */
    }
}
   