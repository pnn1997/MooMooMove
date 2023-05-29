using UnityEngine;

public class UfoController : MonoBehaviour
{
    public HerdManager targets;
    public CircleCollider2D weaponCollider;
    public SpriteRenderer weaponRenderer;
    public float moveSpeed = 12;
    public float chargeTime = 2;
    public float cooldownTime = 0.5f;

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
    }

    void Update()
    {
        switch (weaponState)
        {
            case WeaponState.IDLE:
                MoveToHerdCenter();
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

        Vector3 displacement = moveSpeed * Time.deltaTime * faceDirection;
        transform.Translate(displacement);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, faceDirection);
        if (GetComponent<SpriteRenderer>().isVisible)
        {
            // Apply smooth rotation when the ufo is on camera
            targetRotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                360 * Time.fixedDeltaTime);
        }
        transform.rotation = targetRotation;
    }
}
