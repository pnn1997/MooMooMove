using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdManager : MonoBehaviour
{
    public GameObject kingCow;      // Used by cows in the herd for movement calculations
    public CowAudioHandler audioHandler;
    public List<GameObject> herd;
    public bool IsHerdGone { get; private set; }

    private static int counter = 0;

    public void Initialize()
    {
        IsHerdGone = false;
        herd = new List<GameObject>();
    }

    public void Add(GameObject cow)
    {
        if (cow.GetComponent<CowmonnerController>())
        {
            herd.Add(cow);
            cow.transform.parent = this.transform;
            cow.name = "Cow " + counter.ToString();
            cow.GetComponent<CowmonnerController>().audioHandler = audioHandler;
            counter++;
        }
    }

    public void Remove(GameObject cow)
    {
        herd.Remove(cow);

        if (herd.Count == 0)
        {
            IsHerdGone = true;
        }
    }

    public int GetHerdSize()
    {
        return herd.Count;
    }
}
