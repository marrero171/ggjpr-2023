using UnityEngine;
using UnityEngine.UIElements;

public class ConfirmPanel : VisualElement
{
    Label label;
    Button Confirm, Cancel;


    public new class UxmlTraits : VisualElement.UxmlTraits { }
    public new class UxmlFactory : UxmlFactory<ConfirmPanel, UxmlTraits> { }

}