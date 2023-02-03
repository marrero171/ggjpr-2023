using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDAndMenu : MonoBehaviour
{
    public static HUDAndMenu instance;
    public PlayerController player;
    public UIDocument doc;

    ProgressBar healthBar;
    VisualElement itemIcon;
    Label itemLabel, itemCount;

    void Start()
    {
        HUDAndMenu.instance = this;
        healthBar = doc.rootVisualElement.Q<ProgressBar>("HealthBar");
        healthBar.highValue = player.MaxHealth;



        itemIcon = doc.rootVisualElement.Q<VisualElement>("ActiveItemIcon");
        itemLabel = doc.rootVisualElement.Q<Label>("ActiveItemLabel");
        itemCount = doc.rootVisualElement.Q<Label>("ActiveItemCount");
        UpdateHealth();

        UpdateIcon();
    }

    public void UpdateHealth() { healthBar.value = player.Health; healthBar.title = player.Health + "/" + player.MaxHealth; }

    public void UpdateIcon()
    {
        itemIcon.style.backgroundImage = new StyleBackground(player.selectedItem != null ? player.selectedItem.itemSprite : null);
        itemLabel.text = player.selectedItem != null ? player.selectedItem.itemName : string.Empty;
        //     itemCount.text = (player.selectedItem != null) ? player.Inventory[player.selectedItem].ToString() : string.Empty;
        if (player.selectedItem != null)
            itemCount.text = player.Inventory[player.selectedItem].ToString();
        else itemCount.text = string.Empty;
    }
}
