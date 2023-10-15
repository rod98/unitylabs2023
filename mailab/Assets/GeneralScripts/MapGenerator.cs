using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject floor_prefab;
    public GameObject house_prefab;

    GameObject Generate() {
        GameObject level = new GameObject();
        GameObject house = Instantiate(house_prefab, new Vector3(5, house_prefab.GetComponent<Renderer>().bounds.size.y / 2f, 5), Quaternion.identity);

        Debug.Log("Instantiated the house");

        for (int iy = -5; iy < 5; iy++)
            for (int ix = -5; ix < 5; ix++) {
                int posx = ix * (int)floor_prefab.GetComponent<Renderer>().bounds.size.x;
                int posz = iy * (int)floor_prefab.GetComponent<Renderer>().bounds.size.z;
                GameObject floor = Instantiate(floor_prefab, new Vector3(posx, 0, posz), Quaternion.identity);
                floor.transform.SetParent(level.transform, false);
            }

        return level;
    }
    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
