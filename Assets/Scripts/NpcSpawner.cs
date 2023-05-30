using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    public GameObject cowmonnerPrefab;
    public uint numCowmonners = 3;
    public HerdManager cowmonners;
    public GameObject ufoPrefab;
    public InvasionManager invasion;
    public KingCowController kingCow;

    public Vector3 ufoSpawnLocation = new(0, -50, 0);
    public float ufoSpawnTime = 5.0f;                  // Seconds before the next UFO spawns
    private float elapsedTime;
    private const uint MAX_COWS = 50;

    // Start is called before the first frame update
    void Start()
    {
        cowmonners.Initialize();
        numCowmonners = (uint) Mathf.Min(numCowmonners, MAX_COWS);
        SpawnCowCircles(kingCow.transform.position, numCowmonners);

        invasion.Initialize();
        SpawnUfo();
    }

    void Update()
    {
        // Scoring is added every second for each cow still alive in the herd
        if (elapsedTime > ufoSpawnTime)
        {
            elapsedTime = 0;
            if (invasion.CanAddUfo())
            {
                SpawnUfo();
            }
            else
            {
                invasion.IncreaseChargeSpeed();
            }
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }
    }

    void SpawnUfo()
    {
        var ufo = Instantiate(ufoPrefab, ufoSpawnLocation, Quaternion.identity);
        invasion.Add(ufo);
        ufo.GetComponent<UfoController>().targets = cowmonners;
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
        const int MAX_COWS_CIRCLE_3 = 30;

        uint circle1Cows;
        uint circle2Cows = 0;
        uint circle3Cows = 0;

        if (numCows > MAX_COWS_CIRCLE_1)
        {
            circle2Cows = numCows - MAX_COWS_CIRCLE_1;
            circle1Cows = MAX_COWS_CIRCLE_1;
        }
        else
        {
            circle1Cows = numCows;
        }

        if (circle2Cows > MAX_COWS_CIRCLE_2)
        {
            circle3Cows = circle2Cows - MAX_COWS_CIRCLE_2;
            circle2Cows = MAX_COWS_CIRCLE_2;
        }

        if (circle3Cows > MAX_COWS_CIRCLE_3)
        {
            circle3Cows = MAX_COWS_CIRCLE_3;
        }

        // Spawn cow circle 1
        SpawnCowCircle(center, circle1Cows, 2);
        // Spawn cow circle 2
        SpawnCowCircle(center, circle2Cows, 4);
        // Spawn cow circle 3
        SpawnCowCircle(center, circle3Cows, 6);
    }
}
