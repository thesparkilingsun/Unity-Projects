using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO; //System.IO.FileInfo, System.IO.StreamReader, System.IO.StreamWriter
using System; //Exception
using System.Text; //Encoding


/*
----------------------------
Please mainly look at followinf codes for simulation.
- Inside of Update() {else if (SceneManager.GetActiveScene().name == "SimulationScene")}
- moveTarget()
- SimulationNew()
- MakeMove()
----------------------------
*/

public class PlatformManager : PlatformGenericSingleTon<PlatformManager>
{

    GameObject configurationManager;
    PlatformConfigurationData configManagerScript;
    PlatformDataNode node;
    public GameObject clickedGameObject;
    public GameObject[,] cube;
    public Color color;
    Text sizeText;
    Text rangeText;
    Text index;
   

    string line = "";
    string str;
    string[] strArr;
    string[] dataArray;
    string[] colorData;
    string[,] nodeData;
    public int X = 0;
    public int Y = 0;
    public float scaleOnY = 0.1f;
    public float space = 0.1f;
    public float range = 1.0f;
    public Boolean built = false;
    public Boolean program = false;
    public Material material;

    float[,] isProgrammed;
    float timeCount = 0.2f;

    Color[,] colorInfo;
    float[,] yInfo;
    int lineNo = 0;

    GameObject pCam;
    Camera platformCamera;

    // Use this for initialization

    // Delegate and Event for Platform manger Update
    public delegate void PlatformManagerChanged(PlatformConfigurationData data);
    public static event PlatformManagerChanged OnPlatformManagerChanged;


    void Start(){
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    public void OnEnable()
    {
        //UIManager.OnActiveSceneChange += OnActiveSceneChanged;
        UIManager.BuildPlatform += BuildPlatform;
    }

    public void OnDisable()
    {
        //UIManager.OnActiveSceneChange -= OnActiveSceneChanged;
    }

    // This is being called when Scene Changed 
    // This is for the UI Changing Scenes
    void OnActiveSceneChanged(Scene precScene, Scene nextScene){
       
        
        if (nextScene.name == "Programming"){
            pCam = GameObject.Find("PlatformCamera");
            platformCamera = pCam.GetComponent<Camera>();
            //configurationManager = GameObject.Find("--Gameobject");
            //configManagerScript = configurationManager.GetComponent<PlatformConfigurationData>();
            //program = true;
            //BuildPlatform(configManagerScript);
            //sizeText = GameObject.Find("Size").GetComponent<Text>();
            //sizeText.text = "Size: " + configManagerScript.M.ToString() + " * " + configManagerScript.N.ToString();
            //rangeText = GameObject.Find("Range").GetComponent<Text>();
            //rangeText.text = "MaxY:" + configManagerScript.RandomHeight.ToString();
            //index = GameObject.Find("CubeIndex").GetComponent<Text>();
            //PlatformDataNode node = gameObject.GetComponentInChildren<PlatformDataNode>();
            //node.i = configManagerScript.M;
            //node.j = configManagerScript.N;
            //node.NextPosition = gameObject.transform.position.y;
            color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }else if (nextScene.name == "Simulate"){
            ReadFile();
        }
    }

    // Update is called once per frame
    void Update(){
        timeCount -= Time.deltaTime;
        if (SceneManager.GetActiveScene().name == "Programming"){
            program = true;
            if (Input.GetMouseButtonDown(0)){

              
                Vector3 mousePosition = Input.mousePosition;
                bool UIHit = IsUGUIHit(mousePosition);
                if (UIHit == false){
                    clickedGameObject = null;

                    Ray ray = platformCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit = new RaycastHit();

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
                        clickedGameObject = hit.collider.gameObject;
                        str = clickedGameObject.transform.name;
                        Debug.Log(str);
                        strArr = str.Split('/');
                        //index.text = "[" + strArr[1] + ", " + strArr[2] + "]";
                        clickedGameObject.GetComponent<Renderer>().material.color = color;
                    }
                }
            }
        }else if (SceneManager.GetActiveScene().name == "Simulate"){
            //Make a time lag.
            //If I don't make time lag or it is too small, rendering can't keep up with update.
            if(timeCount <= 0){
                Debug.Log("lineNo:" + lineNo);
                StartCoroutine(SimulationNew(lineNo));
                lineNo++;       //Increment the index of processed line
                if(lineNo == X){
                    lineNo = 0;
                    timeCount = 0.01f;
                }else{
                    timeCount = 0.01f;    
                }

            }
        }
    }

    public static bool IsUGUIHit(Vector3 _scrPos){ // Input.mousePosition
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = _scrPos;
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, result);
        return (result.Count > 0);
    }


    public void BuildPlatform(PlatformConfigurationData pcd)
    {
        if (SceneManager.GetActiveScene().name == "Setup")
        {
            //Text textX = GameObject.Find("TextX").GetComponent<Text>(); //Change These
            //Text textY = GameObject.Find("TextY").GetComponent<Text>();// Enter Text Boxes here
            X = pcd.M;
            Y = pcd.N;
            //space = pcd.RandomHeight;
        }
       
        cube = new GameObject[X, Y];
        for (int i = 0; i < X; i += 1)
        {
            for (int j = 0; j < Y; j += 1)
            {
                cube[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //cube[i, j].transform.parent = GameObject.Find("Gameobject").transform;
                cube[i, j].transform.position = new Vector3((i + 0.5f + i * space), 0, j + 0.5f + j * space);
                cube[i, j].transform.rotation = Quaternion.identity;
                cube[i, j].transform.localScale = new Vector3(1, scaleOnY, 1);
                cube[i, j].AddComponent<BoxCollider>();
                cube[i, j].GetComponent<Renderer>().material = material;
                cube[i, j].name = "Cube/" + i.ToString() + "/" + j.ToString();

            }
        }
        built = true;
        
        
    }

   

    public void ReadFile(){
        Debug.Log("Reading");
        FileInfo fi = new FileInfo(Application.dataPath + @"/Resources/NodeData.txt");
        Debug.Log(fi);
        int i, j;
        try{
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8)){
                line = sr.ReadLine();
                dataArray = line.Split('\t');
                X = int.Parse(dataArray[0]);
                Y = int.Parse(dataArray[1]);
                cube = new GameObject[X, Y];
                nodeData = new string[X, Y];
                colorInfo = new Color[X, Y];
                yInfo = new float[X, Y];
                isProgrammed = new float[X, Y];
                for (i = 0; i < X; i++){
                    for (j = 0; j < Y; j++){
                        nodeData[i, j] = sr.ReadLine();
                    }
                }
            }

            //Create platform based on the data
            for (i = 0; i < X; i++){
                for (j = 0; j < Y; j++){
                    dataArray = nodeData[i, j].Split('\t');
                    isProgrammed[i, j] = float.Parse(dataArray[2]);
                    cube[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube[i, j].transform.position = new Vector3(float.Parse(dataArray[1]), float.Parse(dataArray[2]), float.Parse(dataArray[3]));
                    cube[i, j].transform.rotation = Quaternion.identity;
                    cube[i, j].transform.localScale = new Vector3(1, 0.1f, 1);
                    cube[i, j].name = "Cube/" + i.ToString() + "/" + j.ToString();
                    colorData = dataArray[4].Split('(');
                    colorData = colorData[1].Split(')');
                    colorData = colorData[0].Split(',');
                    color = new Color(float.Parse(colorData[0].Trim()), float.Parse(colorData[1].Trim()), float.Parse(colorData[2].Trim()), float.Parse(colorData[3].Trim()));
                    cube[i, j].GetComponent<Renderer>().material.color = color;
                    colorInfo[i, j] = color;
                    yInfo[i, j] = float.Parse(dataArray[2]);
                    //Debug.Log(nodeData[i, j]);
                }
            }
            moveTarget();
        }
        catch (Exception e){
            Debug.Log(e.Message);
        }
    }

    //Shift the next color and position for each base in array colorInfo[,] and yInfo[,]
    public void moveTarget(){
        int i, j;
        Color[] colorInit = new Color[Y];
        float[] yInit = new float[Y];
        for (i = 0; i < X; i++){
            for (j = 0; j < Y; j++){
                if (i == 0){
                    colorInit[j] = colorInfo[0, j];
                    yInit[j] = yInfo[0, j];
                }
                if (i != X - 1){
                    colorInfo[i, j] = colorInfo[i + 1, j];
                    yInfo[i, j] = yInfo[i + 1, j];
                }else{
                    colorInfo[i, j] = colorInit[j];
                    yInfo[i, j] = yInit[j];
                }
            }
        }
    }

    IEnumerator MakeTimeLag(){
        enabled = false;
        yield return new WaitForSeconds(0.5f);
        enabled = true;
    }

    //Change position and color of each unit in line i
    IEnumerator SimulationNew(int i){
        Vector3 yPosNow;
        int j;
        //if(i == X-1){
        enabled = false;    //Stop Update function
        //}
        Debug.Log("Line:" + i);
        for (j = 0; j < Y; j++){
            cube[i, j].GetComponent<Renderer>().material.color = colorInfo[i, j];   //Change color
            yPosNow = cube[i, j].transform.position;
            Debug.Log(i + "," + j + ": " + yPosNow.y + " : " + yInfo[i, j]);
            if (Math.Abs(yPosNow.y - yInfo[i, j]) > 0.01){  //Current and next height are different
                yield return MakeMove(cube[i, j], yPosNow, yInfo[i, j]);
            }
            if (cube[i, j].GetComponent<Renderer>().material.color == Color.white){ //If the base is white (not programmed), set y = 0
                yPosNow.y = 0;
                cube[i, j].transform.position = yPosNow;
            }
        }
        if (i == X - 1){       //After all lines are processed, update the information of next color and position 
            moveTarget();
        }
        enabled = true;     //Start Update function
    }

    //Move each base to the next position
    IEnumerator MakeMove(GameObject cubeMove, Vector3 yPosNow, float yPosNext){
        if (yPosNow.y < yPosNext){
            while (yPosNow.y < yPosNext){   //Next height is higher than current height
                yield return new WaitForSeconds(0.0001f);
                yPosNow.y += 0.15f;
                cubeMove.transform.position = yPosNow;
            }
        }else{
            while (yPosNow.y > yPosNext){   //Nect height is lower than current height
                yield return new WaitForSeconds(0.0001f);
                yPosNow.y -= 0.15f;
                cubeMove.transform.position = yPosNow;
            }
        }
    }
}