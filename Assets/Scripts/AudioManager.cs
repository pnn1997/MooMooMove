using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public ChordProgressionState chord;

    public enum ChordProgressionState
    {
        C = 0,
        Aflat = 1,
        Eflat = 2,
        Bflat = 3
    }
    private const int MAX_TIME_SAMPLES = 1209868;

    void Start()
    {
        chord = ChordProgressionState.C;
    }

    void Update()
    {
        chord = (ChordProgressionState) ((4 * musicSource.timeSamples) / MAX_TIME_SAMPLES);
    }
}
