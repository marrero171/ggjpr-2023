using UnityEngine;
using Pixelplacement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource TownLayer, FightingLayer, FinghtingIntenseLayer, TransitionLayer;
    public AudioClip TownFight, OutsiedFight;
    bool isFighting = false, intensified = false;

    AudioClip Battlefield { get { return ReferenceMaster.instance.player.isInTown ? TownFight : OutsiedFight; } }

    private void Start()
    {
        AudioManager.instance = this;
        TownLayer.volume = 0;
        FightingLayer.volume = 0;
        FinghtingIntenseLayer.volume = 0;
        Tween.Volume(TownLayer, 1, 5, 1);
    }

    public void ToggleFighting(bool battle, bool intense = false)
    {
        Tween.Volume((battle ? TownLayer : FightingLayer), 0, 2, 0, Tween.EaseInOut);
        if (!isFighting) { TransitionLayer.Play(); FightingLayer.clip = Battlefield; FightingLayer.Play(); }

        if (isFighting != battle) isFighting = battle;
        // if (intensified != intense) 
        intensified = ReferenceMaster.instance.player.Health <= 5 && battle;

        Tween.Volume(FinghtingIntenseLayer, ((intensified && battle) ? 1 : 0), 1, 0, Tween.EaseInOut);
        // if (intensified) Tween.Volume(FinghtingIntenseLayer, (battle ? 1 : 0), 2, 0, Tween.EaseInOut);
        Tween.Volume((battle ? FightingLayer : TownLayer), 1, 2, 2, Tween.EaseInOut);
    }



}