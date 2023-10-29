using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class PlayerDamageable : DamageableObject
{
    void Start()
    {
        // Canvas canvas_comp = null;
        // Text text;
        // RectTransform rectTransform;
       
        SetupHealthbar();
        ResetHealth();
        
        // RectTransform healthbarRectTransform = healthbar_obj.GetComponent<RectTransform>();
        // healthbarRectTransform.pivot = new Vector2(0, 0);
        // healthbarRectTransform.anchorMin = new Vector2(0, 1);
        // healthbarRectTransform.anchorMax = new Vector2(0, 1);

        // healthbarRectTransform.anchoredPosition = new Vector2(1, 1);

    }

    void Update() {

    }
}