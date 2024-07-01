using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCMerchantRnd : MonoBehaviour
{
    public ItemSO[] Items => items;
    [SerializeField] private ItemSO[] items;
    [SerializeField] private MerchantDisplayer merchantDisplayer;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            merchantDisplayer.SetMerchantItems(GenerateRandomItems());
        }
    }

    private ItemSO[] GenerateRandomItems()
    {
        List<ItemSO> tmpUtems = items.ToList();

        ItemSO[] rndItems = new ItemSO[Random.Range(1, items.Length)];

        for (int i = 0; i < rndItems.Length; i++)
        {
            int ind = Random.Range(0, tmpUtems.Count);
            rndItems[i] = tmpUtems[ind];
            tmpUtems.RemoveAt(ind);
        }

        return rndItems;
    }
}