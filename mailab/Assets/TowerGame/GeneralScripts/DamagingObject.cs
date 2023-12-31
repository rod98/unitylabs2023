using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObject : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 10f;
    public PlayerBehaviour owner = null;

    public bool doDestructOnCollision = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() 
    {
        lifetime -= Time.fixedDeltaTime;

        if (lifetime <= 0)
            Destruct();
    }

    void OnCollisionEnter(Collision collision) {
        if (doDestructOnCollision)
            Destruct();
    }

    void Destruct() {
        // Destroy(this.gameObject.transform.parent.gameObject);
        Destroy(this.gameObject);
    }
}
