using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {

    public GameObject Base;
   public  GameObject cube;
    Vector3[] normal;
  public Material myMaterial;
    public Material yellow;
    public Material green;
    GameObject positioner;
   

    // Material blockMaterial;
    // Use this for initialization
   

   


    void Start () {

        Instantiate(Base, transform.position = new Vector3(0, 0, 0),Quaternion.identity);
        positioner = GameObject.CreatePrimitive(PrimitiveType.Cube);
        positioner.tag = "Player";
        positioner.gameObject.AddComponent<BoxCollider>();
  
    }


    

    // Update is called once per frame
    void Update () {

        
        RaycastHit hitInfo = new RaycastHit();

        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        Collider col = hitInfo.collider;
        normal = new Vector3[]
                   {
                       new Vector3(0, 0, 1.0f), // 0
                       new Vector3(0, 0, -1.0f), // 1
                       new Vector3(0,1.0f,0), // 2
                       new Vector3(1.0f,0,0), // 3
                       new Vector3(-1.0f,0,0),//4
                        new Vector3(0,-1.0f,0) // 5
                   };

        if (hit)
        {
           


            if (col.gameObject.tag == "Base")
            {
                positioner.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.5f,hitInfo.point.z);
                positioner.transform.gameObject.GetComponent<Renderer>().material = yellow;
                Debug.Log("Hitting Base");
            }
            if (col.gameObject.tag == "MyCube")
            {

                positioner.transform.gameObject.GetComponent<Renderer>().material = green;
                if (hitInfo.normal == normal[1])
                {
                    positioner.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.normal.z - 0.66f);
                    //cube.transform.position = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, hitInfo.transform.position.z - 1.0f);
                }

                if (hitInfo.normal == normal[0])
                {
                    positioner.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z + 0.64f);
                    //cube.transform.position = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, hitInfo.transform.position.z + 1.0f);
                }
                if (hitInfo.normal == normal[2])
                {
                    positioner.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.71f, hitInfo.point.z);
                    //cube.transform.position = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y + 0.5f, hitInfo.transform.position.z);
                }
                if (hitInfo.normal == normal[3])
                {
                    positioner.transform.position = new Vector3(hitInfo.point.x + 0.78f, hitInfo.point.y, hitInfo.point.z);
                    // cube.transform.position = new Vector3(hitInfo.transform.position.x + 1.0f, hitInfo.transform.position.y, hitInfo.transform.position.z);
                }
                if (hitInfo.normal == normal[4])
                {
                    positioner.transform.position = new Vector3(hitInfo.point.x - 0.78f, hitInfo.point.y , hitInfo.point.z);
                    // cube.transform.position = new Vector3(hitInfo.transform.position.x - 1.0f, hitInfo.transform.position.y, hitInfo.transform.position.z);
                }


            }



            if (Input.GetMouseButtonUp(0))
            {
                

                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(1, 1, 1);
                cube.tag = "MyCube";
                cube.transform.gameObject.GetComponent<Renderer>().material = myMaterial;
                cube.gameObject.transform.position = hitInfo.point;
  
                //Code End for vertex colod change


               


                   
               Debug.Log( hitInfo.normal+""+"Location: "+hitInfo.point);
               
                
                


            //    Debug.Log("Original Position: "+col.gameObject.transform.position);
            //    Debug.Log("Changed Position: "+col.gameObject.transform.position);



                if (col.gameObject.tag == "MyCube")
                {
                    Debug.Log("Cube HIT!!!");

                    //cube.transform.parent = col.gameObject.transform;
                    if (hitInfo.normal == normal[0])
                    {
                        cube.transform.position = new Vector3(hitInfo.transform.localPosition.x, hitInfo.transform.localPosition.y, hitInfo.transform.localPosition.z + 1.0f);
                    }
                    if (hitInfo.normal == normal[1])
                    {

                        cube.transform.position = new Vector3(hitInfo.transform.localPosition.x, hitInfo.transform.localPosition.y, hitInfo.transform.localPosition.z - 1.0f);
                    } 
                    if (hitInfo.normal == normal[2])
                    {
                        cube.transform.position = new Vector3(hitInfo.transform.localPosition.x, hitInfo.transform.localPosition.y + 1.0f, hitInfo.transform.localPosition.z);
                    }
                    if (hitInfo.normal == normal[3])
                    {
                        cube.transform.position = new Vector3(hitInfo.transform.localPosition.x + 1.0f, hitInfo.transform.localPosition.y, hitInfo.transform.localPosition.z);
                    }
                    if (hitInfo.normal == normal[4])
                    {
                        cube.transform.position = new Vector3(hitInfo.transform.localPosition.x - 1.0f, hitInfo.transform.localPosition.y, hitInfo.transform.localPosition.z);
                    }
                    if(hitInfo.normal == normal[5])
                    {
                        cube.transform.position = new Vector3(hitInfo.transform.localPosition.x, hitInfo.transform.localPosition.y - 1.0f, hitInfo.transform.localPosition.z);
                    }
                }
                if (col.gameObject.tag == "Base")
                {

                    cube.transform.position = new Vector3(hitInfo.transform.localPosition.x, hitInfo.transform.localPosition.y + 0.34f, hitInfo.transform.localPosition.z);
                    Debug.Log("Base Hit");
                    
                }


            }
            else
            {
                Debug.Log("No HIt");
            }
          

        }
        else
        {
            Debug.Log("Say Something stupid!!");
        }



    }


  

}
