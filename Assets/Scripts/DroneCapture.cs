using System.Collections;
using System.IO;
using UnityEngine;
 
public class DroneCapture : MonoBehaviour
{
    public KeyCode screenshotKey;
    public Camera _camera;
    string filePath;
    string fileName;
    string fullpath;
    public bool capture = false;
    public RenderTexture JobsiteDroneTexture;

    void Start()
    {
        _camera = GetComponent<Camera>();
        Debug.Log(Application.persistentDataPath);
        filePath = Application.persistentDataPath + "/DroneImages";

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        fullpath = Application.persistentDataPath + "/DroneImages/output.png";
    }

    
    private void LateUpdate()
    {
        if (capture)
        {
            StartCoroutine(TakeSnapshot());
            //Capture();
            //SaveTextureAsPNG(JobsiteDroneTexture,fullpath);
        }
    }
    

    public void Capture()
    {
        fileName = string.Format("{0}/DroneIMG_{1}.png",
    filePath,
    System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

        //RenderTexture activeRenderTexture = RenderTexture.active;
        //Debug.Log(_camera);
        //RenderTexture.active = _camera.targetTexture;
        //_camera.Render();

        //JobsiteDroneTexture

        Texture2D image = new Texture2D(JobsiteDroneTexture.width, JobsiteDroneTexture.height);
        image.ReadPixels(new Rect(0, 0, JobsiteDroneTexture.width, JobsiteDroneTexture.height), 0, 0);
        image.Apply();
        //RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToPNG();
        //Destroy(image);
        File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "output.png"), bytes);
    }

    /*
    public static void SaveTextureAsPNG(RenderTexture _texture, string _fullPath)
    {
        myTexture2D.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0);
        myTexture2D.Apply();
        byte[] _bytes = _texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(_fullPath, _bytes);
    }
    */
    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    private IEnumerator TakeSnapshot()
    {
        yield return frameEnd;

        fileName = string.Format("{0}/DroneIMG_{1}.png",
    filePath,
    System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

        Texture2D image = new Texture2D(JobsiteDroneTexture.width, JobsiteDroneTexture.height);
        image.ReadPixels(new Rect(0, 0, JobsiteDroneTexture.width, JobsiteDroneTexture.height), 0, 0);
        image.Apply();
        //RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToPNG();
        //Destroy(image);
        File.WriteAllBytes(fileName, bytes);

    }
}