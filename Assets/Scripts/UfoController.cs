using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoController : MonoBehaviour
{
    public GameObject targets;
    public GameObject weapon;
    public float moveSpeed = 12;
    public float chargeTime = 2;
    public float cooldownTime = 0.5f;

    private enum WeaponState
    {
        IDLE,
        CHARGING,
        FIRED
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
            case WeaponState.FIRED:
                WeaponCooldown();
                break;
        }
    }

    private void InitializeWeapon()
    {
        var renderer = weapon.GetComponent<SpriteRenderer>();
        if (!renderer)
        {
            return;
        }

        renderer.color = new (255, 0, 0, 0);
        renderer.enabled = true;
        weaponState = WeaponState.CHARGING;
    }

    private void ChargeWeapon()
    {
        var collider = weapon.GetComponent<CircleCollider2D>();
        var renderer = weapon.GetComponent<SpriteRenderer>();
        if (!collider || !renderer)
        {
            return;
        }

        var alphaVal = renderer.color.a;
        if (alphaVal >= 1.0f)
        {
            // Fire weapon since it's fully charged
            FireWeapon();
        }
        else
        {
            var chargeAmount = 1.0f / chargeTime;
            var newAlpha = alphaVal + (chargeAmount * Time.deltaTime);
            renderer.color = new(255, 0, 0, newAlpha);
        }
    }

    private void FireWeapon()
    {
        var collider = weapon.GetComponent<CircleCollider2D>();
        var renderer = weapon.GetComponent<SpriteRenderer>();
        if (!collider || !renderer)
        {
            return;
        }

        renderer.color = new(0, 0, 0);
        collider.enabled = true;
        weaponState = WeaponState.FIRED;
    }

    private void WeaponCooldown()
    {
        var collider = weapon.GetComponent<CircleCollider2D>();
        var renderer = weapon.GetComponent<SpriteRenderer>();
        if (!collider || !renderer)
        {
            return;
        }

        var alphaVal = renderer.color.a;
        if (alphaVal <= 0)
        {
            // Weapon has completely cooled down
            DisableWeapon();
        }
        else
        {
            var chargeAmount = 1.0f / cooldownTime;
            var newAlpha = alphaVal - (chargeAmount * Time.deltaTime);
            renderer.color = new(0, 0, 0, newAlpha);
        }
    }

    private void DisableWeapon()
    {
        weaponState = WeaponState.IDLE;
        var collider = weapon.GetComponent<CircleCollider2D>();
        var renderer = weapon.GetComponent<SpriteRenderer>();
        if (!collider || !renderer)
        {
            return;
        }

        collider.enabled = false;
        renderer.enabled = false;
    }

    private void MoveToHerdCenter()
    {
        HerdManager cows = targets.GetComponent<HerdManager>();
        if (!cows)
        {
            return;
        }

        Vector3 positionSum = Vector3.zero;
        int count = 0;

        foreach (var cow in cows.herd)
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
        targetRotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            360 * Time.fixedDeltaTime);
        transform.rotation = targetRotation;
    }
}
