using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvasionManager : MonoBehaviour
{
    public GameObject kingCow;          // Used by UFOs for movement calculations
    public List<GameObject> invasion;   // Collection of UFOs

    private const int MAX_UFO_COUNT = 5;
    private static int counter = 0;

    private int chargeSpeedLevel;
    private float curChargeTime;
    private const float MIN_CHARGE_TIME = 1.0f;
    private const float INITIAL_CHARGE_TIME = 2.5f;

    public void Initialize()
    {
        invasion = new List<GameObject>();
        chargeSpeedLevel = 0;
        curChargeTime = INITIAL_CHARGE_TIME;
    }

    public bool CanAddUfo()
    {
        return invasion.Count < MAX_UFO_COUNT;
    }

    public void Add(GameObject ufo)
    {
        if (ufo.GetComponent<UfoController>())
        {
            invasion.Add(ufo);
            ufo.transform.parent = this.transform;
            ufo.name = "UFO " + counter.ToString();
            ufo.GetComponent<UfoController>().chargeTime = curChargeTime;
            counter++;
        }
    }

    public void IncreaseChargeSpeed()
    {
        if (curChargeTime > MIN_CHARGE_TIME)
        {
            chargeSpeedLevel++;
            curChargeTime = INITIAL_CHARGE_TIME - (0.1f * chargeSpeedLevel);

            foreach (var ufo in invasion)
            {
                ufo.GetComponent<UfoController>().chargeTime = curChargeTime;
            }
        }
    }

    public int GetDifficulty()
    {
        return invasion.Count + chargeSpeedLevel;
    }
}
