using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    public GameObject cowmonnerPrefab;
    public uint numCowmonners = 3;
    public HerdManager cowmonners;

    private const uint MAX_COWS = 20;

    // Start is called before the first frame update
    void Start()
    {
        cowmonners.Initialize();
        numCowmonners = (uint) Mathf.Min(numCowmonners, MAX_COWS);

        SpawnCowCircles(new Vector3(0, 0, 0), numCowmonners);
    }

    void SpawnCowCircle(Vector3 center, uint numCows, float circleRadius)
    {
        for (int i = 0; i < numCows; i++)
        {
            float angle = ( (2 * Mathf.PI) / (float)numCows) * i;
            Vector3 cowLocation = center + new Vector3(Mathf.Cos(angle) * circleRadius, Mathf.Sin(angle) * circleRadius, 10);
            cowmonners.Add(Instantiate(cowmonnerPrefab, cowLocation, Quaternion.identity));
        }
    }

    void SpawnCowCircles(Vector3 center, uint numCows)
    {
        const int MAX_COWS_CIRCLE_1 = 5;
        const int MAX_COWS_CIRCLE_2 = 15;
        uint circle1Cows;
        uint circle2Cows = 0;

        if (numCows > MAX_COWS_CIRCLE_1)
        {
            circle1Cows = MAX_COWS_CIRCLE_1;
            circle2Cows = numCows - circle1Cows;
        }
        else
        {
            circle1Cows = numCows;
        }

        if (circle2Cows > MAX_COWS_CIRCLE_2)
        {
            circle2Cows = MAX_COWS_CIRCLE_2;
        }

        // Spawn cow circle 1
        SpawnCowCircle(center, circle1Cows, 2);
        // Spawn cow circle 2
        SpawnCowCircle(center, circle2Cows, 4);
    }
}