using ExtEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;
[RequireComponent(typeof(CountDownTimer))]
[RequireComponent(typeof(BoxCollider))]
public class TreeScript : Interactable
{
    CountDownTimer timer;
    public SpriteRenderer treeSprite;
    int growthCycles;
    int currentCycle = 0;
    public PlantInfo plantInfo;

    public bool isPlanted { private set; get; } = false;
    public bool fullyGrown = false;

    private void OnEnable()
    {
        //Grow once and start the timer
        timer = GetComponent<CountDownTimer>();
        timer.SetTimerProcessMode(CountDownTimer.TimerProcessMode.TIMER_PROCESS_PHYSICS);
    }

    public void Harvest()
    {
        //This will drop an item and reset timers

    }

    public void Grow()
    {
        currentCycle++;
        if (currentCycle < growthCycles)
        {
            Debug.Log("Growing tree");
            UpdateSprite(currentCycle);
        } else
        {
            Debug.Log("Grew, lol dropping item");
            DropItem();
        }
        timer.StartTimer(plantInfo.cycleInfo.cycleInterval);
    }

    public void UpdateSprite(int num)
    {
        if (num < plantInfo.cycleInfo.plantStageSprites.Count)
        {
            treeSprite.sprite = plantInfo.cycleInfo.plantStageSprites[currentCycle];
        } else
        {
            Debug.Log("Out of bounds");
        }
    }

    public void Plant()
    {
        ItemInfo actorItem = activeActor.selectedItem;
        plantInfo = (PlantInfo)actorItem.externalReference;
        activeActor.UseItem(actorItem);
        // Use plis
        Debug.Log("Planting go brr");

        growthCycles = plantInfo.cycleInfo.plantStageSprites.Count;
        timer.StartTimer(plantInfo.cycleInfo.cycleInterval);
        currentCycle = 0;

        UpdateSprite(currentCycle);
        isPlanted = true;
    }

    public void DropItem()
    {
        //TODO
    }
}
