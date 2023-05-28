using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdManager : MonoBehaviour
{
    public GameObject kingCow;
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
            cow.name = counter.ToString();
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


}
