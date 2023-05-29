using UnityEngine;

public class KingCowController : CowController
{
    public bool IsAlive { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        IsAlive = true;
        Initialize();
        checkForFriendlyCollision = false;
        CowController.remainingSpeedBoost = MAX_SPEED_BOOST_DURATION;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCommand = ProcessKeyInput();
        Move();
    }

    // Handles game over event
    protected override void HandleCowAbduction()
    {
        IsAlive = false;
    }

    // Processes key input to determine where the king cow is attempting to move
    private Vector3 ProcessKeyInput()
    {
        Vector3 movement = new(0.0f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            movement += Vector3.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            movement += Vector3.left;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            movement += Vector3.down;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            movement += Vector3.up;
        }

        // Hold space for speed boost
        if (Input.GetKey(KeyCode.Space) && CowController.remainingSpeedBoost > 0.0f)
        {
            CowController.isSpeedBoostEnabled = true;
        }
        // Disable speed boost when space key is no longer pressed
        // Begin recharge cooldown time if the speed boost was just disabled
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CowController.isSpeedBoostEnabled = false;
            CowController.remainingCooldownTime = 0.0f;
        }

        // Process remaining speed boost amounts
        if (CowController.isSpeedBoostEnabled)
        {
            CowController.remainingSpeedBoost -= Time.deltaTime;

            if (CowController.remainingSpeedBoost <= 0)
            {
                CowController.remainingSpeedBoost = 0;
                CowController.remainingCooldownTime = 0.0f;
                CowController.isSpeedBoostEnabled = false;
            }
        }
        else
        {
            if (CowController.remainingCooldownTime < SPEED_BOOST_COOLDOWN_DURATION)
            {
                CowController.remainingCooldownTime += Time.deltaTime;
            }
            else
            {
                if (CowController.remainingSpeedBoost > MAX_SPEED_BOOST_DURATION)
                {
                    CowController.remainingSpeedBoost = MAX_SPEED_BOOST_DURATION;
                }
                else if (CowController.remainingSpeedBoost < MAX_SPEED_BOOST_DURATION)
                {
                    CowController.remainingSpeedBoost += Time.deltaTime;
                }
            }
        }

        movement.Normalize();
        movement = MoveSpeed * Time.deltaTime * movement;
        return movement;
    }


}
