using System.IO;
using UnityEngine;
 
public class DroneCapture : MonoBehaviour
{
    public KeyCode screenshotKey;
    public Camera _camera;
    string filePath;
    string fileName;
    public bool capture = false; 

    void Start()
    {
        _camera = GetComponent<Camera>();
        Debug.Log(Application.persistentDataPath);
        filePath = Application.persistentDataPath + "/DroneImages";

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
    }

    
    private void LateUpdate()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            Capture();
        }
    }
    

    public void Capture()
    {
        fileName = string.Format("{0}/DroneIMG_{1}.png",
    filePath,
    System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

        RenderTexture activeRenderTexture = RenderTexture.active;
        Debug.Log(_camera);
        RenderTexture.active = _camera.targetTexture;

        _camera.Render();

        Texture2D image = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToPNG();
        Destroy(image);

        Debug.Log(bytes);
        //Debug.Log(Application.persistentDataPath);
        File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "output.png"), bytes);
        //File.WriteAllBytes(fileName, bytes);
    }
}