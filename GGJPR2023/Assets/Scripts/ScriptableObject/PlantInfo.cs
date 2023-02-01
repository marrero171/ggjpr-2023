using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantInfo", menuName = "General/Plant Info")]
public class PlantInfo : ScriptableObject
{
    public string plantName;
    public ItemInfo harvestable;
}

[System.Serializable]
public class PlantLifeCycle
{
    Sprite plantSprite;
    int waterRequired = 1;
    float cycleInterval = 300;
    bool canHarvest = false;
}
