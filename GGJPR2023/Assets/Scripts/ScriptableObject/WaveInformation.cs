using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveInformation", menuName = "Game/Wave Info")]
public class WaveInformation : ScriptableObject
{
    [Serializable]
    public struct RoundInfo
    {
        [SerializeField] public GameObject enemyToSpawn;
        [SerializeField] public int enemyAmount;
        [SerializeField] public float spawnInterval;
    }

    [Serializable]
    public struct WaveInfo
    {
        [SerializeField] public RoundInfo[] rounds;
        [SerializeField] public float waveCooldown;
    }

    [SerializeField] public List<WaveInfo> waves = new List<WaveInfo>();
}

