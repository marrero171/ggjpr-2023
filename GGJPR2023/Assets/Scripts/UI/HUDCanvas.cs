using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class HUDCanvas : MonoBehaviour
{

    public static HUDCanvas instance;
    bool isPaused = false;
    public PlayerController player;
    public HomeArea village;
    [Header("HUD")]
    public Slider healthBar;
    public TMP_Text healthText, itemLabel, itemCount;
    public Image itemIcon;
    [Header("UI Stuffs")]
    public GameObject PausePanel;
    public GameObject ConfirmExitPanel;
    [Header("Culture Inspector")]
    public Slider RudeKind;
    public Slider RushSteady, LazyEager, Hunger, Thirst, Emotion;

    Vector3 AverageNeeds, AverageBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        if (HUDCanvas.instance != null) Destroy(gameObject);
        HUDCanvas.instance = this;
        if (player == null) player = GameObject.FindObjectOfType<PlayerController>();
        UpdateHealth();
        UpdateIcon();
    }

    public void UpdateHealth()
    {
        if (player == null) return;
        healthBar.value = player.Health;
        healthText.text = player.Health + "/" + player.MaxHealth;
    }

    public void UpdateIcon()
    {
        itemIcon.sprite = (player.selectedItem != null ? player.selectedItem.itemSprite : null);
        itemIcon.color = (player.selectedItem != null ? Color.white : Color.clear);
        itemLabel.text = player.selectedItem != null ? player.selectedItem.itemName : string.Empty;
        if (player.selectedItem != null && player.Inventory.ContainsKey(player.selectedItem))
            itemCount.text = player.Inventory[player.selectedItem].ToString();
        else itemCount.text = string.Empty;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.001f : 1;
        if (isPaused && village != null)
        {
            AverageBehaviour = village.GetVillagerBehaviourAverage();
            AverageNeeds = village.GetActorNeedsAverage();
            RudeKind.value = AverageBehaviour.x;
            RushSteady.value = AverageBehaviour.y;
            LazyEager.value = -AverageBehaviour.z;
            Hunger.value = AverageNeeds.x;
            Thirst.value = AverageNeeds.y;
            Emotion.value = AverageNeeds.z;
        }
        PausePanel.SetActive(isPaused);
        ConfirmExitPanel.SetActive(false);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Title");
    }
}
