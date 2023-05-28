using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class CowController : MonoBehaviour
{
    public float landMoveSpeed = 10;
    public float waterMoveSpeed = 2.5f;
    public float speedTransition = 3;


    public float MoveSpeed { get; private set; }
    public Vector3 MoveCommand { get; set; }

    private float cowRadius;
    private Vector3 displacement;

    // Initialization steps
    protected void Initialize()
    {
        cowRadius = GetComponent<CircleCollider2D>().radius;
        MoveSpeed = landMoveSpeed;
    }

    // General Processing
    // --------------------------------------

    public void Move()
    {
        // Calculate current speed based on terrain
        CalculateTerrainSpeed();

        // Reset displacement
        displacement.Set(0, 0, 0);

        // Peform displacement calculations
        ProcessMoveCommand();
        ProcessFriendlyCollisions();
        ProcessOutOfBounds();

        // Check for gameplay collisions
        ProcessHazardCollisions();

        // Move the cow
        displacement.z = 0;
        transform.Translate(displacement);
    }

    public void SetMoveCommand(Vector3 newMoveCommand)
    {
        MoveCommand = newMoveCommand;
    }

    protected void ProcessMoveCommand()
    {
        displacement = MoveCommand;
        MoveCommand = Vector3.zero;
    }

    protected void CalculateTerrainSpeed()
    {
        int layerMask = LayerMask.GetMask("Island");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + displacement, cowRadius, layerMask);
        bool isOnIsland = (hits.Length != 0);
        float targetSpeed = isOnIsland ? landMoveSpeed : waterMoveSpeed;

        if (Mathf.Abs(targetSpeed - MoveSpeed) > 0.1)
        {
            MoveSpeed += (targetSpeed - MoveSpeed) * Time.deltaTime * speedTransition;
        }
        else if (MoveSpeed != targetSpeed)
        {
            MoveSpeed = targetSpeed;
        }
    }

    // Processes collisions that occur based on attempted movement.
    // This will adjust the displacement to prevent collisions with obstacles.
    protected void ProcessFriendlyCollisions()
    {
        int layerMask = LayerMask.GetMask("NPC", "Player");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + displacement, cowRadius, layerMask);

        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();

        foreach (Collider2D hit in hits)
        {
            // Skip if we're checking against the current collider
            if (hit == circleCollider)
            {
                continue;
            }

            ColliderDistance2D colliderDistance = hit.Distance(circleCollider);

            if (colliderDistance.isOverlapped)
            {
                Vector2 adjustment = colliderDistance.pointA - colliderDistance.pointB;
                adjustment.x += displacement.x;
                adjustment.y += displacement.y;
                displacement = new(adjustment.x, adjustment.y, 0);
            }
        }
    }


    // Processes collisions with hazards.
    protected void ProcessHazardCollisions()
    {
        int layerMask = LayerMask.GetMask("Hazards");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + displacement, cowRadius, layerMask);

        if (hits.Length != 0)
        {
            HandleCowAbduction();
        }
    }

    protected void ProcessOutOfBounds()
    {
        int layerMask = LayerMask.GetMask("Boundaries");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + displacement, cowRadius, layerMask);

        if (hits.Length == 0)
        {
            displacement = Vector3.zero;
        }
    }

    protected abstract void HandleCowAbduction();
}
