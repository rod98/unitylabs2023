using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject playerHead;
    public GameObject playerBody;
    public GameObject weapon_prefab;
    public GameObject killLabelObject = null;

    private int killPoints = 0;
    private TextMeshProUGUI killLabelText = null;
    private float time = 0.0f;
    private float jumpTimeout = 0.5f;
    private float lastJump = 0.0f;
    private float lastAttack = 0.0f;
    // private bool isMoving = false;
    private Rigidbody rigidBody;

    private int max_jumps = 3;
    private int left_jumps = 0;

    float rspeed = 10f;

    private GameObject hRotationalObject;
    private GameObject vRotationalObject;

    public void AddKillPoints(int points) {
        killPoints += points;

        // Debug.Log("New   kill points: " + points.ToString());
        // Debug.Log("Total kill points: " + killPoints.ToString());
    }

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.detectCollisions = true;

        hRotationalObject = playerBody;
        vRotationalObject = playerHead;

        // if (killLabelObject)
        killLabelText = killLabelObject.GetComponent<TextMeshProUGUI>();
        // playerObj = GetComponent<GameObject>();
    }
    void Update()
    {

        float dx = Input.GetAxis("Mouse X");
        hRotationalObject.transform.Rotate(0f, dx * rspeed, 0f);

        float vx_angle = vRotationalObject.transform.eulerAngles.x;

        if (vx_angle > 180)
            vx_angle -= 360;

        float dy = Input.GetAxis("Mouse Y");
        if ((dy < 0f && vx_angle < 75) || (dy > 0 && vx_angle > -75)) 
            vRotationalObject.transform.Rotate(-dy * rspeed, 0f, 0f);

        // Debug.Log(
        //     ((int)vx_angle).ToString() + "; " +
        //     ((int)vRotationalObject.transform.eulerAngles.y).ToString() + "; " +
        //     ((int)vRotationalObject.transform.eulerAngles.z).ToString()
        // );
        // if (killLabelText)
        killLabelText.text = "Points: " + killPoints.ToString();
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

            // if point is below object - TODO: make the check better (check the collision is actually between the legs)
            if (collision.transform.position.y < playerBody.transform.position.y)
                left_jumps = max_jumps;
        }
        // if (collision.relativeVelocity.magnitude > 2)
        //     audioSource.Play();
    }

    // private float ToRad(float degrees) {
    //     return degrees * Mathf.PI / 180f;
    // }

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

        // float hangle = hRotationalObject.transform.eulerAngles.y;

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

            DamagingObject dobj = attack_obj.GetComponent<DamagingObject>();
            dobj.owner = this;

            Rigidbody rb = attack_obj.GetComponent<Rigidbody>();
            rb.velocity = 100 * vRotationalObject.transform.forward.normalized;

            lastAttack = time;
        }

        if (Input.GetKey(forwKey))
            fwsgn = 1;
        
        if (Input.GetKey(backKey))
            fwsgn = -1;
        

        if (Input.GetKey(rghtKey))
            lrsgn = -1;
        
        if (Input.GetKey(leftKey))
            lrsgn = 1;
        
        // Debug.Log(playerHead.transform.rotation.y);

        Vector3 speedDirection = new Vector3(0, 0, 0);

        if (fwsgn != 0) {
            speedDirection = hRotationalObject.transform.forward * fwsgn;
            // float x_speed = fwsgn * hspeed * Mathf.Sin(ToRad(hangle));
            // float z_speed = fwsgn * hspeed * Mathf.Cos(ToRad(hangle));
            // rigidBody.velocity = new Vector3(x_speed, rigidBody.velocity.y, z_speed);
            
            // rigidBody.velocity = new Vector3(x_speed, rigidBody.velocity.y, z_speed);
        }
         if (lrsgn != 0) {
            Vector3 side    = Vector3.Cross(hRotationalObject.transform.forward, hRotationalObject.transform.up);
            speedDirection  = speedDirection.normalized;
            speedDirection += side.normalized * lrsgn;
            // float x_speed =  lrsgn * hspeed * Mathf.Cos(ToRad(hangle));
            // float z_speed = -lrsgn * hspeed * Mathf.Sin(ToRad(hangle));
            // rigidBody.velocity = new Vector3(x_speed, rigidBody.velocity.y, z_speed);
        }
        float y = rigidBody.velocity.y;
        rigidBody.velocity = speedDirection.normalized * hspeed + new Vector3(0, y, 0);
    }
}
