using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FileCreate : MonoBehaviour
{

    GameObject platformManager;
    PlatformManager platformManagerScript;
    Button fileButton;
    int i, j;
    float posX, posY, posZ;
    Color color;
    string filePath;
    string writeData;

    // Use this for initialization
    void Start(){
        filePath = Application.dataPath + @"/Resources/NodeData.txt";
        platformManager = GameObject.Find("--GameObject");
        platformManagerScript = platformManager.GetComponent<PlatformManager>();
        fileButton = GameObject.Find("btnProgramPlatform").GetComponent<Button>();
        fileButton.onClick.AddListener(FileWrite);
    }

    // Update is called once per frame
    void FileWrite(){
        File.WriteAllText(filePath, platformManagerScript.X + "\t" + platformManagerScript.Y + "\t" + platformManagerScript.space + "\t" + platformManagerScript.range + "\n");
        for (i = 0; i < platformManagerScript.X; i++)
        {
            for (j = 0; j < platformManagerScript.Y; j++)
            {
                posX = platformManagerScript.cube[i, j].transform.position.x;
                posY = platformManagerScript.cube[i, j].transform.position.y;
                posZ = platformManagerScript.cube[i, j].transform.position.z;
                color = platformManagerScript.cube[i, j].GetComponent<Renderer>().material.color;
                writeData = platformManagerScript.cube[i, j].name + "\t" + posX + "\t" + posY + "\t" + posZ + "\t" + color + "\n";
                File.AppendAllText(filePath, writeData);
            }
        }
        Debug.Log("Data Saved");
    }
}