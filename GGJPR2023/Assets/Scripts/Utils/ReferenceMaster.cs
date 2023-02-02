using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceMaster : MonoBehaviour
{
    public static ReferenceMaster instance;
    [Header("Items to be referenced")]
    public DroppedItem DroppedItem;
    public AttackProjectile Projectile;




    void Start()
    {
        if (ReferenceMaster.instance != null) Destroy(gameObject);
        ReferenceMaster.instance = this;
    }
}
