using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour {

    Dropdown dropDownColorSelection;

  public  InputField inputPlatformMDimension;
   public InputField inputPlatformNDimension;

    public Slider sliderDeltaSpacing;
   public Slider sliderYRange;

   public Text txtPlatformDeltaSpacing;
   public Text txtPlatformDimensions;
   public Text txtPlatformYAxisRange;
    Text txtSelectedNodeName;
    Text txtSelctedNodePosition;

    public delegate void BuildPlatformClicked(PlatformConfigurationData pcd);
    public static event BuildPlatformClicked BuildPlatformOnClicked;

    public delegate void NodeProgramChanged();
    public static event NodeProgramChanged OnNodeProgramChanged;

    public delegate void WriteProgramData();
    public static event WriteProgramData OnWriteProgramData;


    private void OnEnable()
    {
        PlatformManager.OnPlatformManagerChanged += PlatformManager_OnPlatformManagerChanged;
        PlatformManager.OnPlatformManagerUpdateUI += PlatformManager_OnPlatformManagerUpdateUI;
        PlatformDataNode.OnUpdatePlatformDataNodeUI += PlatformDataNode_OnUpdatePlatformDataNodeUI;
    }



    private void OnDisable()
    {
        PlatformManager.OnPlatformManagerChanged -= PlatformManager_OnPlatformManagerChanged;
        PlatformManager.OnPlatformManagerUpdateUI -= PlatformManager_OnPlatformManagerUpdateUI;
        PlatformDataNode.OnUpdatePlatformDataNodeUI -= PlatformDataNode_OnUpdatePlatformDataNodeUI;
    }


    void Start()
    {
        
    }

     void Update()
    {
        
    }


    public void ButtonClicked(Button btn)
    {

        switch (btn.name)
         {
             case "configBtn":
                SceneManager.LoadScene("Configuration");
                break;

             case "progBtn":
                SceneManager.LoadScene("ApplicationProgramming");
                break;

             case "simulateBtn":
                SceneManager.LoadScene("Simulation");
                break;

            case "mainMenuBtn":
                SceneManager.LoadScene("MainMenu");
                break;

            case "setupBtn":
                Debug.Log("Setup Btn is clicked");
                break;

            case "progPlatformBtn":
                Debug.Log("Building Platform");
                break;

            case "exitBtn":
                Debug.Log("Application Button Clicked");
                Application.Quit();
                break;

             }

    }
   


   


	public void SliderValueChanged(Slider s)
    {
        switch (s.name)
        {
            case "deltaSpacingSld":
                txtPlatformDeltaSpacing.text = sliderDeltaSpacing.value.ToString(); 
             break;

            case "sliderYRange":
                txtPlatformYAxisRange.text = sliderYRange.value.ToString();
            break;
 
            
        }

    }


    public void PlatformDataNode_OnUpdatePlatformDataNodeUI(PlatformDataNode pdn)
    {
        pdn.sliderSelectedProgramNodeHeight = sliderYRange;
        // Update values from UI to datanode
        pdn.sliderSelectedProgramNodeHeight.minValue = 0;

        // Insert field for Max value of slider here
        pdn.sliderSelectedProgramNodeHeight.maxValue = PlatformManager.Instance.configurationData.RandomHeight;
        //
        pdn.txtSelectedNodeName = txtSelectedNodeName;
        pdn.txtSelectedNodePosition = txtSelctedNodePosition;




    }
    
    void PlatformManager_OnPlatformManagerChanged(PlatformConfigurationData data)
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
            txtPlatformDeltaSpacing.text = string.Format("{0:0.00}f",data.deltaSpace);
        txtPlatformDimensions.text = string.Format("{0}*{1}",data.M,data.N);
        txtPlatformYAxisRange.text = string.Format("{0}", data.RandomHeight);

        //Here I will insert change in Camera Pos

    }

    void PlatformManager_OnPlatformManagerUpdateUI(PlatformConfigurationData pm) {

        // update all the data onto Ui 
        // the value of M and N 
        // Value of range and Height

        inputPlatformMDimension.text = pm.M.ToString();
        inputPlatformNDimension.text = pm.N.ToString();

        txtPlatformDeltaSpacing.text = pm.deltaSpace.ToString();
        txtPlatformYAxisRange.text = pm.RandomHeight.ToString();


       }




  }
