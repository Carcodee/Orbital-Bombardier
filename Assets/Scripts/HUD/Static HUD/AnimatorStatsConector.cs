using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStatsConector : MonoBehaviour
{
    public MenuController menuRef;
    public AudioSource dropSupplyAudioRef;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void GetAnimFunc()
    {
        menuRef.LoadNextText();
    }
    public void GetBombTextAnimFunc()
    {
        menuRef.DeactivateBombText();
    }
    public void playAudio()
    {
        dropSupplyAudioRef.PlayOneShot(dropSupplyAudioRef.clip);
    }
}
