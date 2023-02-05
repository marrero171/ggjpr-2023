using UnityEngine;
using UnityEngine.UIElements;

public class TownStatusPanel : VisualElement
{

    PauseMenu pauseMenu;


    public new class UxmlTraits : VisualElement.UxmlTraits { }
    public new class UxmlFactory : UxmlFactory<TownStatusPanel, UxmlTraits> { }

}