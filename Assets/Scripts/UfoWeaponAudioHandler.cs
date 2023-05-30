using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoWeaponAudioHandler : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioSource cNote;
    public AudioSource aFlatNote;
    public AudioSource eFlatNote;
    public AudioSource bFlatNote;

    public void PlayWeaponAudio()
    {
        switch (audioManager.chord)
        {
            case AudioManager.ChordProgressionState.C:
                if (!cNote.isPlaying)
                {
                    cNote.Stop();
                }
                cNote.Play();
                break;
            case AudioManager.ChordProgressionState.Aflat:
                if (!aFlatNote.isPlaying)
                {
                    aFlatNote.Stop();
                }
                aFlatNote.Play();
                break;
            case AudioManager.ChordProgressionState.Eflat:
                if (!eFlatNote.isPlaying)
                {
                    eFlatNote.Stop();
                }
                eFlatNote.Play();
                break;
            case AudioManager.ChordProgressionState.Bflat:
                if (!bFlatNote.isPlaying)
                {
                    bFlatNote.Stop();
                }
                bFlatNote.Play();
                break;
        }
    }
}
