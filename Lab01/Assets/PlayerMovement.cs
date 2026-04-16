using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 platformVelocity;
    private bool groundedPlayer;

    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravityValue = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        // Fix: Even if grounded, keep a tiny bit of downward force 
        // so the controller "sticks" to the floor properly.
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        // Directions fixed (inverted as we discussed)
        Vector3 move = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Jumping logic
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            // This math calculates the upward force needed
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        // Apply gravity continuously
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Final move call for vertical movement (falling/jumping)
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void FixedUpdate()
    {
        CheckForPlatform();

        if (groundedPlayer && platformVelocity != Vector3.zero)
        {
            controller.Move(platformVelocity * Time.fixedDeltaTime);
        }
    }

    private void CheckForPlatform()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f, LayerMask.GetMask("Platforms")))
        {
            MovingPlatform platform = hit.collider.GetComponent<MovingPlatform>();
            if (platform != null)
            {
                platformVelocity = platform.GetVelocity();
                return;
            }
        }
        platformVelocity = Vector3.zero;
    }
}