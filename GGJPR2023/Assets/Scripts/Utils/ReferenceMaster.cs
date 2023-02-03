using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceMaster : MonoBehaviour
{
    public static ReferenceMaster instance;
    [Header("Items to be referenced")]
    public DroppedItem DroppedItem;
    public AttackProjectile Projectile;
    public PlayerController player;

    void Start()
    {
        if (ReferenceMaster.instance != null) Destroy(gameObject);
        ReferenceMaster.instance = this;
        if (Utils.PoolingSystem.instance == null) new Utils.PoolingSystem();
    }
}
