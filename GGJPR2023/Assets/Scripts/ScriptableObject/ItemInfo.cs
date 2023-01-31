using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "General/Item Info")]
public class ItemInfo : ScriptableObject
{
    public enum ItemType
    {
        Consumable,
        Plantable,
        Resource,
        Throwable
    }

    [field:SerializeField] public Sprite itemSprite { get; private set;}
    [field:SerializeField] public ItemType itemType { get; private set;}
    [field:SerializeField] public string itemName { get; private set;} = "Item";
    [TextArea(5, 10)]
    [SerializeField] public string description = "Description of item.";
}
