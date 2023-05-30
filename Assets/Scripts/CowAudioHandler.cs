using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowAudioHandler : MonoBehaviour
{
    public AudioSource[] cowNoises;
    public int currentCow = -1;

    private int cowMap = 0;
    private int fullCowMap = 0;

    private void Start()
    {
        currentCow = -1;
    }

    public void PlayCowAudio()
    {
        currentCow = Random.Range(0, cowNoises.Length);
        cowNoises[currentCow].Play();
    }
}
