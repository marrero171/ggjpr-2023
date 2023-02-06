using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pixelplacement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public LayerMask MusicChangeLayers;
    public float MusicChangeRadius = 30;
    public AudioSource TownLayer, FightingLayer, FinghtingIntenseLayer, TransitionLayer;
    public AudioClip TownFight, OutsiedFight;
    PlayerController playerController;
    bool isFighting = false, intensified = false;

    AudioClip Battlefield { get { return playerController.isInTown ? TownFight : OutsiedFight; } }

    private void Start()
    {
        AudioManager.instance = this;
        playerController = GameObject.FindObjectOfType<PlayerController>();
        TownLayer.volume = 0;
        FightingLayer.volume = 0;
        FinghtingIntenseLayer.volume = 0;
        Tween.Volume(TownLayer, 1, 5, 1);
        StartCoroutine(updateBGMLayerCoroutine());
    }

    public void ToggleFighting(bool battle, bool intense = false)
    {
        Tween.Volume((battle ? TownLayer : FightingLayer), 0, 2, 0, Tween.EaseInOut);
        if (!isFighting) { TransitionLayer.Play(); FightingLayer.clip = Battlefield; FightingLayer.Play(); }

        if (isFighting != battle) isFighting = battle;
        // if (intensified != intense) 
        intensified = playerController.Health <= 5 && battle;

        Tween.Volume(FinghtingIntenseLayer, ((intensified && battle) ? 1 : 0), 1, 0, Tween.EaseInOut);
        // if (intensified) Tween.Volume(FinghtingIntenseLayer, (battle ? 1 : 0), 2, 0, Tween.EaseInOut);
        Tween.Volume((battle ? FightingLayer : TownLayer), 1, 2, 2, Tween.EaseInOut);
    }

    IEnumerator updateBGMLayerCoroutine()
    {
        while (true)
        {
            Collider[] hits = Physics.OverlapSphere(playerController.transform.position, MusicChangeRadius, MusicChangeLayers);
            ToggleFighting(hits.Length > 0);
            yield return new WaitForSeconds(1);
        }
    }



}