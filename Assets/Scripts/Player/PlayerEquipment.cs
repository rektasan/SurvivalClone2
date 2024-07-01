using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private EquipSlotDisplay[] equipSlots;
    [SerializeField] private GameObject[] equipObjects;
    private PlayerInventory inventory;
    private PlayerAttack playerAttack;

    private ItemSO[] equipedItems;

    private int curSelected = -1;

    void Start()
    {
        inventory = GetComponent<PlayerInventory>();
        playerAttack = GetComponent<PlayerAttack>();

        for (int i = 0; i < equipObjects.Length; i++)
        {
            equipObjects[i].SetActive(false);
        }

        equipedItems = new ItemSO[equipSlots.Length];
        for (int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i].Setup(this, i);
            equipSlots[i].Equip(null);
        }
        curSelected = -1;
    }

    public void SelectSlot(int ind)
    {
        if (curSelected != -1)
            equipSlots[curSelected].Deselect();

        if (curSelected == ind)
        {
            if(RemoveEquipment(ind))
            {
                playerAttack.SetDamageToDef();
                playerAttack.SetWeaponAnimInd(0);

                equipSlots[ind].Equip(null);
                equipObjects[ind].SetActive(false);
                equipedItems[ind] = null;
            }
            curSelected = -1;
        }
        else
        {
            curSelected = ind;
        }
    }

    private bool RemoveEquipment(int ind)
    {
        if (equipedItems[ind] == null)
            return false;

        int cellInd = inventory.GetSuitableCell(equipedItems[ind]);

        if (cellInd < 0)
            return false;

        for (int j = 0; j < equipObjects.Length; j++)
        {
            if (equipObjects[j].name == equipedItems[ind].EquippedName)
            {
                equipObjects[j].SetActive(false);
                break;
            }
        }

        inventory.AddItem(equipedItems[ind], cellInd);
        inventory.Group();
        inventory.Sort();

        return true;
    }

    public void EquipItem(ItemSO item, int fromCell)
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].EquipType == item.ItemType)
            {
                RemoveEquipment(i);

                equipSlots[i].Equip(item);
                equipedItems[i] = item;

                playerAttack.SetNewDamage(equipedItems[i].ItemModifier);
                playerAttack.SetWeaponAnimInd(1);

                for (int j = 0; j < equipObjects.Length; j++)
                {
                    if (equipObjects[j].name == equipedItems[i].EquippedName)
                    {
                        equipObjects[j].SetActive(true);
                        break;
                    }
                }

                inventory.Sort();
                return;
            }
        }
    }
}