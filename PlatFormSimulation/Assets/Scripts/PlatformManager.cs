using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlatformManager : PlatformGenericSingleTon<PlatformManager>
{


    public GameObject PlatformBasePref;
    GameObject currentSelection;
   public  PlatformConfigurationData configurationData;
    int oldM  ;
    int oldN ;
    GameObject[,] platformNode;
    bool program;
    bool simulateText;
    float spaceX = 0.2f, spaceZ = 0.2f;


    public delegate void PlatformManagerUpdateUI(PlatformConfigurationData configurationData );
    public delegate void PlatformManagerChanged(PlatformConfigurationData pcd);

    public static event PlatformManagerUpdateUI OnPlatformManagerUpdateUI;
    public static event PlatformManagerChanged OnPlatformManagerChanged;


    
    

    // Use this for initialization
    void Start() {
        
        platformNode = new GameObject[oldM, oldN];
      

    }
// Iska kuch karte hai
    //private bool IsPointerOverUIElement()
    //{

    //}
    // Update is called once per frame
    void Update() {


        if (!program)
        {

            if (Input.GetMouseButtonUp(0))
              {
            //    if (IsPointerOverUIElement())
            //        return;
                #region Screen To WorldRaycastHit 
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {

                    if (currentSelection != null)
                    {
                        PlatformDataNode pdn = currentSelection.transform.GetComponent<PlatformDataNode>();
                        pdn.ResetDataNode();

                    }

                    currentSelection = hitInfo.transform.gameObject;
                    PlatformDataNode newPdn = currentSelection.transform.GetComponent<PlatformDataNode>();
                    newPdn.SelectNode();


                }
                else
                {
                    Debug.Log("" + hit);
                }



            }


        }

    }



    private void OnEnable()
    {
        UIManager.BuildPlatformOnClicked += UIManager_BuildPlatformOnClicked;
        UIManager.OnWriteProgramData += UIManager_OnWriteProgramData;

        // UIManager.OnUpdatePlatformNode += UIManager_OnUpdatePlatformNode;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        UIManager.BuildPlatformOnClicked -= UIManager_BuildPlatformOnClicked;
        UIManager.OnWriteProgramData -= UIManager_OnWriteProgramData;

        // UIManager.OnUpdatePlatformNode += UIManager_OnUpdatePlatformNode;

        
    }


    public void BuiltPlatform()
    {

        if (platformNode != null)
        {

            DestroyPlatform();
        }

       
        // for trying programming mode added hardcoded values
        
        platformNode = new GameObject[16, 8];
        spaceX = 0.2f;

        for (int i = 0; i < configurationData.M; i++)
        {

            spaceZ = 0.0f;
            for (int j = 0; j < configurationData.N; j++)
            {


                float x = i * 1 + 0.2f;
                float z = j * 1 + 0.2f;

            Instantiate(PlatformBasePref, this.transform.position = new Vector3(x, 0, z), transform.rotation = Quaternion.identity);

               
               
                PlatformBasePref.transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
                //  currentColor[i, j] = new Color(1.0f, 1.0f, 1.0f); // default color of platform when loaded
                PlatformBasePref.name = string.Format("Cube-{0}-{1}", i, j);
               // cube.transform.gameObject.GetComponent<Renderer>().material.color = currentColor[i, j];
                platformNode[i, j] = PlatformBasePref.gameObject;
               // currentPosition[i, j] = 0.0f;
               // PlatformBasePref.GetComponent<Renderer>().material = myMaterial;
              //  nextPosition[i, j] = 0.0f; // only moving in Y position
                spaceZ += 0.2f;

            }
            spaceX += 0.2f;

        }



    }



    void DestroyPlatform()
    {
        for (int i = 0; i < oldM; i++)
        {
            for (int j = 0; j < oldN; j++)
            {
                Destroy(platformNode[i, j]);
            }
        }

        platformNode = null;

        //currentColor = null;
        //nextColor = null;
        //currentPosition = null;
        //nextPosition = null;

    }

    public static bool NearlyEquals(float? currentValue, float? nextValue, float NegligibleDifference = 0.001f)
    {
        if (currentValue != nextValue)
        {
            if (currentValue == null || nextValue == null)
                return false;

            return Mathf.Abs(currentValue.Value - nextValue.Value) < NegligibleDifference;
        }
        return true;
    }

    void Inputs()
    {
        if (Input.GetKeyUp(KeyCode.T)) //start or stop simulation
        {
            // Simulate = !Simulate;

        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            // Range++;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            //decrease range
            //if (Range > 1.0f)
            //{
            //    Range--;

            //}
            //if (Range == 0)
            //    Range = 1.0f;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            //quit application
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // Random value of color in red spectrum

            //colorSelected = MySelectedColor.Red;



        }

        else if (Input.GetKeyDown(KeyCode.G))
        {

            // Random value of color in green spectrum
            //colorSelected = MySelectedColor.Green;



        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            // Random value of color in blue Spectrum

            //colorSelected = MySelectedColor.Blue;

        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            // Random value of color in greyScale
            //colorSelected = MySelectedColor.GreyScale;

        }

        if (Input.GetKeyDown(KeyCode.E))
        {

            // if E is pressed Generate random color in RGB spectrum

            //colorSelected = MySelectedColor.Random;
        }

       
    }

    void UIManager_OnWriteProgramData()
    {
        // Here I need to paste code given My sir
    }

    void UIManager_BuildPlatformOnClicked(PlatformConfigurationData pcd)
    {
        configurationData = pcd;

        BuiltPlatform();

        oldM = pcd.M;
        oldN = pcd.N;

    }

    private void SceneManager_sceneLoaded(Scene scene,LoadSceneMode loadScene)
    {
        if (SceneManager.GetActiveScene().name.Contains("Simulate"))
            simulateText = true;

        else
            simulateText = false;

        if(simulateText)
        { // Do Something here
            Debug.Log("Simulate out Platform");
        }
        else
        {
            if(platformNode != null)
            {
                if (SceneManager.GetActiveScene().name.Contains("ApplicationProgramming"))
                    program = true;
                else
                    program = false;

                BuiltPlatform();

                if (OnPlatformManagerUpdateUI != null)
                    OnPlatformManagerUpdateUI(configurationData);
                         
            }
        }
        


    }
    
}
#endregion