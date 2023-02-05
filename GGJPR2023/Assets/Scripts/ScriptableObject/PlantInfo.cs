using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantInfo", menuName = "General/Plant Info")]
public class PlantInfo : ScriptableObject
{
    public int waterRequired = 5;
    public float cycleInterval = 300;
    [Tooltip("Add Meshes for each stage")]
    public GenericDictionary<Mesh, Material> stages;

    public ItemInfo harvestable;
    public List<ItemInfo> possibleDrops;
}

