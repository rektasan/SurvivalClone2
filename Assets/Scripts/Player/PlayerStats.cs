using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public bool HasFullHP => curHp == maxHp;
    [SerializeField] private float maxHp;
    [SerializeField] private Image hpBar;

    public bool HasFullHunger => curHunger == maxHunger;
    [Space]
    [SerializeField] private float maxHunger;
    [SerializeField] private float hungerLostPerSec;
    [SerializeField] private Image hungerBar;

    [Space]
    [SerializeField] private int startMoney;
    [SerializeField] private TMP_Text moneyTmp;

    private float curHp;
    private float curHunger;

    public int CurMoney => curMoney;
    private int curMoney;

    void Start()
    {
        curHp = maxHp;
        hpBar.fillAmount = curHp / maxHp;

        curHunger = maxHunger;
        hungerBar.fillAmount = curHunger / maxHunger;

        curMoney = startMoney;
        moneyTmp.text = curMoney.ToString();
    }

    public void TakeDamage(float dmg)
    {
        curHp -= dmg;

        if(curHp > maxHp)
            curHp = maxHp;

        hpBar.fillAmount = curHp / maxHp;

        if(curHp <= 0)
        {
            Die();
        }
    }

    public void GetHungry(float hunger)
    {
        curHunger -= hunger;

        if (curHunger > maxHunger)
            curHunger = maxHunger;

        hungerBar.fillAmount = curHunger / maxHunger;

        if (curHunger <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddMoney(int amount)
    {
        curMoney += amount;
        moneyTmp.text = curMoney.ToString();
    }

    void Update()
    {
        GetHungry(hungerLostPerSec * Time.deltaTime);
    }
}
