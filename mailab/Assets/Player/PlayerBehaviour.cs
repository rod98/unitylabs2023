using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject playerHead;
    public GameObject playerBody;
    public GameObject weapon_prefab;
    private float time = 0.0f;
    private float jumpTimeout = 0.5f;
    private float lastJump = 0.0f;
    private float lastAttack = 0.0f;
    // private bool isMoving = false;
    private Rigidbody rigidBody;

    private int max_jumps = 3;
    private int left_jumps = 0;

    float rspeed = 3f;

    private GameObject hRotationalObject;
    private GameObject vRotationalObject;

    void Start(){
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.detectCollisions = true;

        hRotationalObject = playerBody;
        vRotationalObject = playerHead;
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

        float vx_angle = vRotationalObject.transform.eulerAngles.x;

        if (vx_angle > 180)
            vx_angle -= 360;

        if (Input.GetAxis("Mouse Y") < 0f) {
            //Code for action on mouse moving up
            if (vx_angle < 75)
                vRotationalObject.transform.Rotate( rspeed, 0f, 0f);
        }
        if (Input.GetAxis("Mouse Y") > 0f) {
            //Code for action on mouse moving down
            if (vx_angle > -75)
                vRotationalObject.transform.Rotate(-rspeed, 0f, 0f);
        }

        // Debug.Log(
        //     ((int)vx_angle).ToString() + "; " +
        //     ((int)vRotationalObject.transform.eulerAngles.y).ToString() + "; " +
        //     ((int)vRotationalObject.transform.eulerAngles.z).ToString()
        // );
    }

    void FixedUpdate() {
        time += Time.fixedDeltaTime;

        ProcessControls();
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            // Debug.Log(contact.point);

            // if point is below object - TODO: add check
            left_jumps = max_jumps;
        }
        // if (collision.relativeVelocity.magnitude > 2)
        //     audioSource.Play();
    }

    private float ToRad(float degrees) {
        return degrees * Mathf.PI / 180f;
    }

    void ProcessControls() {
        KeyCode jumpKey = KeyCode.Space;
        KeyCode forwKey = KeyCode.W;
        KeyCode backKey = KeyCode.S;
        KeyCode rghtKey = KeyCode.D;
        KeyCode leftKey = KeyCode.A;
        KeyCode atckKey = KeyCode.Mouse0;
        int hspeed = 5;
        int vspeed = 10;

        int fwsgn = 0;
        int lrsgn = 0;

        float hangle = hRotationalObject.transform.eulerAngles.y;

        if (Input.GetKey(jumpKey) && time - lastJump >= jumpTimeout && left_jumps > 0) {
            // myBody.velocity = new Vector3(0,10,0);
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, vspeed, rigidBody.velocity.z);
            // isMoving = true;
            // Debug.Log("jump");

            lastJump = time;
            left_jumps -= 1;
        }

        if (Input.GetKey(atckKey) && time - lastAttack > 0.3f) {
            Vector3 global_pos = playerHead.transform.position;
            // global_pos.y += 10;
            // global_pos.x += 2 * Mathf.Sin(ToRad(hangle));
            // global_pos.z += 2 * Mathf.Cos(ToRad(hangle));
            global_pos += vRotationalObject.transform.forward.normalized * 2;
            GameObject attack_obj = Instantiate(weapon_prefab, global_pos, Quaternion.identity);

            attack_obj.transform.rotation = hRotationalObject.transform.rotation;

            Rigidbody rb = attack_obj.GetComponent<Rigidbody>();
            rb.velocity = 10 * vRotationalObject.transform.forward.normalized;

            lastAttack = time;
        }

        if (Input.GetKey(forwKey))
            fwsgn = 1;
        
        if (Input.GetKey(backKey))
            fwsgn = -1;
        

        if (Input.GetKey(rghtKey))
            lrsgn = 1;
        
        if (Input.GetKey(leftKey))
            lrsgn = -1;
        
        // Debug.Log(playerHead.transform.rotation.y);

        if (fwsgn != 0) {
            float x_speed = fwsgn * hspeed * Mathf.Sin(ToRad(hangle));
            float z_speed = fwsgn * hspeed * Mathf.Cos(ToRad(hangle));
            rigidBody.velocity = new Vector3(x_speed, rigidBody.velocity.y, z_speed);
        }
         if (lrsgn != 0) {
            float x_speed =  lrsgn * hspeed * Mathf.Cos(ToRad(hangle));
            float z_speed = -lrsgn * hspeed * Mathf.Sin(ToRad(hangle));
            rigidBody.velocity = new Vector3(x_speed, rigidBody.velocity.y, z_speed);
        }
    }
}
