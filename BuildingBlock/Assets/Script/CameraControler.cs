using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {

    float zoom = 10.0f;
  
    private float speed = 15.0f;
    
	// Update is called once per frame


	void Update () {

       

        Vector3 pos = transform.position;
       
        transform.LookAt(pos);
        if (Input.GetKey(KeyCode.W))
        {
            pos.y += zoom * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            pos.y -= zoom * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.z += zoom * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            pos.z -= zoom * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            pos.x += zoom * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            pos.x -= zoom * Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(pos, -Vector3.up, Time.deltaTime * speed);
        }
       
        if (Input.GetKey(KeyCode.LeftArrow))
              {
                 transform.RotateAround(pos, Vector3.up, Time.deltaTime * speed);
              }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.RotateAround(pos, new Vector3(1,0,0), Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.RotateAround(pos, new Vector3(-1, 0, 0), Time.deltaTime * speed);
        }



        transform.position = pos;
	}
}
