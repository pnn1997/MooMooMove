using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAnimator : MonoBehaviour
{
    public SpriteRenderer waterSprite1;
    public SpriteRenderer waterSprite2;
    public float animationTime = 5;
    public float alphaTarget = 0.2f;

    private WaterTextureState waterState;
    private float angle1;
    private float angle2;

    private enum WaterTextureState
    {
        TEXTURE_1_ACTIVE,
        TEXTURE_1_FADING,
        TEXTURE_2_ACTIVE,
        TEXTURE_2_FADING
    }

    void Start()
    {
        waterSprite1.color = new Color(255, 255, 255, alphaTarget);
        waterSprite2.color = new Color(255, 255, 255, 0);
        waterState = WaterTextureState.TEXTURE_1_FADING;
        angle1 = 0;
        angle2 = 100;
    }

    void Update()
    {
        switch (waterState)
        {
            case WaterTextureState.TEXTURE_1_ACTIVE:
                ActivateTexture(waterSprite1, WaterTextureState.TEXTURE_1_FADING);
                break;
            case WaterTextureState.TEXTURE_1_FADING:
                DeactivateTexture(waterSprite1, WaterTextureState.TEXTURE_2_ACTIVE);
                break;
            case WaterTextureState.TEXTURE_2_ACTIVE:
                ActivateTexture(waterSprite2, WaterTextureState.TEXTURE_2_FADING);
                break;
            case WaterTextureState.TEXTURE_2_FADING:
                DeactivateTexture(waterSprite2, WaterTextureState.TEXTURE_1_ACTIVE);
                break;
        }
    }

    void RotateTexture1()
    {
        angle1 += 60;
        waterSprite1.transform.rotation = Quaternion.AngleAxis(angle1, Vector3.forward);
    }
    void RotateTexture2()
    {
        angle2 += 45;
        waterSprite2.transform.rotation = Quaternion.AngleAxis(angle2, Vector3.forward);
    }

    void ActivateTexture(SpriteRenderer waterSprite, WaterTextureState nextState)
    {
        float alphaVal = waterSprite.color.a;

        if (alphaVal == 0)
        {
            // First activation
            RotateTexture1();
            RotateTexture2();
        }

        if (alphaVal >= alphaTarget)
        {
            waterState = nextState;
        }
        else
        {
            var alphaUpdate = alphaTarget / animationTime;
            var newAlpha = alphaVal + (alphaUpdate * Time.deltaTime);

            waterSprite.color = new(255, 255, 255, newAlpha);
        }
    }

    void DeactivateTexture(SpriteRenderer waterSprite, WaterTextureState nextState)
    {
        float alphaVal = waterSprite.color.a;

        if (alphaVal <= 0)
        {
            waterState = nextState;
        }
        else
        {
            var alphaUpdate = alphaTarget / animationTime;
            var newAlpha = alphaVal - (alphaUpdate * Time.deltaTime);

            waterSprite.color = new(255, 255, 255, newAlpha);
        }
    }
}
