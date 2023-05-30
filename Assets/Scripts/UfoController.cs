using UnityEngine;

public class UfoController : MonoBehaviour
{
    public HerdManager targets;
    public UfoWeaponAudioHandler audioHandler;
    public CircleCollider2D weaponCollider;
    public SpriteRenderer weaponRenderer;
    public float moveSpeed = 12;
    public float chargeTime = 2;
    public float cooldownTime = 0.5f;
    public bool isRetreat = false;
    public bool isLeaderUfo = false;

    private Vector3 direction;
    private Vector3 displacement;
    private float ufoRadius;

    public float avoidOtherStrength = 1;                // How fast ufos will move away from each other
    public float collisionAvoidCheckDistance = 30f;     // How close ufos are before trying to avoid collition

    private enum WeaponState
    {
        IDLE,
        CHARGING,
        COOLDOWN
    }
    private WeaponState weaponState;

    void Start()
    {
        DisableWeapon();
        ufoRadius = GetComponent<CircleCollider2D>().radius;
        isRetreat = false;
    }

    void Update()
    {
        switch (weaponState)
        {
            case WeaponState.IDLE:
                // Calculate the intended direction
                MoveToHerdCenter();
                AvoidOtherUfos();

                // Perform translations/rotations
                direction.z = 0;
                direction.Normalize();
                displacement = moveSpeed * Time.deltaTime * direction;

                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                targetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    360 * Time.fixedDeltaTime);
                transform.rotation = targetRotation;

                // Process forced displacements due to boundary collisions
                ProcessFriendlyCollisions();

                transform.Translate(displacement, Space.World);
                break;
            case WeaponState.CHARGING:
                ChargeWeapon();
                break;
            case WeaponState.COOLDOWN:
                WeaponCooldown();
                break;
        }
    }

    private void InitializeWeapon()
    {
        weaponRenderer.color = new (255, 0, 0, 0);
        weaponRenderer.enabled = true;
        weaponState = WeaponState.CHARGING;
    }

    private void ChargeWeapon()
    {
        var alphaVal = weaponRenderer.color.a;
        if (alphaVal >= 1.0f)
        {
            // Fire weapon since it's fully charged
            FireWeapon();
        }
        else
        {
            var chargeAmount = 1.0f / chargeTime;
            var newAlpha = alphaVal + (chargeAmount * Time.deltaTime);
            weaponRenderer.color = new(255, 0, 0, newAlpha);
        }
    }

    private void FireWeapon()
    {
        weaponRenderer.color = new(0, 0, 0);
        weaponCollider.enabled = true;
        weaponState = WeaponState.COOLDOWN;
        audioHandler.PlayWeaponAudio();
    }

    private void WeaponCooldown()
    {
        // Disable hitbox while cooldown is in effect
        weaponCollider.enabled = false;

        var alphaVal = weaponRenderer.color.a;
        if (alphaVal <= 0f)
        {
            // Weapon has completely cooled down
            DisableWeapon();
        }
        else
        {
            var chargeAmount = 0.75f / cooldownTime;
            var newAlpha = alphaVal - (chargeAmount * Time.deltaTime);
            weaponRenderer.color = new(0, 0, 0, newAlpha);
        }
    }

    private void DisableWeapon()
    {
        weaponCollider.enabled = false;
        weaponRenderer.enabled = false;
        weaponState = WeaponState.IDLE;
    }

    private void MoveToHerdCenter()
    {
        Vector3 positionSum = Vector3.zero;
        int count = 0;

        foreach (var cow in targets.herd)
        {
            positionSum += cow.transform.position;
            count++;
        }

        if (count == 0)
        {
            return;
        }

        // Get average position of cows
        Vector3 positionAverage = positionSum / count;
        positionAverage.z = 0;
        Vector3 faceDirection = (positionAverage - transform.position);
        faceDirection.z = 0;

        // Start charging the weapon if close to center of the herd
        if (faceDirection.magnitude < 1.0f)
        {
            InitializeWeapon();
        }

        faceDirection.Normalize();
        direction = faceDirection;
        
        if (isRetreat)
        {
            direction *= -1;
        }
    }

    protected void ProcessFriendlyCollisions()
    {
        if (isRetreat || isLeaderUfo)
        {
            // Ignore for leader UFO or if UFOs should retreat
            return;
        }

        int layerMask = LayerMask.GetMask("Enemies");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + displacement, ufoRadius, layerMask);

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

    private void AvoidOtherUfos()
    {
        if (isLeaderUfo)
        {
            // Ignore for leader UFO or if UFOs should retreat
            return;
        }

        Vector3 faceAwayDirection = Vector3.zero;

        InvasionManager ufos = transform.parent.GetComponent<InvasionManager>();
        if (!ufos)
        {
            return;
        }

        // Check against other ufos
        foreach (var ufo in ufos.invasion)
        {
            float distance = Vector2.Distance(ufo.transform.position, transform.position);

            //if the distance is within range calculate away vector from it and subtract from away direction.
            if (distance <= collisionAvoidCheckDistance)
            {
                faceAwayDirection += (transform.position - ufo.transform.position);
            }
        }

        faceAwayDirection.z = 0;
        faceAwayDirection = faceAwayDirection.normalized; //we need to normalize it so we are only getting direction

        direction += avoidOtherStrength * faceAwayDirection;
        direction /= (avoidOtherStrength + 1);
        direction = direction.normalized;
    }
}
