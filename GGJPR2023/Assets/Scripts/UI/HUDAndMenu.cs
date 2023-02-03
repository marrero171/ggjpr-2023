using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDAndMenu : MonoBehaviour
{
    public Actor player;
    public UIDocument doc;

    ProgressBar healthBar;
    VisualElement icon;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => ReferenceMaster.instance != null);
        healthBar = doc.rootVisualElement.Q<ProgressBar>("HealthBar");
        icon = doc.rootVisualElement.Q<ProgressBar>("ActiveItemIcon");
    }

    public void UpdateHealth()
    {
        healthBar.value = player.Health;
    }

    public void UpdateIcon()
    {
        icon.style.backgroundImage = new StyleBackground(player.selectedItem.itemSprite);
    }
}
