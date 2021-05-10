using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformDataNode : MonoBehaviour {

    int i;
    int j;

    Image imgSelectedProgramNodeColor;
    Image imgSelectedNodeColor;
    public Color nextColor;
    public float nextPosition = 0.0f;
    bool program = false;
    bool selected = false;
    bool simulate = false;

   public  Slider sliderSelectedProgramNodeHeight;
    public Text txtSelectedNodeName;
    public Text txtSelectedNodePosition;
    public delegate void UpdatePlatformDataNodeUI(PlatformDataNode pdn);
    public static event UpdatePlatformDataNodeUI OnUpdatePlatformDataNodeUI;

   

    Canvas myCanvas;
     void OnEnable()
    {
        UIManager.OnNodeProgramChanged += UIManager_OnNodeProgramChanged;
    }
     void OnDisable()
    {
        UIManager.OnNodeProgramChanged -= UIManager_OnNodeProgramChanged;
    }


    // Use this for initialization
    void Start () {
        //program = false;
        //selected = false;
        //simulate = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SelectNode()
    {
        nextColor = Color.blue;
        nextPosition = UnityEngine.Random.Range(0, 7);

        selected = true;
        simulate = true;

        myCanvas.enabled = true;

        if (OnUpdatePlatformDataNodeUI != null)
            OnUpdatePlatformDataNodeUI(this);

        sliderSelectedProgramNodeHeight.value = transform.position.y;
    }

    public  void ResetDataNode()
    {
        if (!program)
        {
            simulate = true;
            selected = false;

            nextColor = Color.white;
            nextPosition = 0.0f;
     // selected node
        }
        else
        {
            simulate = true;
            selected = false;
        }
    } 


    void UpdateUI()
    {

        txtSelectedNodeName.text = transform.name;
        txtSelectedNodePosition.text = string.Format("Height: {0:0:00}f",transform.position.y);

    }

    void UIManager_OnNodeProgramChanged()
    {


    }



}
