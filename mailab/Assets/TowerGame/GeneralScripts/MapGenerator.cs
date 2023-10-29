using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // public GameObject floor_prefab;
    public GameObject house_prefab;
    public String mapFilename = "";

    GameObject Generate() {
        String fullPath = "Maps/" + mapFilename; // + ".json";
        TextAsset text = Resources.Load<TextAsset>(fullPath) as TextAsset;
        // Debug.Log(fullPath);
        // Debug.Log(text);

        List<GameObject> floor_prefabs = new List<GameObject>(); 
        for (int i = 0; i < 99; i++) {
            String nm = "Prefabs/Structures/Floor" + i.ToString();
            // Debug.Log(Application.dataPath + "Resources/Prefabs/Structures/" + nm + ".prefab");
            GameObject current_floor_prefab = Resources.Load(nm) as GameObject;
            if (current_floor_prefab) {
                Debug.Log("Successfully loaded " + nm);
                // floor_prefabs[i] = current_floor_prefab;
                floor_prefabs.Add(current_floor_prefab);
            }
        }

        int cnt = floor_prefabs.Count;
        Debug.Log("Total different floors: " + cnt.ToString());

        float y_size = 0;
        if (house_prefab.transform.childCount > 0) {
            GameObject house_model = house_prefab.transform.GetChild(0).gameObject;
            y_size = house_model.GetComponent<Renderer>().bounds.size.y;
        }
        else {
            y_size = house_prefab.GetComponent<Renderer>().bounds.size.y;
        }

        Debug.Log("Instantiated the house");

        GameObject level = new("LevelMap");

        if (text) {
            LevelMap mapJson = JsonUtility.FromJson<LevelMap>(text.text);
            Debug.Log(mapJson.size);
            Vector3 tile_size = mapJson.floor_tile_size;

            float   playerBaseYPos = mapJson.GetHeight((int)mapJson.base_position.x, (int)mapJson.base_position.z) * tile_size.y;
            Vector3 playerBasePos  = new(
                mapJson.base_position.x * tile_size.x, 
                y_size / 2f + playerBaseYPos, 
                mapJson.base_position.z * tile_size.z
            );
            GameObject playerBase = Instantiate(house_prefab, playerBasePos, Quaternion.identity);
            playerBase.transform.SetParent(level.transform);

            Vector3 sz = mapJson.size;
            // float[] heights = mapJson.height_map;

            for (int iz = 0; iz < (int)sz.z; iz++)
                for (int ix = 0; ix < (int)sz.x; ix++) {
                    // float height = heights[ix + iz * (int)sz.z];
                    float height = mapJson.GetHeight(ix, iz);

                    GameObject current_floor_prefab = floor_prefabs[(int)height % floor_prefabs.Count];

                    // int px = (int)(sz.x/2) - ix;
                    // int pz = (int)(sz.z/2) - iz;
                    int px = ix;
                    int pz = iz;
                    int posx = px * (int)tile_size.x;
                    int posy = (int)(tile_size.y * (height - 0.5));
                    int posz = pz * (int)tile_size.z;
                    GameObject floor = Instantiate(current_floor_prefab, new Vector3(posx, posy, posz), Quaternion.identity);
                    floor.name = "floor@" + px.ToString() + "x" +  ((int)height).ToString() + "x" + pz.ToString();
                    floor.transform.SetParent(level.transform);
                }

            int pposx = (int)tile_size.x * (int)mapJson.starting_player_position.x;
            int pposy = (int)(mapJson.GetHeight((int)mapJson.starting_player_position.x, (int)mapJson.starting_player_position.z) * tile_size.y) + 1;
            int pposz = (int)tile_size.z * (int)mapJson.starting_player_position.z;
            GameObject playerObj = GameObject.FindGameObjectsWithTag("Player")[0];
            playerObj.transform.localPosition = new(pposx, pposy, pposz);
        }
        else {
            for (int iy = -1; iy < 1; iy++)
                for (int ix = -1; ix < 1; ix++) {
                    // int posx = ix * (int)floor_prefabs[0].GetComponent<Renderer>().bounds.size.x;
                    // int posy =    - (int)floor_prefabs[0].GetComponent<Renderer>().bounds.size.y / 2;
                    // int posz = iy * (int)floor_prefabs[0].GetComponent<Renderer>().bounds.size.z;
                    int posx = ix * (int)floor_prefabs[0].GetComponent<Renderer>().bounds.size.x;
                    int posy =    - (int)floor_prefabs[0].GetComponent<Renderer>().bounds.size.y / 2;
                    int posz = iy * (int)floor_prefabs[0].GetComponent<Renderer>().bounds.size.z;
                    GameObject floor = Instantiate(floor_prefabs[0], new Vector3(posx, posy, posz), Quaternion.identity);
                }
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