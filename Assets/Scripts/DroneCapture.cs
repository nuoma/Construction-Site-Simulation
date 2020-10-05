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

        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = Camera.targetTexture;

        Camera.Render();

        Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height, TextureFormat.RGB24, false, true);
        image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToJPG();
        Destroy(image);

        //File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + fileCounter + ".png", bytes);
        File.WriteAllBytes(fileName, bytes);

        capture = false;
    }
}
   