using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // seed for random generator (used to generate random light's intensity and color).
    private const int SEED = 11;
    public static string img_path = @"C:\Users\mar-z\progetti\data\SynthOutdoor\images\";
    //public static string img_path = @"C:\Users\mar-z\Desktop\saved_screenshots\images\";
    public static string mask_path = @"C:\Users\mar-z\progetti\data\SynthOutdoor\masks\";
    //public static string mask_path = @"C:\Users\mar-z\Desktop\saved_screenshots\masks\";

    public static string normals_path = @"C:\Users\mar-z\progetti\data\SynthOutdoor\normals\";

    public static string nomeScreenPrefisso = "scene";
    //public static string fileName = @"C:\Users\mar-z\Desktop\saved_screenshots\light.csv";
    public static string fileName = @"C:\\Users\\mar-z\\progetti\\data\\SynthOutdoor\\light.csv";
    public static string sep = ";";
    private int savingCounter = 0;
    private const int NUM_SCREEN_SHOT = 50000;
    //private const int NUM_SCREEN_SHOT = 5;
    private const int CHANGE_LIGHT_FREQ = 5;
    private Camera mainCamera;
    private Terrain terrain;
    private Light sun;

    private Screenshotter screenshotter;
    private CaptureNormals captureNormals;

    private static string FORMAT = "0.000";

    private List<GameObject> firstLevelObjects;
    private GameObject[] allSceneObjects;

    private Dictionary<GameObject, Material> objectMonoMaterials;

    private Dictionary<GameObject, Material[]> oldMaterials;
    private Material oldSkyMaterial;
    private Color oldAmbientLight;
    private Color oldCameraBackground;

    // bool isMonoColor = false;

    // bool manual = false;

    private static int n_frames_from_last_screenshot = 0;
    private const int FRAMES_BETWEEN_SCREENSHOTS = 60;

    public const float deltaTimeMio = 0.004f;

    void initMaterialsDictionaries()
    {
        oldMaterials = new Dictionary<GameObject, Material[]>();

        firstLevelObjects = new List<GameObject>();

        allSceneObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach(GameObject go in allSceneObjects)
        {
            if(go.transform.root.gameObject == go)
            {
                firstLevelObjects.Add(go);
            }

            // save old materials into dictionary
            Renderer rnd = go.GetComponent<Renderer>();
            if(rnd != null)
            {
                oldMaterials.Add(go, rnd.materials);
            }
        }

        // create dictionary with "monocromatic" material (one for each parent object).
        objectMonoMaterials = new Dictionary<GameObject, Material>();
        foreach (GameObject o in firstLevelObjects)
        {
            Material m = new Material(Shader.Find("Standard"));
            m.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            objectMonoMaterials.Add(o, m);
        }

        oldSkyMaterial = RenderSettings.skybox;
        oldAmbientLight = RenderSettings.ambientLight;
        oldCameraBackground = mainCamera.backgroundColor;
    }


    void setRecursiveMaterial(GameObject o, Material material)
    {
        Renderer rnd = o.GetComponent<Renderer>();
        if(rnd != null)
        {
            int N_old_materials = rnd.materials.Length;

            Material[] newMaterials = new Material[N_old_materials];
            for(int i = 0; i < N_old_materials; i++)
            {
                newMaterials[i] = material;
            }

            rnd.materials = newMaterials;
            rnd.sharedMaterials = newMaterials;
        }

        foreach(Transform child in o.transform)
        {
            setRecursiveMaterial(child.gameObject, material);
        }
    }


    void setMonoColorAllOBjects()
    {

        foreach (GameObject o in firstLevelObjects)
        {
            setRecursiveMaterial(o, objectMonoMaterials[o]);
        }

        //oldAmbientLight = RenderSettings.ambientLight;
        RenderSettings.skybox = null;
        RenderSettings.ambientLight = Color.white;

        //GameObject.Find("Terrain").SetActive(false);
        
        //terrain.materialTemplate.mainTexture = null;
        //terrain.materialTemplate.color = Color.green;

        mainCamera.backgroundColor = Color.white;
        sun.gameObject.SetActive(false);
        terrain.gameObject.SetActive(false);

    }


    void setOldMaterials()
    {
        foreach(GameObject o in oldMaterials.Keys)
        {
            o.GetComponent<Renderer>().materials = oldMaterials[o];
        }

        RenderSettings.skybox = oldSkyMaterial;
        RenderSettings.ambientLight = oldAmbientLight;
        mainCamera.backgroundColor = oldCameraBackground;

        sun.gameObject.SetActive(true);
        terrain.gameObject.SetActive(true);

    }


    // Start is called before the first frame update
    void Start()
    {
        // initialize random generator with seed (for reproducibility).
        Random.InitState(SEED);

        mainCamera = gameObject.GetComponent<Camera>();
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();

        sun = GameObject.Find("sun").GetComponent<Light>();
        screenshotter = gameObject.GetComponent<Screenshotter>();
        var tmp = GameObject.FindObjectsOfType<CaptureNormals>();
        if(tmp.Length == 0)
        {
            throw new System.Exception("CaptureNormals script not found!");
        }
        captureNormals = tmp[0];

        initMaterialsDictionaries();
        System.IO.File.WriteAllText(fileName, "\"sep=;\"\nIDscreenshot;L_x;L_y;L_z;L_R;L_G;L_B;A_R;A_G;A_B\n");

        //InvokeRepeating("creaCoppiaScreenshot_GeometriaInfo", 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //        if (Input.GetKeyDown(KeyCode.Space))
        //        {
        //            if (isMonoColor)
        //            {
        //                setOldMaterials();
        //                isMonoColor = false;
        //            }
        //            else
        //            {
        //                setMonoColorAllOBjects();
        //                isMonoColor = true;
        //            }
        //        }
        //        else if(Input.GetKeyDown(KeyCode.Escape))
        //        {
        //#if UNITY_EDITOR
        //            // Application.Quit() does not work in the editor so
        //            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        //            UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //                     Application.Quit();
        //#endif
        //        }
        //        else if(Input.GetKeyDown(KeyCode.Return))
        //        {
        //            FlyCamera flyCameraScript = GameObject.FindObjectOfType<FlyCamera>();
        //            WayPoints wayPointsScript = GameObject.FindObjectOfType<WayPoints>();
        //            if (!manual)
        //            {
        //                manual = true;
        //                flyCameraScript.enabled = true;
        //                CancelInvoke("creaCoppiaScreenshot_GeometriaInfo");
        //                wayPointsScript.enabled = false;
        //            }
        //            else
        //            {
        //                manual = false;
        //                flyCameraScript.enabled = false;
        //                InvokeRepeating("creaCoppiaScreenshot_GeometriaInfo", 0.5f, 0.5f);
        //                wayPointsScript.enabled = true;
        //            }

        //        }
        //        else if (Input.GetKeyDown(KeyCode.X))
        //        {
        //            creaCoppiaScreenshot_GeometriaInfo();
        //        }



        //n_frames_from_last_screenshot++;

        //if(n_frames_from_last_screenshot == FRAMES_BETWEEN_SCREENSHOTS)
        //{
        //    creaCoppiaScreenshot_GeometriaInfo();
        //    Debug.Log("creata coppia screenshot");
        //    n_frames_from_last_screenshot = 0;
        //}
    }

    public void createScreenshotAndGeometryGT()
    {
        if (savingCounter < NUM_SCREEN_SHOT)
        {
            try
            {
                string nome_file = img_path + nomeScreenPrefisso + savingCounter + ".png";
                acquireScreenshot(nome_file, false);
                string nome_maschera = mask_path + "mask_" + nomeScreenPrefisso + savingCounter + ".png";
                setMonoColorAllOBjects();
                acquireScreenshot(nome_maschera, false);
                setOldMaterials();

                // save normals
                string nomeFileNormali = normals_path + "normals_" + nomeScreenPrefisso + savingCounter + ".png";
                captureNormals.CaptureAndSaveScreenshot(nomeFileNormali);

                saveTransformations();
                savingCounter++;

                // ogni CHANGE_FREQ_LIGHT salvataggi cambio l'illuminazione ambientale
                if (savingCounter % CHANGE_LIGHT_FREQ == 0)
                {
                    float intensity = Random.Range(0.2f, 0.3f);
                    RenderSettings.ambientLight = new Color(intensity, intensity, intensity);

                    System.IO.File.AppendAllText(fileName, RenderSettings.ambientLight.ToString());

                    oldAmbientLight = RenderSettings.ambientLight;
                }

                // ogni CHANGE_FREQ_LIGHT salvataggi cambio il colore della luce direzionale
                if ((savingCounter + CHANGE_LIGHT_FREQ / 2) % CHANGE_LIGHT_FREQ == 0)
                {
                    float R = Random.Range(0.8f, 1f);
                    float G = Random.Range(0.8f, 0.85f);
                    float B = Random.Range(0.7f, 0.8f);
                    sun.color = new Color(R, G, B);

                    //System.IO.File.AppendAllText(fileName, sun.color.ToString());
                }
            }
            catch(System.IO.IOException e)
            {
                Debug.LogError(e.ToString());

#if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
#else
                                     Application.Quit();
#endif
            }
            catch (System.Exception e)
            {
                Debug.Log(e.ToString());
#if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
#else
                                     Application.Quit();
#endif
            }

        }
        else
        {

            // save any game data here
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
                     Application.Quit();
#endif

        }
    }

    // take screenshot, delegating to specific class
    void acquireScreenshot(string nome_file, bool applyAA = true)
    {
        //ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height, path + nomeScreenPrefisso + contatoreSalvataggi + ".png");
        screenshotter.TakeScreenshot(nome_file, applyAA);
    }


    // save in a file position and orientation of camera and directional light (sun)
    void saveTransformations()
    {
        Vector3 cameraDirection = mainCamera.transform.rotation * Vector3.forward;
        Vector3 lightDirection = sun.transform.rotation * Vector3.forward;
        Vector3 lightDirection_cameraRef = mainCamera.transform.InverseTransformDirection(lightDirection);

        Vector2 direzioneLuce_2D_project = new Vector2(lightDirection_cameraRef.x, lightDirection_cameraRef.y);
        direzioneLuce_2D_project.Normalize();


        string riga = nomeScreenPrefisso + savingCounter + ".png" + sep +
                lightDirection_cameraRef.x.ToString(FORMAT) + sep + lightDirection_cameraRef.y.ToString(FORMAT) + sep + lightDirection_cameraRef.z.ToString(FORMAT) + sep +
                sun.color.r + sep + sun.color.g + sep + sun.color.b + sep +
                RenderSettings.ambientLight.r + sep + RenderSettings.ambientLight.g + sep + RenderSettings.ambientLight.b + "\n";
        

        System.IO.File.AppendAllText(fileName, riga);

    }
}
