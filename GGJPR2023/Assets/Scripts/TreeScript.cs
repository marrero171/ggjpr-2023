using ExtEvents;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;
using Utils;
[RequireComponent(typeof(CountDownTimer))]
[RequireComponent(typeof(BoxCollider))]
public class TreeScript : Interactable
{
    CountDownTimer timer;
    //public SpriteRenderer treeSprite;
    int growthCycles;
    int currentCycle = 0;
    [SerializeField] int waterLevel = 5;
    int waterThreshold = 5;
    bool healthy = true;

    public PlantInfo plantInfo;

    public Mesh soilMesh;
    public Material soilMaterial;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public GameObject treeObject;

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

        soilMesh = meshFilter.mesh;
        soilMaterial = meshRenderer.material;

        timer.SetTimerProcessMode(CountDownTimer.TimerProcessMode.TIMER_PROCESS_PHYSICS);
    }

    private void FixedUpdate()
    {
        if (!isPlanted || fullyGrown) return;

        float newSize = Mathf.InverseLerp(0, timer.GetWaitTime() * growthCycles, timer.GetTimeLeft());
        newSize = Mathf.Abs(1 - newSize);
        treeObject.transform.localScale = Vector3.one * newSize;
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

        timer.StartTimer(plantInfo.cycleInterval);
        if (currentCycle < growthCycles - 1)
        {
            Debug.Log("Growing tree");
            currentCycle++;
            print(currentCycle - growthCycles);
            UpdateStage(currentCycle);
            Debug.Log(currentCycle);
        }
        else
        {
            fullyGrown = true;
            Debug.Log("Grew, lol dropping item");
            DropItem();
        }
    }

    public void UpdateStage(int num = -1)
    {
        Debug.Log(num);
        if (num < 0)
        {
            meshFilter.mesh = soilMesh;
            meshRenderer.material = soilMaterial;
            return;
        }
        else if (num < plantInfo.stages.Count)
        {
            meshFilter.mesh = plantInfo.stages.ElementAt(num).Key;
            meshRenderer.material = plantInfo.stages.ElementAt(num).Value;
        }
    }

    public void Plant()
    {
        if (isPlanted) return;

        ItemInfo actorItem = activeActor.selectedItem;
        plantInfo = (PlantInfo)actorItem.externalReference;

        if (plantInfo == null) return;
        activeActor.UseItem(actorItem);
        
        // Use plis
        // Debug.Log("Planting go brr");

        growthCycles = plantInfo.stages.Count;
        timer.StartTimer(plantInfo.cycleInterval);
        currentCycle = 0;

        WaterLevel = 0;
        waterThreshold = plantInfo.waterRequired;

        treeObject.transform.localScale = Vector3.zero;
        UpdateStage(currentCycle);
        isPlanted = true;
    }

    public void KillPlant()
    {
        UpdateStage(-1);
        plantInfo = null;
        isPlanted = false;
        timer.Stop();
        treeObject.transform.localScale = Vector3.one;
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
        if (!plantInfo.harvestable) return;
        DroppedItem newItem = PoolingSystem.instance.GetObject(ReferenceMaster.instance.DroppedItem.gameObject).GetComponent<DroppedItem>();
        newItem.transform.position = transform.position + (Vector3.up * 7.5f);
        newItem.gameObject.SetActive(true);
        newItem.item = plantInfo.harvestable;
    }
}
