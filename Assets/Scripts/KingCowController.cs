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

        movement.Normalize();
        movement = MoveSpeed * Time.deltaTime * movement;
        return movement;
    }


}
