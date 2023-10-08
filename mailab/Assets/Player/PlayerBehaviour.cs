using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private float time = 0.0f;
    private float jumpTimeout = 0.5f;
    private float lastJump = 0.0f;
    // private bool isMoving = false;
    public GameObject playerHead;
    public GameObject playerBody;
    public Rigidbody rigidBody;

    private int max_jumps = 3;
    private int left_jumps = 0;

    float rspeed = 3f;

    private GameObject hRotationalObject;

    void Start(){
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.detectCollisions = true;

        hRotationalObject = playerBody;
        // playerObj = GetComponent<GameObject>();
    }
    void Update()
    {
        if(Input.GetAxis("Mouse X")<0f){
            //Code for action on mouse moving left
            hRotationalObject.transform.Rotate(0f, -rspeed, 0f);
        }
        if(Input.GetAxis("Mouse X")>0f){
            //Code for action on mouse moving right
            hRotationalObject.transform.Rotate(0f, rspeed, 0f);
        }
    }

    void FixedUpdate() {
        time = time + Time.fixedDeltaTime;

        ProcessControls();
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            Debug.Log(contact.point);

            // if point is below object - TODO: add check
            left_jumps = max_jumps;
        }
        // if (collision.relativeVelocity.magnitude > 2)
        //     audioSource.Play();
    }

    void ProcessControls() {
        KeyCode jumpKey = KeyCode.Space;
        KeyCode forwKey = KeyCode.W;
        KeyCode backKey = KeyCode.S;
        int hspeed = 5;
        int vspeed = 10;

        int sgn = 0;

        if (Input.GetKey(jumpKey) && time - lastJump >= jumpTimeout && left_jumps > 0) {
            // myBody.velocity = new Vector3(0,10,0);
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, vspeed, rigidBody.velocity.z);
            // isMoving = true;
            Debug.Log("jump");

            lastJump = time;
            left_jumps -= 1;
        }

        if (Input.GetKey(forwKey)){
            // myBody.velocity = new Vector3(myBody.velocity.x, myBody.velocity.y, hspeed);
            sgn = 1;
        }

        if (Input.GetKey(backKey)){
            // myBody.velocity = new Vector3(myBody.velocity.x, myBody.velocity.y, -hspeed);
            sgn = -1;
        }

        // Debug.Log(playerHead.transform.rotation.y);

        if (sgn != 0) {
            float x_speed = sgn * hspeed * Mathf.Sin(hRotationalObject.transform.eulerAngles.y * Mathf.PI / 180f);
            float z_speed = sgn * hspeed * Mathf.Cos(hRotationalObject.transform.eulerAngles.y * Mathf.PI / 180f);
            rigidBody.velocity = new Vector3(x_speed, rigidBody.velocity.y, z_speed);
        }
    }
}
