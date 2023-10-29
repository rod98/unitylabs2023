using System;
using UnityEngine;

[System.Serializable]

public class LevelMap
{
    public Vector3 size;
    public Vector3 floor_tile_size;
    public float[] height_map;
    // void LevelMap();
    public Vector3 base_position;
    public Vector3 starting_player_position;

    public float GetHeight(int x, int z) {
        if (x < size.x && z < size.z) {
            return height_map[x + z * (int)size.z];
        }

        return 0;
    }
}
 
