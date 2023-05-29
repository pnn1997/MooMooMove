using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    public Slider boostBar;

    private void Start()
    {
        boostBar.maxValue = CowController.MAX_SPEED_BOOST_DURATION;
        boostBar.value = CowController.remainingSpeedBoost;
    }

    private void Update()
    {
        boostBar.value = CowController.remainingSpeedBoost;
    }
}
