using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class DamageableObject : MonoBehaviour
{
    private float time = 0.0f;
    private float last_hit = 0.0f;
    private int health;
    public int maxHealth = 100;
    public int pointsForKill = 100;
    protected Slider healthbar_comp;
    public GameObject healthbar_obj = null;
    public GameObject mainCamera_obj = null;
    private Camera mainCamera_comp;
    private GameObject canvas_obj;
    private RectTransform canvas_rect_transform;
    private Renderer obj_renderer;

    // Dictionary < string, string > phonebook = new Dictionary < string, string > ();
    private Dictionary<PlayerBehaviour, int> damagers = new Dictionary<PlayerBehaviour, int>(10);
    private int totalDamagers = 0;

    public void GetDamaged(int damage_value, PlayerBehaviour damager = null) {
        if (health > 0) {
            health -= damage_value;
            // Debug.Log(name + " gets damaged by " + damage_value.ToString() + " (" + health.ToString() + " health left)");

            if (damager) {
                totalDamagers += 1;
                if (damagers.ContainsKey(damager))
                    damagers[damager] += 1;
                else
                    damagers[damager]  = 1;
            }
        }

        if (health <= 0)
            Die();

        if (healthbar_comp)
            healthbar_comp.value = health;
    }

    void PlayDeathAnimation() {

    }

    void Die() {
        Debug.Log(name + " is dead!");
        if (healthbar_comp)
            Destroy(healthbar_comp);

        if (healthbar_obj)
            Destroy(healthbar_obj);

        foreach (KeyValuePair<PlayerBehaviour, int> entry in damagers)
        {
            // entry.Value or entry.Key
            entry.Key.AddKillPoints((int)(pointsForKill * entry.Value * 100.0 / totalDamagers));
        }

        PlayDeathAnimation();
            
        Destroy(gameObject);
        Destroy(this);
    }

    public void ResetHealth() {
        if (maxHealth <= 0)
            maxHealth = 1;

        health = maxHealth;

        Debug.Log(this.name + " max Health: " + maxHealth.ToString());
        Debug.Log(this.name + "     health: " + health.ToString());

        if (healthbar_comp) {
            healthbar_comp.maxValue = maxHealth;
            healthbar_comp.value    = health;
        }
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

    protected void SetupHealthbar() {
        if (!healthbar_obj) {
            GameObject healthBarSlider_prefab = Resources.Load("Prefabs/UI/HealthBarSlider") as GameObject;

            float object_height = MaxHeight(gameObject);
            Debug.Log("object_height: " + object_height.ToString());

            if (!mainCamera_obj)
                mainCamera_obj = GameObject.FindGameObjectWithTag("MainCamera"); //.GetComponent<Camera>();

            if (mainCamera_obj)
                mainCamera_comp = mainCamera_obj.GetComponent<Camera>();

            if (mainCamera_obj && mainCamera_obj.transform.childCount > 0) {
                canvas_obj = mainCamera_obj.transform.GetChild(0).gameObject;
                // GameObject canvas_obj = null;
                Canvas canvas_comp = canvas_obj.GetComponent<Canvas>();
                canvas_rect_transform = canvas_obj.GetComponent<RectTransform>();

                healthbar_obj = Instantiate(healthBarSlider_prefab, new Vector3(0, 0, 0), Quaternion.identity);
                // // healthbar_obj = new GameObject();
                healthbar_obj.transform.SetParent(canvas_comp.transform, true);
                healthbar_obj.name = "HealthbarObject";

                healthbar_obj.transform.localPosition = new Vector3(0, 0, 0);
            }
        }

        healthbar_comp = healthbar_obj.GetComponent<Slider>();
    }

    void Start()
    {
        // Canvas canvas_comp = null;
        // Text text;
        // RectTransform rectTransform;
       
        SetupHealthbar();
        ResetHealth();
        obj_renderer = gameObject.GetComponent<Renderer>();
    }

    Vector3 WorldToScreenPointAdjusted(Vector3 pos) {
        if (mainCamera_comp && canvas_rect_transform) {
            Vector3 adjustedPosition = mainCamera_comp.WorldToScreenPoint(pos);
            adjustedPosition.x *= canvas_rect_transform.rect.width  / (float)mainCamera_comp.pixelWidth;
            adjustedPosition.y *= canvas_rect_transform.rect.height / (float)mainCamera_comp.pixelHeight;
            Vector3 canvas_rect_transform3 = new Vector3(canvas_rect_transform.sizeDelta.x, canvas_rect_transform.sizeDelta.y, 0);
            // return adjustedPosition - canvas_rect_transform.sizeDelta / 2f;
            return adjustedPosition - canvas_rect_transform3 /  2f;
        }
        return new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 screenPos = mainCamera_comp.WorldToScreenPoint(gameObject.transform.position);
        
        // Vector3 screenMin = mainCamera_comp.WorldToScreenPoint(min);
        // Vector3 screenMax = mainCamera_comp.WorldToScreenPoint(max);

        if (obj_renderer) {
            Vector3 min = obj_renderer.bounds.min;
            Vector3 max = obj_renderer.bounds.max;

            float height = max.y - min.y;

            min.y = max.y + height * 0.25f;
            max.y = min.y;

            Vector3 middle = (min + max) / 2;

            Vector3 adjustedPosition = WorldToScreenPointAdjusted(middle);

            if (healthbar_obj) {
                if (adjustedPosition.z < 0)
                    healthbar_obj.SetActive(false);
                else {
                    healthbar_obj.transform.localPosition = adjustedPosition;
                    healthbar_obj.SetActive(true);
                }
            }
        }
        else {
            obj_renderer = gameObject.GetComponent<Renderer>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // if (time - last_hit > 1f) {
        Debug.Log("Collision with " + collision.gameObject.name);
        DamagingObject damagingObject = collision.gameObject.GetComponent<DamagingObject>();

        if (damagingObject) {
            GetDamaged(damagingObject.damage, damagingObject.owner);
            last_hit = time;
        }
        // }
    }

    void FixedUpdate() {
        time += Time.fixedDeltaTime;
    }
}
