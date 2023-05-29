using UnityEngine;

public class CowmonnerController : CowController
{
    public Vector3 direction;

    public float moveToKingStrength = 1;                // How fast the cows will move to the king
    public float moveToKingDistance = 1;                // How far the cows need to be from the king before trying to meet up
    public float moveToCenterStrength = 1;              // How fast the cows will move to the center
    public float localCowDistance = 1;                  // How close cows have to be to be considered close
    public float avoidOtherStrength = 1;                // How fast cows will move away from each other
    public float collisionAvoidCheckDistance = 0.2f;    // How close cows are before trying to avoid collition
    public float alignmentStrength = 1;                 // How fast cows will try to move in the same direction as other cows
    public float alignmentCheckDistance = 1;            // How close cows are before checking where the other cows are headed

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // Only move if in the current herd
        if (transform.parent.GetComponent<HerdManager>())
        {
            direction = Vector3.zero;

            AlignWithOthers();
            MoveToCenter();
            AvoidOtherCows();
            MoveToKing();

            MoveCommand = direction * (MoveSpeed * Time.deltaTime);
            Move();
        }
    }

    protected override void HandleCowAbduction()
    {
        var cowHerd = transform.parent.GetComponent<HerdManager>();
        if (cowHerd)
        {
            cowHerd.Remove(gameObject);
        }
        Destroy(gameObject);
    }

    private void MoveToKing()
    {
        HerdManager cows = transform.parent.GetComponent<HerdManager>();

        float distance = Vector2.Distance(cows.kingCow.transform.position, transform.position);
        float tolerance = Mathf.Abs(distance - moveToKingDistance);

        if (tolerance > 0.1)
        {
            Vector3 faceDirection = Vector3.zero;

            if (distance > moveToKingDistance)
            {
                faceDirection = (cows.kingCow.transform.position - transform.position).normalized;
            }

            //move cow towards the king
            float distMultFactor = tolerance * 10.0f;
            float deltaTimeStrength = (moveToKingStrength * distMultFactor) * Time.deltaTime;
            direction += deltaTimeStrength * faceDirection;
            direction /= (deltaTimeStrength + 1);
            direction = direction.normalized;
        }
    }

    private void MoveToCenter()
    {
        Vector3 positionSum = transform.position;
        int count = 0;

        HerdManager cows = transform.parent.GetComponent<HerdManager>();
        if (!cows)
        {
            return;
        }

        // Check against cowmonner cows
        foreach (var cow in cows.herd)
        {
            float distance = Vector2.Distance(cow.transform.position, transform.position);
            if (distance <= localCowDistance)
            {
                positionSum += cow.transform.position;
                count++;
            }
        }
        // Check against king cow
        float kingDistance = Vector2.Distance(cows.kingCow.transform.position, transform.position);
        //if the distance is within range calculate away vector from it and subtract from away direction.
        if (kingDistance <= localCowDistance)
        {
            positionSum += cows.kingCow.transform.position;
            count++;
        }
        if (count == 0)
        {
            return;
        }


        //get average position of cows
        Vector3 positionAverage = positionSum / count;
        Vector3 faceDirection = (positionAverage - transform.position).normalized;

        //move cows toward center
        float deltaTimeStrength = moveToCenterStrength * Time.deltaTime;
        direction += deltaTimeStrength * faceDirection;
        direction /= (deltaTimeStrength + 1);
        direction = direction.normalized;
    }

    private void AvoidOtherCows()
    {
        Vector3 faceAwayDirection = Vector3.zero;

        HerdManager cows = transform.parent.GetComponent<HerdManager>();
        if (!cows)
        {
            return;
        }

        // Check against other cows
        foreach (var cow in cows.herd)
        {
            float distance = Vector2.Distance(cow.transform.position, transform.position);

            //if the distance is within range calculate away vector from it and subtract from away direction.
            if (distance <= collisionAvoidCheckDistance)
            {
                faceAwayDirection += (transform.position - cow.transform.position);
            }
        }

        // Check against king cow
        float kingDistance = Vector2.Distance(cows.kingCow.transform.position, transform.position);
        //if the distance is within range calculate away vector from it and subtract from away direction.
        if (kingDistance <= collisionAvoidCheckDistance)
        {
            faceAwayDirection += (transform.position - cows.kingCow.transform.position);
        }

        faceAwayDirection.z = 0;
        faceAwayDirection = faceAwayDirection.normalized; //we need to normalize it so we are only getting direction

        direction += avoidOtherStrength * faceAwayDirection;
        direction /= (avoidOtherStrength + 1);
        direction = direction.normalized;
    }

    private void AlignWithOthers()
    {
        //we will need to find average direction of all nearby boids
        Vector3 directionSum = Vector3.zero;
        int count = 0;

        HerdManager cows = transform.parent.GetComponent<HerdManager>();
        if (!cows)
        {
            return;
        }

        foreach (var cow in cows.herd)
        {
            float distance = Vector2.Distance(cow.transform.position, transform.position);
            if (distance <= alignmentCheckDistance)
            {
                var cowDir = cow.GetComponent<CowmonnerController>().direction;
                directionSum += cowDir;
                count++;
            }
        }

        Vector3 directionAverage = directionSum / count;
        directionAverage.z = 0;
        directionAverage = directionAverage.normalized;

        //now add this direction to direction vector to steer towards it
        float deltaTimeStrength = alignmentStrength * Time.deltaTime;
        direction += deltaTimeStrength * directionAverage;
        direction /= (deltaTimeStrength + 1);
        direction = direction.normalized;
    }

}
