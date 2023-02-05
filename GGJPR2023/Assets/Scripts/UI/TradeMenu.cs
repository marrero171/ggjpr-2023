using UnityEngine;
using UnityEngine.UIElements;

public class TradeMenu : VisualElement
{

    PauseMenu pauseMenu;


    public new class UxmlTraits : VisualElement.UxmlTraits { }
    public new class UxmlFactory : UxmlFactory<TradeMenu, UxmlTraits> { }

}