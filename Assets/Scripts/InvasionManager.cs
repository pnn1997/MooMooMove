using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvasionManager : MonoBehaviour
{
    public GameObject kingCow;          // Used by UFOs for movement calculations
    public List<GameObject> invasion;   // Collection of UFOs
    public GameStateManager gameStateManager;
    public UfoWeaponAudioHandler audioHandler;

    private const int MAX_UFO_COUNT = 5;
    private static int counter = 0;

    private int chargeSpeedLevel;
    private float curChargeTime;
    private const float MIN_CHARGE_TIME = 1.0f;
    private const float INITIAL_CHARGE_TIME = 2.5f;

    private float boundsRadius;
    private Vector3 boundsPos;

    public void Initialize()
    {
        invasion = new List<GameObject>();
        chargeSpeedLevel = 0;
        curChargeTime = INITIAL_CHARGE_TIME;

        var waterBounds = GameObject.Find("WaterBounds").GetComponent<CircleCollider2D>();
        boundsRadius = waterBounds.radius;
        boundsPos = waterBounds.transform.position;
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
            if (counter == 0)
            {
                ufo.GetComponent<UfoController>().isLeaderUfo = true;
            }
            ufo.GetComponent<UfoController>().audioHandler = audioHandler;
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
        else
        {
            // Victory if we are in time attack mode
            if (!gameStateManager.isEndless)
            {
                // Have all UFOs run away
                 foreach (var ufo in invasion)
                {
                    ufo.GetComponent<UfoController>().isRetreat = true;
                }
            }


            // Check if all the UFOs have left the bounds
            int layerMask = LayerMask.GetMask("Enemies");
            Collider2D[] hits = Physics2D.OverlapCircleAll(Vector3.zero, 100, layerMask);
            if (hits.Length == 0)
            {
                gameStateManager.SetGameState(GameStateManager.GameState.VICTORY);
            }
        }
    }

    public int GetDifficulty()
    {
        return invasion.Count + chargeSpeedLevel;
    }
}
