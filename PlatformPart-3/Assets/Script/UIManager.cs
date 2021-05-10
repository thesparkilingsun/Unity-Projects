using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using PlatfromManagerVer2;

public class UIManager : MonoBehaviour
{

    #region UI Custom Events
    public delegate void BuildPlatformClicked(PlatformConfigurationData pcd);
    public static event BuildPlatformClicked BuildPlatformOnClicked;

    public delegate void UpdateCameraPosition(PlatformConfigurationData pcd);
    public static event UpdateCameraPosition OnUpdateComeraPosition;

    public delegate void NodeProgramChanged(Slider s);
    public static event NodeProgramChanged OnNodeProgramChanged;

    public delegate void WriteProgramData();
    public static event WriteProgramData OnWriteProgramData;

    public delegate void ReadProgramData();
    public static event ReadProgramData OnReadProgramData;

    // Add event to read program
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
    //public Image imgSelectedNodeColor;
    #endregion
    #endregion

    private void OnEnable()
    {
        PlatformManagerVer3.OnPlatformManagerChanged += PlatformManager_OnPlatformManagerChanged;
        PlatformManagerVer3.OnPlatformManagerUpdateUI += PlatformManagerVer2_OnPlatformManagerUpdateUI;

        PlatformDataNodeVer2.OnUpdatePlatformDataNodeUI += PlatformDataNodeVer2_OnUpdatePlatformDataNodeUI;
    }

    private void OnDisable()
    {
        PlatformManagerVer3.OnPlatformManagerChanged -= PlatformManager_OnPlatformManagerChanged;
        PlatformManagerVer3.OnPlatformManagerUpdateUI -= PlatformManagerVer2_OnPlatformManagerUpdateUI;

        PlatformDataNodeVer2.OnUpdatePlatformDataNodeUI -= PlatformDataNodeVer2_OnUpdatePlatformDataNodeUI;
    }


    private void PlatformDataNodeVer2_OnUpdatePlatformDataNodeUI(PlatformDataNodeVer2 dataNode)
    {
        dataNode.sliderSelectedProgramNodeHeight = sliderYRange;

        // update the min/max values for slider
        dataNode.sliderSelectedProgramNodeHeight.minValue = 0;
        dataNode.sliderSelectedProgramNodeHeight.maxValue = PlatformManagerVer3.Instance.configurationData.RandomHeight;

        dataNode.txtSelectedNodeName = txtSelectedNodeName;
        dataNode.txtSelectedNodePosition = txtSelectedNodePosition;
    }

    private void PlatformManagerVer2_OnPlatformManagerUpdateUI()
    {
        if(!PlatformManagerVer3.Instance.Program)
            sliderYRange.value = PlatformManagerVer3.Instance.configurationData.RandomHeight; //0.0f;
        else
            sliderYRange.value = 0.0f;

        // check to see if they are null or not ...
        if (txtSelectedNodeName!=null)
            txtSelectedNodeName.text = "";
        if(txtSelectedNodePosition!=null)
            txtSelectedNodePosition.text = string.Format("Height:");
    }


    private void PlatformManager_OnPlatformManagerChanged(PlatformConfigurationData data)
    {
        if(data!=null)
        {
            if(inputPlatformMDimension!=null)
                inputPlatformMDimension.text = data.M.ToString();
            if (inputPlatformNDimension != null)
                inputPlatformNDimension.text = data.N.ToString();
            if (sliderDeltaSpacing != null)
                sliderDeltaSpacing.value = data.deltaSpace;
            if (sliderYRange != null)
                sliderYRange.value = data.RandomHeight;

            if (txtPlatformDeltaSpacing!=null)
                txtPlatformDeltaSpacing.text = string.Format("{0:0.00}f", data.deltaSpace);
            txtPlatformDimensions.text = string.Format("{0}x{1}", data.M, data.N);
            txtPlatformYAxisRange.text = string.Format("{0}", data.RandomHeight);

            if (OnUpdateComeraPosition != null)
                OnUpdateComeraPosition(data);
        }
    }

    //void RefreshUiElements(PlatformConfigurationData data)
    //{
    //    inputPlatformMDimension.text = data.M.ToString();
    //    inputPlatformNDimension.text = data.N.ToString();
    //    sliderDeltaSpacing.value = data.deltaSpace;
    //    sliderYRange.value = data.RandomHeight;

    //    txtPlatformDeltaSpacing.text = string.Format("{0}f", data.deltaSpace);
    //    txtPlatformDimensions.text = string.Format("{0}x{1}", data.M, data.N);
    //    txtPlatformYAxisRange.text = string.Format("{0}", data.RandomHeight);
    //}


    #region FUNCTION TO HANDLE MAIN MENU FEATURES
    public void ButtonClicked(Button b)
    {
        switch(b.name)
        {
            case "btnMainMenu":
                {
                    SceneManager.LoadScene("MainMenu");
                    break;
                }
            case "btnConfiguration":
                {
                    SceneManager.LoadScene("Configuration");
                    break;
                }
            case "btnProgramming":
                {
                    SceneManager.LoadScene("Programming");
                    break;
                }
            case "btnSimulate":
                {
                    if(OnReadProgramData != null) {

                        // Update Valuse of PlatformConfiguration Data
                        // Take values from file Being Read


                        SceneManager.LoadScene("Simulation");

                    }
                    break;
                }
            case "btn-Build-Platform":
                {
                    if (BuildPlatformOnClicked != null)
                    {
                        PlatformConfigurationData pcd = new PlatformConfigurationData();
                        pcd.M = Convert.ToInt32(inputPlatformMDimension.text);
                        pcd.N = Convert.ToInt32(inputPlatformNDimension.text);
                        pcd.deltaSpace = sliderDeltaSpacing.value;
                        pcd.RandomHeight = sliderYRange.value;

                        BuildPlatformOnClicked(pcd);
                    }

                    break;
                }
            case "btn-Program-Platform":
                {
                    Debug.Log("Node Programming ...");

                    if (OnWriteProgramData != null)
                        OnWriteProgramData();

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

    public void SliderValueChanged(Slider s)
    {
        switch(s.name)
        {
            case "Slider-Delta-Spacing":
                {
                    txtPlatformDeltaSpacing.text = string.Format("{0:0.00}f", s.value);
                    break;
                }
            case "Slider-Y-Axis-Range":
                {
                    txtPlatformYAxisRange.text = string.Format("{0:0.00}f", s.value);

                    break;
                }
            case "Slider-Height":
                {
                    if (OnNodeProgramChanged != null)
                        OnNodeProgramChanged(s);

                    break;
                }
        }
    }
    #endregion
}
