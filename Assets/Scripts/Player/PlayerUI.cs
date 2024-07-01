using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject invGroup;
    [SerializeField] private GameObject craftGroup;

    public bool IsInInventory => invCanvas.enabled;
    [SerializeField] private Canvas invCanvas;

    public static PlayerUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        craftGroup.SetActive(false);
        invCanvas.enabled = false;
    }

    public void ChooseFirstCategory(bool isFirst)
    {
        invGroup.SetActive(isFirst);
    }

    public void ChooseSecondCategory(bool isSecond)
    {
        craftGroup.SetActive(isSecond);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !DialogDisplayer.Instance.IsInDialog
            && !MerchantDisplayer.Instance.IsWithMerchant)
        {
            invCanvas.enabled = !invCanvas.enabled;

            if (invCanvas.enabled)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
