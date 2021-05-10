
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour {

    public float speed = 10.0f;

    protected float fDistance = 1;
    protected float fSpeed = 1;
    public Camera camera;




    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(6, 6, 6);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        

            #region ZOOM IN/OUT
            if (Input.GetKey(KeyCode.LeftShift))
                if (Input.GetKey(KeyCode.UpArrow))
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift))
                if (Input.GetKey(KeyCode.DownArrow))
                    transform.Translate(-Vector3.forward * speed * Time.deltaTime);
            #endregion

            #region ROTATE VERTICAL/HORIZONTAL
            if (Input.GetKey(KeyCode.RightArrow))
                transform.RotateAround(pos, -Vector3.up, Time.deltaTime * speed);
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.RotateAround(pos, Vector3.up, Time.deltaTime * speed);


            if (Input.GetKey(KeyCode.UpArrow))
                camera.transform.RotateAround(pos, Vector3.right, Time.deltaTime * speed);
            if (Input.GetKey(KeyCode.DownArrow))
                camera.transform.RotateAround(pos, -Vector3.right, Time.deltaTime * speed);
      
        #endregion
    }
}
