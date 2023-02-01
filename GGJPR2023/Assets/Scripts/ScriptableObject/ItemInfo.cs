using UnityEngine;
public enum ItemType
    {
        Consumable,
        Plantable,
        Resource,
        Throwable
    }
[CreateAssetMenu(fileName = "ItemInfo", menuName = "General/Item Info")]
public class ItemInfo : ScriptableObject
{
    

    [field:SerializeField] public Sprite itemSprite { get; private set;}
    [field:SerializeField] public ItemType itemType { get; private set;}
    [field:SerializeField] public string itemName { get; private set;} = "Item";
    [TextArea(5, 10)]
    [SerializeField] public string description = "Description of item.";

    public int effectiveAmount = 5;
    public ScriptableObject externalReference;
}
