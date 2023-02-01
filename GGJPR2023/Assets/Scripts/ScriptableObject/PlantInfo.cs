using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantInfo", menuName = "General/Plant Info")]
public class PlantInfo : ScriptableObject
{
    public string plantName;
    public PlantLifeCycle cycleInfo;
    public ItemInfo harvestable;
}

[System.Serializable]
public class PlantLifeCycle
{
    [Tooltip("Add sprites for each stage")]
    public List<Sprite> plantStageSprites;
    public int waterRequired = 1;
    public float cycleInterval = 300;
    public bool canHarvest = false;
}
