using ExtEvents;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] int waterLevel = 5;
    int waterThreshold = 5;
    bool healthy = true;

    public PlantInfo plantInfo;

    int WaterLevel
    {
        get { return waterLevel; }
        set
        {
            waterLevel = Mathf.Clamp(value, -waterThreshold, waterThreshold);

            if (waterLevel > 0)
            {
                healthy = true;
                timer.timeMultiplier = 1;
            }
            else
            {
                healthy = false;
                timer.timeMultiplier = 0.75f;
            }

            if (waterLevel <= -waterThreshold)
            {
                KillPlant();
            }
        }
    }

    public bool isPlanted = false;
    public bool fullyGrown = false;

    private void OnEnable()
    {
        //Grow once and start the timer
        timer = GetComponent<CountDownTimer>();
        timer.SetTimerProcessMode(CountDownTimer.TimerProcessMode.TIMER_PROCESS_PHYSICS);
    }

    void LateUpdate()
    {
        treeSprite.transform.LookAt(Camera.main.transform);

    }

    public void Harvest()
    {
        //This will drop an item and reset timers

    }

    public void TryPlanting()
    {
        if (isPlanted) return;
        Grow();
    }

    public void Grow()
    {
        WaterLevel--;
        if (!isPlanted)
            return;

        timer.StartTimer(plantInfo.cycleInfo.cycleInterval);
        if (currentCycle < growthCycles - 1)
        {
            Debug.Log("Growing tree");
            currentCycle++;
            UpdateSprite(currentCycle);
            Debug.Log(currentCycle);
        }
        else
        {
            Debug.Log("Grew, lol dropping item");
            DropItem();
        }
    }

    public void UpdateSprite(int num = -1)
    {
        if (num < 0)
        {
            treeSprite.sprite = null;
            return;
        }
        else if (num < plantInfo.cycleInfo.plantStageSprites.Count)
        {
            treeSprite.sprite = plantInfo.cycleInfo.plantStageSprites[currentCycle];
        }
    }

    public void Plant()
    {
        if (isPlanted)
            return;

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

    public void KillPlant()
    {
        UpdateSprite(-1);
        plantInfo = null;
        isPlanted = false;
        timer.Stop();
        Debug.Log("Plant died");
    }

    public void Water()
    {
        if (!isPlanted)
            return;

        ItemInfo actorItem = activeActor.selectedItem;
        WaterLevel += actorItem.effectiveAmount;
        activeActor.UseItem(actorItem);
    }

    public void DropItem()
    {
        //TODO
    }
}
