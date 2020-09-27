using UnityEngine;
using UnityEngine.Rendering;

//attach this script to your camera object
public class textureCamera : MonoBehaviour
{
    public RenderTexture cubemapEye;
    public RenderTexture equirect;


    public void test()
    {
        Camera cam = GetComponent<Camera>();
        cam.RenderToCubemap(cubemapEye, 63, Camera.MonoOrStereoscopicEye.Mono);
        cubemapEye.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Mono);
    }
}