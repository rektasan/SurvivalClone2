using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPCTriggers : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (PlayerUI.Instance.IsInInventory || MerchantDisplayer.Instance.IsWithMerchant
            || DialogDisplayer.Instance.IsInDialog || !Input.GetKey(KeyCode.E))
            return;

        if (other.TryGetComponent(out NPCMerchant merchant))
        {
            MerchantDisplayer.Instance.SetMerchantItems(merchant.Items);
        }
        else if (other.TryGetComponent(out NPCDialog npc))
        {
            DialogDisplayer.Instance.StartDialog(npc);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (DialogDisplayer.Instance.IsInDialog)
            DialogDisplayer.Instance.CloseDialog();

        if (MerchantDisplayer.Instance.IsWithMerchant)
            MerchantDisplayer.Instance.CloseMerchant();
    }
}