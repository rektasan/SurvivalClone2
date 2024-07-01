using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerManager : MonoBehaviour
{
    public float maxHunger;
    public Image hpBar;
    private float curHunger;

    void Start()
    {
        curHunger = maxHunger;
        hpBar.fillAmount = curHunger / maxHunger;
    }

    void Update()
    {
        curHunger -= 0.05f;
        hpBar.fillAmount = curHunger / maxHunger;
    }
}
