using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpManager : MonoBehaviour
{
    float health = 100;
    public Slider slider;
    public GameObject player;

    void Start()
    {
        slider.maxValue = 100;
        slider.value = health;
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "EnemyPlants")
        {
            health -= 1f;
            slider.value = health;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
