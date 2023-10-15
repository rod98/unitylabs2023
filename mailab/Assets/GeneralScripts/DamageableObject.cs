using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DamageableObject : MonoBehaviour
{
    private int health;
    public int defaultHealth;

    public void GetDamaged(int damage_value) {
        health -= damage_value;
        if (health <= 0)
            Die();
    }

    void Die() {

    }

    public void ResetHealth() {
        health = defaultHealth;
    }

    void Start()
    {
        ResetHealth();

        GameObject myGO;
        GameObject myText;
        Canvas myCanvas;
        Text text;
        RectTransform rectTransform;

        // Canvas
        myGO = new GameObject("ScriptCanvas");
        myGO.AddComponent<Canvas>();

        myCanvas = myGO.GetComponent<Canvas>();
        // myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        myGO.AddComponent<CanvasScaler>();
        myGO.AddComponent<GraphicRaycaster>();

        myGO.transform.SetParent(gameObject.transform);

        // // Text
        myText = new GameObject();
        myText.transform.parent = myCanvas.transform;
        myText.name = "TextObject";

        text = myText.AddComponent<Text>();
        text.font = (Font)Resources.Load("MyFont");
        text.text = "wobble";
        text.fontSize = 100;

        // // Text position
        rectTransform = text.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 50, 0);
        rectTransform.sizeDelta = new Vector2(400, 200);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
