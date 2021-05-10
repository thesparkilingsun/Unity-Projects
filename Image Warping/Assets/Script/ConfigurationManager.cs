using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ConfigurationManager : MonoBehaviour {

   public RectTransform imagePanel;
    public RawImage imageHolder;
    GameObject emptyObject;
   public Button loadImgBtn;
    string path;
    Texture2D texture;
    Sprite sprite;
    // Use this for initialization
    void Start () {

           emptyObject = new GameObject();
        
        imagePanel.gameObject.SetActive(false);
        

        


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //public void HideImageHolder()
    //{
    //    Image image = imageHolder.gameObject.GetComponentInChildren<Image>();

    //    if (image == null)
    //    {
    //        imageHolder.gameObject.SetActive(false);
    //    }

    //}
    public void OnLoadImageBtnClicked()
    {
        if(loadImgBtn.IsActive())
        {
            path = UnityEditor.EditorUtility.OpenFilePanel("Load an Image", "", "png,jpeg,jpg,tiff");
           texture = LoadPNG(path);
            imagePanel.gameObject.SetActive(true);
            imageHolder.enabled = true;
            imageHolder.texture = texture;
            
        }

    }



    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2,2 );
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            
        }
        return tex;
    }


}
