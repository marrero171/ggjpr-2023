using System.Linq;
using UnityEngine;
using ExtEvents;
using System.Collections.Generic;

public class Actor : MonoBehaviour
{
    public int Health = 10, MaxHealth = 10;
    //TODO: Inventory
    public Dictionary<int, int> Inventory;
    private void OnEnable()
    {
        Health = MaxHealth;
    }

}