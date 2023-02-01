using ExtEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;
[RequireComponent(typeof(CountDownTimer))]
[RequireComponent(typeof(BoxCollider))]
public class Tree : Interactable
{
    CountDownTimer timer;
    public SpriteRenderer treeSprite;
    int growthCycles;
    public PlantInfo plantInfo;

    private void OnEnable()
    {
        //Grow once and start the timer
        timer = GetComponent<CountDownTimer>();
        growthCycles = plantInfo.cycleInfo.plantStageSprites.Count;
        timer.StartTimer(plantInfo.cycleInfo.cycleInterval);
        growthCycles--;
        treeSprite.sprite = plantInfo.cycleInfo.plantStageSprites[0];

    }

    public void Harvest()
    {
        //This will drop an item and reset timers

    }

    public void TestFunc()
    {
        Debug.Log("Grew, lol");
    }

    public void Plant(ItemInfo plantInfo)
    {
        // Use plis
        Debug.Log("Planting go brr");
    }
}
