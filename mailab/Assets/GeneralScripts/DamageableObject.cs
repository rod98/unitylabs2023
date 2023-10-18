using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DamageableObject : MonoBehaviour
{
    private float time = 0.0f;
    private float last_hit = 0.0f;
    private int health;
    public int maxHealth;
    private Slider healthbar_comp;
    private GameObject healthbar_obj;
    public GameObject mainCamera_obj = null;
    private Camera mainCamera_comp;
    private GameObject canvas_obj;
    private RectTransform canvas_rect_transform;

    public void GetDamaged(int damage_value) {
        if (health > 0) {
            health -= damage_value;
            Debug.Log(name + " gets damaged by " + damage_value.ToString() + " (" + health.ToString() + " health left)");
        }

        if (health <= 0)
            Die();

        healthbar_comp.value = health;
    }

    void Die() {
        Debug.Log(name + " is dead!");
    }

    public void ResetHealth() {
        if (maxHealth <= 0)
            maxHealth = 1;

        health = maxHealth;
        healthbar_comp.value    = health;
        healthbar_comp.maxValue = maxHealth;
    }

    private float MaxHeight(GameObject gobj) {
        float mx = 0;

        if (gobj.GetComponent<Renderer>() != null) {
            // Debug.Log("Got renderer");
            mx = Mathf.Max(mx, gobj.GetComponent<Renderer>().bounds.size.y);
        }

        int childCount = gobj.transform.childCount;
        // Debug.Log("Child Count: " + childCount.ToString());
        for (int i = 0; i < childCount; ++i) {
            GameObject one_child = gobj.transform.GetChild(i).gameObject;
            // float ny = one_child.GetComponent<Renderer>().bounds.size.y;
            float ny = MaxHeight(one_child);
            mx = Mathf.Max(mx, ny);
        }
        // Debug.Log("Found height: " + mx.ToString());
        return mx;
    }

    void Start()
    {
        GameObject canvas_obj;
        
        Canvas canvas_comp;
        // Text text;
        // RectTransform rectTransform;
        GameObject healthBarSlider_prefab = Resources.Load("Prefabs/UI/HealthBarSlider") as GameObject;

        float object_height = MaxHeight(gameObject);
        Debug.Log("object_height: " + object_height.ToString());

        if (!mainCamera_obj)
            mainCamera_obj = GameObject.FindGameObjectWithTag("MainCamera"); //.GetComponent<Camera>();

        mainCamera_comp = mainCamera_obj.GetComponent<Camera>();

        canvas_obj = mainCamera_obj.transform.GetChild(0).gameObject;
        canvas_comp = canvas_obj.GetComponent<Canvas>();

        canvas_rect_transform = canvas_obj.GetComponent<RectTransform>();

        // Canvas
        // canvas_obj = new GameObject("ScriptCanvas");

        // canvas_obj.transform.SetParent(gameObject.transform, true);
        // // canvas_obj.transform.localPosition = new Vector3(0, 0, 0);

        // canvas_obj.AddComponent<Canvas>();
        // canvas_comp = canvas_obj.GetComponent<Canvas>();
        // // canvas_comp.transform.SetParent(canvas_obj.transform, true);
        // float sy = gameObject.transform.localScale.y;
        // canvas_comp.transform.localPosition = new Vector3(0, (object_height + 1) / sy, 0);

        // canvas_comp.renderMode = RenderMode.WorldSpace;
        // canvas_comp.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // canvas_obj.AddComponent<CanvasScaler>();
        // canvas_obj.AddComponent<GraphicRaycaster>();

        healthbar_obj = Instantiate(healthBarSlider_prefab, new Vector3(0, 0, 0), Quaternion.identity);
        // // healthbar_obj = new GameObject();
        healthbar_obj.transform.SetParent(canvas_comp.transform, true);
        healthbar_obj.name = "HealthbarObject";

        healthbar_obj.transform.localPosition = new Vector3(0, 0, 0);
        // healthbar_obj.transform.localScale = new Vector3(5, 5, 5);
        // // Slider healthbar_comp;
        // // healthbar_comp = healthbar_obj.AddComponent<Slider>();
        // // // Image healthbar_comp = healthbar_obj.AddComponent<Image>();
        // // healthbar_obj.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        healthbar_comp = healthbar_obj.GetComponent<Slider>();
        // healthbar_comp.maxValue = maxHealth;

        ResetHealth();
    }

    Vector2 WorldToScreenPointAdjusted(Vector3 pos) {
        Vector2 adjustedPosition = mainCamera_comp.WorldToScreenPoint(pos);
        adjustedPosition.x *= canvas_rect_transform.rect.width  / (float)mainCamera_comp.pixelWidth;
        adjustedPosition.y *= canvas_rect_transform.rect.height / (float)mainCamera_comp.pixelHeight;
        return adjustedPosition - canvas_rect_transform.sizeDelta / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 screenPos = mainCamera_comp.WorldToScreenPoint(gameObject.transform.position);

        Vector3 min = gameObject.GetComponent<Renderer>().bounds.min;
        Vector3 max = gameObject.GetComponent<Renderer>().bounds.max;
        
        // Vector3 screenMin = mainCamera_comp.WorldToScreenPoint(min);
        // Vector3 screenMax = mainCamera_comp.WorldToScreenPoint(max);

        float height = max.y - min.y;

        min.y = max.y + height * 0.25f;
        max.y = min.y;

        Vector3 middle = (min + max) / 2;

        // Vector3 screenMin = WorldToScreenPointAdjusted(min);
        // Vector3 screenMax = WorldToScreenPointAdjusted(max);
        
        // float screenWidth  = screenMax.x - screenMin.x;
        // if (screenMin.y < (float)mainCamera_comp.pixelHeight/2)
        //     ;
        // screenMin.y = Mathf.Max(screenMin.y, -(float)mainCamera_comp.pixelHeight);

        // float screenHeight = screenMax.y - screenMin.y;
        // // Debug.Log((float)mainCamera_comp.pixelHeight);
        // Debug.Log(screenMax.y.ToString() + "; " +  screenMin.y.ToString());
        

        // // canvas_obj = mainCamera_obj.transform.GetChild(0).gameObject;
        // // canvas_comp = canvas_obj.GetComponent<Canvas>();

        // // Canvas canvas_comp = canvas_obj.GetComponent<Canvas>();
        Vector2 adjustedPosition = WorldToScreenPointAdjusted(middle);

        // adjustedPosition.y += height * 0.5f;

        // healthbar_obj.transform.localPosition = adjustedPosition;
        healthbar_obj.transform.localPosition = adjustedPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        // if (time - last_hit > 1f) {
        GetDamaged(10);
        last_hit = time;
        // }
    }

    void FixedUpdate() {
        time += Time.fixedDeltaTime;
    }
}
