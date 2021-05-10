using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{

    #region UI Custom Events
    public delegate void Build(PlatformConfigurationData pcd);
    public static event Build BuildPlatform;

    //public delegate void UpdateCameraPosition(PlatformConfigurationData pcd);
    //public static event UpdateCameraPosition OnUpdateComeraPosition;

    public delegate void NodeProgramChanged(Slider s);
    public static event NodeProgramChanged OnNodeProgramChanged;

    public delegate void WriteProgramData();
    public static event WriteProgramData OnWriteProgramData;

    public delegate void SceneChange(Button b);
    public static event SceneChange OnActiveSceneChange;

    GameObject platformManager;
    PlatformManager platformManagerScript;

    #endregion

    #region UI Control Ref.
    #region PLATFORM BUTTON CONTROLS
    //public Button buttonStartSimulation;

    public Text txtPlatformDimensions;
    public Text txtPlatformDeltaSpacing;
    public Text txtPlatformYAxisRange;
    #endregion

    #region PANEL RECT TRANSFORM
    //[Header("Rect Transform for Panels")]
    //bool DisplayConfigurationPanels = true;
    //public RectTransform panelPlatformConfiguration;
    //public RectTransform panelPlatformColorConfiguration;
    //public RectTransform panelPlatformNodeData;
    #endregion

    #region Platform Configuration UI Variables
    [Header("Platform Configuration Fields")]
    public InputField inputPlatformMDimension;
    public InputField inputPlatformNDimension;
    public Slider sliderDeltaSpacing;
    public Slider sliderYRange;
    public Dropdown dropDownColorSelection;
    #endregion

    #region Selected Node Information Display Variables
    [Header("Selected Node UI Controls")]
    //GameObject currentSelection = null;
    public Text txtSelectedNodeName;
    public Text txtSelectedNodePosition;
    //public Image imgOpenConfigPanel;
    public RectTransform panel;
    PlatformDataNode node;
    bool toggle = false;

    #endregion
    #endregion

    private void OnEnable()
    {


        PlatformManager.OnPlatformManagerChanged += PlatformManager_OnPlatformManagerChanged;
        //PlatformManager.OnPlatformManagerUpdateUI += PlatformManager_OnPlatformManagerUpdateUI;

        //PlatformDataNode.OnUpdatePlatformDataNodeUI += PlatformDataNodeVer2_OnUpdatePlatformDataNodeUI;
    }

    private void OnDisable()
    {


        PlatformManager.OnPlatformManagerChanged -= PlatformManager_OnPlatformManagerChanged;
        //PlatformManager.OnPlatformManagerUpdateUI -= PlatformManagerVer2_OnPlatformManagerUpdateUI;

        //PlatformConfigurationData.OnUpdatePlatformDataNodeUI -= PlatformDataNodeVer2_OnUpdatePlatformDataNodeUI;
        // Read Data 

    }


    void Start()
    {

        Scene(SceneManager.GetActiveScene());


    }


    //private void PlatformDataNodeVer2_OnUpdatePlatformDataNodeUI(PlatformDataNode dataNode)
    //{
    //    dataNode.sliderSelectedProgramNodeHeight = sliderYRange;

    //    // update the min/max values for slider
    //    dataNode.sliderSelectedProgramNodeHeight.minValue = 0;
    //    dataNode.sliderSelectedProgramNodeHeight.maxValue = PlatformManager.Instance.PlatformConfigurationData.RandomHeight;

    //    dataNode.txtSelectedNodeName = txtSelectedNodeName;
    //    dataNode.txtSelectedNodePosition = txtSelectedNodePosition;
    //}

    //private void PlatformManagerVer2_OnPlatformManagerUpdateUI()
    //{
    //    if(!PlatformManagerVer3.Instance.Program)
    //        sliderYRange.value = PlatformManagerVer3.Instance.configurationData.RandomHeight; //0.0f;
    //    else
    //        sliderYRange.value = 0.0f;

    //    // check to see if they are null or not ...
    //    if (txtSelectedNodeName!=null)
    //        txtSelectedNodeName.text = "";
    //    if(txtSelectedNodePosition!=null)
    //        txtSelectedNodePosition.text = string.Format("Height:");
    //}

    private void PlatformManager_OnPlatformManagerChanged(PlatformConfigurationData data)
    {
        if (data != null)
        {
            if (inputPlatformMDimension != null)
                inputPlatformMDimension.text = data.M.ToString();
            if (inputPlatformNDimension != null)
                inputPlatformNDimension.text = data.N.ToString();
            if (sliderDeltaSpacing != null)
                sliderDeltaSpacing.value = data.deltaSpace;
            if (sliderYRange != null)
                sliderYRange.value = data.RandomHeight;

            if (txtPlatformDeltaSpacing != null)
                txtPlatformDeltaSpacing.text = string.Format("{0:0.00}f", data.deltaSpace);
            txtPlatformDimensions.text = string.Format("{0}x{1}", data.M, data.N);
            txtPlatformYAxisRange.text = string.Format("{0}", data.RandomHeight);

            //if (OnUpdateComeraPosition != null)
            //    OnUpdateComeraPosition(data);
        }
    }



    #region FUNCTION TO HANDLE MAIN MENU FEATURES

    public void ButtonClicked(Button b)
    {

        switch (b.name)
        {

            case "btnMainMenu":
                {
                    SceneManager.LoadScene("MainMenu");

                    panel = null;
                    Debug.Log("Main Menu");
                    break;
                }
            case "btnSetup":
                {

                    SceneManager.LoadScene("Setup");
                    break;
                }
            case "btnProgram":
                {
                    SceneManager.LoadScene("Programming");
                    platformManager = GameObject.Find("--GameObject");
                    platformManagerScript = platformManager.GetComponent<PlatformManager>();
                    for (int i = 0; i < platformManagerScript.X; i += 1)
                    {
                        for (int j = 0; j < platformManagerScript.Y; j += 1)
                        {
                            DontDestroyOnLoad(platformManagerScript.cube[i, j]);
                        }
                    }
                    //PlatformConfigurationData pcd = new PlatformConfigurationData();

                    //node.NextPosition = transform.position.y;
                    //pcd.M = node.i;
                    //pcd.N = node.j;
                    //BuildPlatform(pcd);
                    break;
                }
            case "btnSimulate":
                {
                    SceneManager.LoadScene("Simulate");
                    panel = null;
                    break;
                }
            case "btnBuildPlatform":
                {

                    if (BuildPlatform != null)
                    {
                        PlatformConfigurationData pcd = new PlatformConfigurationData();
                        pcd.M = Convert.ToInt32(inputPlatformMDimension.text);
                        pcd.N = Convert.ToInt32(inputPlatformNDimension.text);
                        pcd.deltaSpace = sliderDeltaSpacing.value;
                        pcd.RandomHeight = sliderYRange.value;
                        PlatformDataNode node = new PlatformDataNode();
                        node.i = pcd.M;
                        node.j = pcd.N;
                        BuildPlatform(pcd);
                    }

                    panel.gameObject.SetActive(false);
                    break;
                }
            case "btnProgramPlatform":
                {
                    Debug.Log("Node Programming ...");

                    if (OnWriteProgramData != null)
                        OnWriteProgramData();
                    panel.gameObject.SetActive(false);
                    break;
                }
            case "btnExit":
                {
                    Debug.Log(b.name);
                    Application.Quit();
                    break;
                }


        }
    }

    public void onButtonForScene(Button b)
    {
        OnActiveSceneChange(b);
    }



    public void SliderValueChanged(Slider s)
    {
        switch (s.name)
        {
            case "sliderSpacing":
                {
                    txtPlatformDeltaSpacing.text = string.Format("{0:0.00}f", s.value);
                    break;
                }
            case "sliderHeight":
                {
                    txtPlatformYAxisRange.text = string.Format("{0:0.00}f", s.value);

                    break;
                }
            case "SliderNodeHeigh":
                {
                    if (OnNodeProgramChanged != null)
                        OnNodeProgramChanged(s);

                    break;
                }
        }
    }
    #endregion
    public void Scene(Scene scene)
    {
        switch (scene.name)
        {
            case "MainMenu":
                panel = null;
                break;
            case "Setup":
                panel.gameObject.SetActive(false); ;
                break;
            case "Programming":
                panel.gameObject.SetActive(false);
                break;
            case "Simulate":

                panel = null;
                break;

        }
    }

    public void TogglePanel()
    {

        toggle = !toggle;
        if (Input.GetMouseButtonUp(0))
        {
            panel.gameObject.SetActive(toggle);
        }
    }



}
