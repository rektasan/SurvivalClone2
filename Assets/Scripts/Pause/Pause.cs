using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool IsPaused => pauseCanvas.enabled;
    [SerializeField] private Canvas pauseCanvas;

    public static Pause Instance { get; private set; }

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

    private void Start()
    {
        PauseGame(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(!IsPaused);
        }
    }

    public void PauseGame(bool isPaused)
    {
        pauseCanvas.enabled = isPaused;

        if(isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else
        {
            if (!PlayerUI.Instance.IsInInventory && !DialogDisplayer.Instance.IsInDialog
                && !MerchantDisplayer.Instance.IsWithMerchant)
                Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1f;
        }
    }

    public void SetSoundStatus(bool canPlaySounds)
    {
        AudioManager.Instance.BlockSounds(!canPlaySounds);
    }
}