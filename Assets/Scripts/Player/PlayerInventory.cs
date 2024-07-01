using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private AudioClip pickSound;

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private int slotsCount;

    private PlayerStats stats;

    public ItemSO[] Items => itemsData;
    private ItemSO[] itemsData;
    private int[] itemsCount;
    private InvSlotDisplay[] itemDisplayers;

    private int curSlot;

    private PlayerEquipment playerEquipment;

    void Start()
    {
        playerEquipment = GetComponent<PlayerEquipment>();
        stats = GetComponent<PlayerStats>();

        itemsData = new ItemSO[slotsCount];
        itemsCount = new int[slotsCount];
        itemDisplayers = new InvSlotDisplay[slotsCount];

        for (int i = 0; i < slotsCount; i++)
        {
            InvSlotDisplay invSlotDisplay = Instantiate(slotPrefab, contentParent).GetComponent<InvSlotDisplay>();
            invSlotDisplay.SetItem(null);
            invSlotDisplay.Setup(this, i);
            itemDisplayers[i] = invSlotDisplay;
        }

        curSlot = -1;
    }

    public void SelectSlot(int ind)
    {
        if (itemsData[ind] == null)
        {
            itemDisplayers[ind].Deselect();
            return;
        }

        if (curSlot != -1)
            itemDisplayers[curSlot].Deselect();

        if (curSlot == ind)
        {
            UseItem();
            curSlot = -1;
        }
        else
        {
            if(curSlot >= 0)
            {
                ChangeItems(ind);
            }
            else
            {
                curSlot = ind;
            }   
        }
    }

    private void UseItem()
    {
        if (itemsData[curSlot].ItemType == ItemType.Backpack || itemsData[curSlot].ItemType == ItemType.Tool)
        {
            ItemSO temp = itemsData[curSlot];

            itemsData[curSlot] = null;
            itemsCount[curSlot] = 0;
            itemDisplayers[curSlot].SetItem(null);

            playerEquipment.EquipItem(temp, curSlot);
        }
        else if(itemsData[curSlot].ItemType == ItemType.Food && !stats.HasFullHunger)
        {
            stats.GetHungry(-itemsData[curSlot].ItemModifier);
            ChangeItemCount(itemsData[curSlot], 1);
            Group();
            Sort();
        }
        else if (itemsData[curSlot].ItemType == ItemType.Medical && !stats.HasFullHP)
        {
            stats.TakeDamage(-itemsData[curSlot].ItemModifier);
            ChangeItemCount(itemsData[curSlot], 1);
            Group();
            Sort();
        }
    }

    private void ChangeItems(int ind)
    {
        ItemSO tempItem = itemsData[curSlot];
        int tempCount = itemsCount[curSlot];

        itemsData[curSlot] = itemsData[ind];
        itemsCount[curSlot] = itemsCount[ind];

        itemDisplayers[curSlot].SetItem(itemsData[ind]);
        itemDisplayers[curSlot].UpdateItemCount(itemsCount[ind]);

        itemsData[ind] = tempItem;
        itemsCount[ind] = tempCount;

        itemDisplayers[ind].SetItem(tempItem);
        itemDisplayers[ind].UpdateItemCount(tempCount);

        itemDisplayers[ind].Deselect();
        curSlot = -1;

        Group();
        Sort();
    }

    public int GetItemCount(ItemSO item)
    {
        int count = 0;
        for (int i = 0; i < itemsData.Length; i++)
        {
            if (itemsData[i] == item)
            {
                count += itemsCount[i];
            }
        }
        return count;
    }

    public void ChangeItemCount(ItemSO item, int newCount)
    {
        for (int i = 0; i < itemsData.Length; i++)
        {
            if (newCount == 0)
                return;

            if (itemsData[i] == item)
            {
                if (itemsCount[i] < newCount)
                {
                    newCount -= itemsCount[i];
                    itemsCount[i] = 0;
                }
                else
                {
                    itemsCount[i] -= newCount;
                    newCount = 0;
                }

                itemDisplayers[i].UpdateItemCount(itemsCount[i]);

                if (itemsCount[i] == 0)
                {
                    itemDisplayers[i].SetItem(null);
                    itemsData[i] = null;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Item item))
        {
            int cell = GetSuitableCell(item.ItemData);
            if (cell >= 0)
            {
                AudioManager.Instance.PlaySound(pickSound, true);
                AddItem(item.ItemData, cell);
                Destroy(item.gameObject);
            }
        }
    }

    public int GetSuitableCell(ItemSO item)
    {
        for (int i = 0; i < itemsData.Length; i++)
        {
            if (itemsData[i] == item && itemsCount[i] < itemsData[i].ItemsInStack)
            {
                return i;
            }
            else if (!itemsData[i])
            {
                return i;
            }
            else if (i == itemsData.Length - 1)
            {
                return -1;
            }
        }
        return -1;
    }

    public void AddItem(ItemSO item, int cellInd)
    {
        if (itemsData[cellInd] == item && itemsCount[cellInd] < itemsData[cellInd].ItemsInStack)
        {
            itemsCount[cellInd]++;
            itemDisplayers[cellInd].UpdateItemCount(itemsCount[cellInd]);
        }
        else if (!itemsData[cellInd])
        {
            itemsData[cellInd] = item;
            itemsCount[cellInd] = 1;
            itemDisplayers[cellInd].SetItem(item);
            itemDisplayers[cellInd].UpdateItemCount(1);
        }
    }

    public void Sort()
    {
        for (int i = 0; i < itemsData.Length; i++)
        {
            if (itemsData[i] != null)
                continue;

            for (int j = i + 1; j < itemsData.Length; j++)
            {
                if (itemsData[j] != null)
                {
                    itemsData[i] = itemsData[j];
                    itemsCount[i] = itemsCount[j];

                    itemDisplayers[i].SetItem(itemsData[i]);
                    itemDisplayers[i].UpdateItemCount(itemsCount[i]);

                    itemsData[j] = null;
                    itemsCount[j] = 0;

                    itemDisplayers[j].SetItem(null);

                    break;
                }
            }
        }

        if (curSlot >= 0 && itemsData[curSlot] == null)
        {
            itemDisplayers[curSlot].Deselect();
            curSlot = -1;
        }
    }

    public void Group()
    {
        for (int i = 0; i < itemsData.Length; i++)
        {
            if (itemsData[i] == null)
                continue;

            if (itemsCount[i] == itemsData[i].ItemsInStack)
                continue;

            for (int j = i + 1; j < itemsData.Length; j++)
            {
                if (itemsData[j] != null && itemsData[j] == itemsData[i])
                {
                    if (itemsCount[i] + itemsCount[j] > itemsData[i].ItemsInStack)
                    {
                        itemsCount[j] -= (itemsData[i].ItemsInStack - itemsCount[i]);
                        itemDisplayers[j].UpdateItemCount(itemsCount[j]);

                        itemsCount[i] = itemsData[i].ItemsInStack;
                        itemDisplayers[i].UpdateItemCount(itemsCount[i]);
                    }
                    else
                    {
                        itemsCount[i] += itemsCount[j];
                        itemDisplayers[i].UpdateItemCount(itemsCount[i]);

                        itemsData[j] = null;
                        itemsCount[j] = 0;
                        itemDisplayers[j].SetItem(null);
                    }

                    if (itemsData[i].ItemsInStack == itemsCount[i])
                        break;
                }
            }
        }
    }

    private void Update()
    {
        if (PlayerUI.Instance.IsInInventory && Input.GetMouseButtonDown(1) && curSlot >= 0 && itemsData[curSlot] != null)
        {
            Instantiate(itemsData[curSlot].ItemObject, transform.position + transform.forward * 3 + transform.up, Quaternion.identity);

            if (itemsCount[curSlot] > 1)
            {
                itemsCount[curSlot]--;
                itemDisplayers[curSlot].UpdateItemCount(itemsCount[curSlot]);

                Group();
                Sort();
            }
            else
            {
                itemsData[curSlot] = null;
                itemsCount[curSlot] = 0;
                itemDisplayers[curSlot].SetItem(null);

                itemDisplayers[curSlot].Deselect();
                curSlot = -1;

                Sort();
            }
        }
    }
}
