using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : VisualElement
{

    public Button Resume, Exit;
    public ConfirmPanel confirmPanel;


    public new class UxmlTraits : VisualElement.UxmlTraits { }
    public new class UxmlFactory : UxmlFactory<PauseMenu, UxmlTraits> { }

}