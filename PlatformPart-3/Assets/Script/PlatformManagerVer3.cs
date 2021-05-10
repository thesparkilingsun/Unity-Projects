﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace PlatfromManagerVer3
{
    public class PlatformManagerVer3 : PlatformGenericSinglton<PlatformManagerVer3>
    {

        #region Platform Manager Custom Events
        public delegate void PlatformManagerChanged(PlatformConfigurationData data);
        public static event PlatformManagerChanged OnPlatformManagerChanged;

        public delegate void PlatformManagerUpdateUI();
        public static event PlatformManagerUpdateUI OnPlatformManagerUpdateUI;
        #endregion

        public enum ColorShade
        {
            GrayScale,
            RedScale,
            GreenScale,
            BlueScale,
            Random
        }

        public GameObject PlatformBasePref;
        public int oldM;
        public int oldN;

        public PlatformConfigurationData configurationData = new PlatformConfigurationData();
        float spaceX = 0.0f;
        float spaceZ = 0.0f;

        public GameObject[,] platformNode;

        public bool SimulateTest = false;
        public bool Program = false;

        //public ColorShade shade = ColorShade.GrayScale;

        #region Selected Node Information Display Variables
        [Header("Selected Node UI Controls")]
        GameObject currentSelection = null;
        //public Text txtSelectedNodeName;
        //public Text txtSelectedNodePosition;
        //public Image imgSelectedNodeColor;
        #endregion

        private void OnEnable()
        {
            UIManager.BuildPlatformOnClicked += UIManager_BuildPlatformOnClicked;
            UIManager.OnWriteProgramData += UIManager_OnWriteProgramData;
         

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void OnDisable()
        {
            UIManager.BuildPlatformOnClicked -= UIManager_BuildPlatformOnClicked;
            UIManager.OnWriteProgramData -= UIManager_OnWriteProgramData;
           

            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        private void UIManager_BuildPlatformOnClicked(PlatformConfigurationData pcd)
        {
            configurationData = pcd;

            BuildPlatform();

            oldM = pcd.M;
            oldN = pcd.N;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (platformNode != null)
            {
                if (SceneManager.GetActiveScene().name.Contains("Program"))
                    Program = true;
                else
                    Program = false;

                BuildPlatform();

                if (OnPlatformManagerUpdateUI != null)
                    OnPlatformManagerUpdateUI();
            }

            Debug.Log(SceneManager.GetActiveScene().name);
        }

        private void UIManager_OnUpdatePlatformNode(PlatformDataNodeVer2 data)
        {
            PlatformDataNodeVer2 pdn = platformNode[data.i, data.j].GetComponent<PlatformDataNodeVer2>();
            pdn = data;
        }


        private void UIManager_OnWriteProgramData()
        {
            // we will save the platform configuration data 
            // we will save the platform node program data
            Debug.Log("SAVING PLATFORM PROGRAM DATA ... SIMULATION");
            //Debug.Log(configurationData.ToString());

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Application.dataPath, "WriteLines.txt")))
            {
                outputFile.WriteLine(configurationData.ToString());
                for (int i = 0; i < oldM; i++)
                {
                    for (int j = 0; j < oldN; j++)
                    {
                        //Debug.Log(platformNode[i, j].GetComponent<PlatformDataNodeVer2>().ToString());
                        outputFile.WriteLine(platformNode[i, j].GetComponent<PlatformDataNodeVer2>().ToString());
                    }
                }
            }
        }

        // Use this for initialization
        void Start()
        {
        }

        #region BUILD PLATFORM FROM UI

        public void DestroyPlatform()
        {
            // check to see if there is no platform currently configured
            // if there is one, delete it
            if (platformNode != null)
            {
                for (int i = 0; i < oldM; i++)
                {
                    for (int j = 0; j < oldN; j++)
                    {
                        Destroy(platformNode[i, j], 0.1f);
                    }
                }

                platformNode = null;
            }
        }

        public void BuildPlatform()
        {
            DestroyPlatform();

            platformNode = new GameObject[configurationData.M, configurationData.N];

            spaceX = 0;
            spaceZ = 0;

            for (int i = 0; i < configurationData.M; i++)
            {
                spaceZ = 0.0f;
                for (int j = 0; j < configurationData.N; j++)
                {
                    float x = (i * 1) + spaceX;
                    float z = (j * 1) + spaceZ;

                    //Debug.Log(string.Format("x={0} z={1}", x, z));
                    // create a platform pref ...
                    var platformBase = Instantiate(PlatformBasePref,
                                                   new Vector3(x, 0, z),
                                                   Quaternion.identity);

                    platformBase.name = string.Format("Node[{0},{1}]", i, j);

                    platformBase.AddComponent<PlatformDataNodeVer2>();

                    platformNode[i, j] = platformBase;

                    PlatformDataNodeVer2 pdn = platformBase.transform.GetComponent<PlatformDataNodeVer2>();
                    pdn.Program = Program;
                    pdn.i = i;
                    pdn.j = j;

                    spaceZ += configurationData.deltaSpace;
                }
                spaceX += configurationData.deltaSpace;
            }

            if (OnPlatformManagerChanged != null)
                OnPlatformManagerChanged(configurationData);
        }

        public void StartSimulationButtonClick()
        {
            SimulateTest = !SimulateTest;
        }
        #endregion

        public static bool NearlyEquals(float? value1, float? value2, float unimportantDifference = 0.01f)
        {
            if (value1 != value2)
            {
                if (value1 == null || value2 == null)
                    return false;

                return Math.Abs(value1.Value - value2.Value) < unimportantDifference;
            }

            return true;
        }

        // Update is called once per frame
        void Update()
        {

            // check to see if platform has been build
            if (platformNode == null)
                return;

            //if (Input.GetKeyUp(KeyCode.T))
            //    SimulateTest = !SimulateTest;
            //if (Input.GetKeyUp(KeyCode.H))
            //    shade = ColorShade.GrayScale;
            //if (Input.GetKeyUp(KeyCode.R))
            //    shade = ColorShade.RedScale;
            //if (Input.GetKeyUp(KeyCode.G))
            //    shade = ColorShade.GreenScale;
            //if (Input.GetKeyUp(KeyCode.B))
            //    shade = ColorShade.BlueScale;
            //if (Input.GetKeyUp(KeyCode.E))
            //    shade = ColorShade.Random;
            //if (Input.GetKeyUp(KeyCode.W))
            //{
            //    RandomHeight += 1;
            //    txtPlatformYAxisRange.text = string.Format("{0}", RandomHeight);
            //}
            //if (Input.GetKeyUp(KeyCode.S))
            //{
            //    RandomHeight -= 1;
            //    if (RandomHeight < 0)
            //        RandomHeight = 0;
            //    txtPlatformYAxisRange.text = string.Format("{0}", RandomHeight);
            //}
            if (Input.GetKey(KeyCode.Q))
                Application.Quit();

            #region Object Selection
            // you can select only if in program mode/scene
            if (Program)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    //if (IsPointerOverUIObject())
                    //    return;

                    #region Screen To World
                    RaycastHit hitInfo = new RaycastHit();
                    bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                    if (hit)
                    {
                        #region COLOR

                        if (currentSelection != null)
                        {
                            PlatformDataNodeVer2 pdn = currentSelection.transform.GetComponent<PlatformDataNodeVer2>();
                            pdn.ResetDataNode();
                        }

                        currentSelection = hitInfo.transform.gameObject;
                        PlatformDataNodeVer2 newPdn = currentSelection.transform.GetComponent<PlatformDataNodeVer2>();
                        newPdn.SelectNode();

                        #endregion
                    }
                    else
                    {
                        Debug.Log("No hit");
                    }
                    #endregion
                }
            }
            #endregion
        }

        /// <summary>
        /// Used to determine if we are over UI element or not.
        /// </summary>
        /// <returns></returns>
        public bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();

           
            //foreach (var result in results)
            //{
            //    Debug.Log(result.gameObject.name);
            //}
            return results.Count > 0;
        }
    }
}
