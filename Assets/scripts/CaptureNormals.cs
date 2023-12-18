using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class CaptureNormals : MonoBehaviour
{
    public Camera renderCamera; // Reference to the camera

    public Material postprocessMaterial;

    private RenderTexture renderTexture;

    private string saveNormalsDir = "";

    private int count = 0;


    void Start()
    {
        renderCamera.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.DepthNormals;

        saveNormalsDir = Application.dataPath + Path.DirectorySeparatorChar + "normals";
        if (!Directory.Exists(saveNormalsDir))
        {
            Directory.CreateDirectory(saveNormalsDir);
        }

        renderTexture = new RenderTexture(1920, 1080, 0);
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postprocessMaterial);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CaptureAndSaveScreenshot();
        }
    }


    void CaptureAndSaveScreenshot()
    {
        renderCamera.targetTexture = renderTexture;

        // Create a new Texture2D with the same dimensions as the render texture
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        renderCamera.Render();

        RenderTexture oldRenderTexture = RenderTexture.active;


        // Read the pixels from the render texture into the texture
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // Save the texture as a PNG file
        byte[] bytes = texture.EncodeToPNG();
        string filePath = saveNormalsDir + Path.DirectorySeparatorChar + "normals_map_" + count.ToString() + ".png";
        count++;
        System.IO.File.WriteAllBytes(filePath, bytes);


        renderCamera.targetTexture = null;
        RenderTexture.active = oldRenderTexture;

        Destroy(texture);
    }

    public void CaptureAndSaveScreenshot(string nomeFile)
    {
        renderCamera.targetTexture = renderTexture;

        // Create a new Texture2D with the same dimensions as the render texture
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        renderCamera.Render();

        RenderTexture oldRenderTexture = RenderTexture.active;


        // Read the pixels from the render texture into the texture
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // Save the texture as a PNG file
        byte[] bytes = texture.EncodeToPNG();
        count++;
        System.IO.File.WriteAllBytes(nomeFile, bytes);


        renderCamera.targetTexture = null;
        RenderTexture.active = oldRenderTexture;

        Destroy(texture);
    }

}
