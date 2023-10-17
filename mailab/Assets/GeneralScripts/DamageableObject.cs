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

    public void GetDamaged(int damage_value) {
        if (health > 0) {
            health -= damage_value;
            Debug.Log(name + " gets damaged by " + damage_value.ToString() + " (" + health.ToString() + " health left)");
        }

        if (health <= 0)
            Die();
    }

    void Die() {
        Debug.Log(name + " is dead!");
    }

    public void ResetHealth() {
        if (maxHealth <= 0)
            maxHealth = 1;

        health = maxHealth;
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
        ResetHealth();

        GameObject canvas_obj;
        GameObject healthbar_obj;
        Canvas canvas_comp;
        // Text text;
        // RectTransform rectTransform;
        GameObject healthBarSlider_prefab = Resources.Load("Prefabs/UI/HealthBarSlider") as GameObject;

        float object_height = MaxHeight(gameObject);
        Debug.Log("object_height: " + object_height.ToString());

        // Canvas
        canvas_obj = new GameObject("ScriptCanvas");

        canvas_obj.transform.SetParent(gameObject.transform);
        // canvas_obj.transform.localPosition = new Vector3(0, 0, 0);

        canvas_obj.AddComponent<Canvas>();
        canvas_comp = canvas_obj.GetComponent<Canvas>();
        canvas_comp.transform.localPosition = new Vector3(0, object_height * 1.1f + 1, 0);

        canvas_comp.renderMode = RenderMode.WorldSpace;
        canvas_comp.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        canvas_obj.AddComponent<CanvasScaler>();
        canvas_obj.AddComponent<GraphicRaycaster>();

        healthbar_obj = Instantiate(healthBarSlider_prefab, new Vector3(0, 0, 0), Quaternion.identity);
        // healthbar_obj = new GameObject();
        healthbar_obj.transform.SetParent(canvas_comp.transform, true);
        healthbar_obj.name = "HealthbarObject";
        healthbar_obj.transform.localPosition = new Vector3(0, 0, 0);
        // Slider healthbar_comp;
        // healthbar_comp = healthbar_obj.AddComponent<Slider>();
        // // Image healthbar_comp = healthbar_obj.AddComponent<Image>();
        // healthbar_obj.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        healthbar_comp = healthbar_obj.GetComponent<Slider>();
        healthbar_comp.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar_comp.value = health;
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
