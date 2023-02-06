using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static WaveInformation;

public class WaveManager : MonoBehaviour
{
    // Delegates
    public delegate void OnWavesFinished();
    public delegate void OnWaveCooldown();
    public delegate void OnWaveStarted();
    public static event OnWavesFinished onWavesFinished;
    public static event OnWaveCooldown onWaveCooldown;
    public static event OnWaveStarted onWaveStarted;

    [SerializeField] WaveInformation waveInformation;

    [SerializeField] public List<RoundInfo> roundQueue = new List<RoundInfo>();

    [SerializeField] int currentEnemyAmount;
    [SerializeField] int waveNum = 0;
    [SerializeField] int currentEnemies = 0;
    [SerializeField] GameObject[] spawners;
    [SerializeField] CountDownTimer waveTimer;
    [SerializeField] CountDownTimer intervalTimer;

    [SerializeField] TextMeshProUGUI timeLeftText;

    private void OnEnable()
    {
        //PlayerController.onPlayerDead += StopWaveManager;
        //GameManager.onGameOver += EndWaveManager;
        //GameManager.onSkipWave += SkipWaveCooldown;
    }
    private void OnDisable()
    {
        //Player.onPlayerDead -= StopWaveManager;
        //GameManager.onGameOver -= EndWaveManager;
        //GameManager.onSkipWave -= SkipWaveCooldown;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        Invoke("StartWaveSystem", 2);
    }

    private void FixedUpdate()
    {
        timeLeftText.SetText("Time until next wave: " + Mathf.Round(waveTimer.GetTimeLeft()));
    }

    void StartWaveSystem()
    {
        if (waveInformation.waves.Count > 0)
        {
            SpawnWave();
        }
    }

    // Resets the rounds and spawns next round
    public void SpawnWave()
    {
        if (waveNum < waveInformation.waves.Count)
        {
            Debug.Log("Wave Started");
            roundQueue = new List<RoundInfo>(waveInformation.waves[waveNum].rounds);
            SpawnRound();
            onWaveStarted?.Invoke();
        } 
        else
        {
            EndWaveManager();
        }
    }

    public void EndWave()
    {
        waveTimer.StartTimer(waveInformation.waves[waveNum].waveCooldown);
        onWaveCooldown?.Invoke();
        Debug.Log("Started wave cooldown");
        waveNum++;
    }

    // Starts the first round
    public void SpawnRound()
    {
        if (roundQueue.Count > 0)
        {
            currentEnemies = roundQueue[0].enemyAmount;
            SpawnEnemies();
            Debug.Log("Round Started");
        } else
        {
            EndWave();
        }
    }
    
    // Spawns enemies until the waves and rounds end
    public void SpawnEnemies()
    {
        GameObject currentEnemy = roundQueue[0].enemyToSpawn;
        int randomSpawn = UnityEngine.Random.Range(0, spawners.Length);

        Instantiate(currentEnemy, spawners[randomSpawn].transform.position, currentEnemy.transform.rotation);
        currentEnemies--;
        if (currentEnemies <= 0)
        {
            roundQueue.RemoveAt(0);
            SpawnRound();
        } else
        {
            intervalTimer.StartTimer(roundQueue[0].spawnInterval);
        }
    }

    //Stops the waves
    public void StopWaveManager()
    {
        Debug.Log("Waves are stopped");
        intervalTimer.SetPaused(true);
        waveTimer.SetPaused(true);
    }

    public void EndWaveManager()
    {
        Debug.Log("Waves are finished, waiting for enemies to die");
        StopWaveManager();
        onWavesFinished?.Invoke();
    }

    public void SkipWaveCooldown()
    {
        waveTimer.Stop();
        SpawnWave();
    }
}
