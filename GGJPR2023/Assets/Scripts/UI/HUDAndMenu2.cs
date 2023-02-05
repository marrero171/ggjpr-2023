using UnityEngine;
using UnityEngine.UIElements;

public class HUDAndMenu2 : VisualElement
{
    public static HUDAndMenu2 instance;

    //Menus
    public PauseMenu pauseMenu;
    public TownStatusPanel townStatusPanel;
    public TradeMenu tradeMenu;

    //HUD
    public ProgressBar healthBar;
    public VisualElement itemIcon;
    public Label itemLabel, itemCount;

    bool pause = false;
    public PlayerController player;
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    public new class UxmlFactory : UxmlFactory<HUDAndMenu2, UxmlTraits> { }

    public void TogglePause()
    {
        if (pauseMenu == null) pauseMenu = this.Q<PauseMenu>("PauseMenu");
        pause = !pause;
        Time.timeScale = pause ? 0.001f : 1;
        pauseMenu.style.visibility = pause ? Visibility.Visible : Visibility.Hidden;
    }

    public HUDAndMenu2()
    {
        HUDAndMenu2.instance = this;
    }

    public void InitializeHUD()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        healthBar = this.Q<ProgressBar>("HealthBar");
        healthBar.highValue = player.MaxHealth;

        itemIcon = this.Q<VisualElement>("ActiveItemIcon");
        itemLabel = this.Q<Label>("ActiveItemLabel");
        itemCount = this.Q<Label>("ActiveItemCount");
        UpdateHealth();
        UpdateIcon();
    }

    public void UpdateHealth()
    {
        if (player == null) return;
        healthBar.value = player.Health;
        healthBar.title = player.Health + "/" + player.MaxHealth;
    }
    public void UpdateIcon()
    {
        itemIcon.style.backgroundImage = new StyleBackground(player.selectedItem != null ? player.selectedItem.itemSprite : null);
        itemLabel.text = player.selectedItem != null ? player.selectedItem.itemName : string.Empty;
        if (player.selectedItem != null && player.Inventory.ContainsKey(player.selectedItem))
            itemCount.text = player.Inventory[player.selectedItem].ToString();
        else itemCount.text = string.Empty;
    }


}