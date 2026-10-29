using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float gravity = 20f;

    private float horizontal;
    private float forward;
    private float verticalVelocity;

    private bool isGrounded;

    // gets the needed components
    private AABBCollider playerCollider;
    private CollisionManager collisionManager;

    private void Start()
    {
        playerCollider = GetComponent<AABBCollider>();
        collisionManager = CollisionManager.Instance;
    }

    // for WASD and Jump ctrls
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        forward = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            verticalVelocity = jumpForce;
            isGrounded = false;
        }
    }

    // movement and gravity
    private void FixedUpdate()
    {
        if (playerCollider == null || collisionManager == null)
            return;

        verticalVelocity -= gravity * Time.fixedDeltaTime;

        Vector3 movement = new Vector3(horizontal, 0f, forward);
        movement = Vector3.ClampMagnitude(movement, 1f);
        movement *= moveSpeed * Time.fixedDeltaTime;

        Move(Vector3.right * movement.x);
        Move(Vector3.forward * movement.z);
        Move(Vector3.up * verticalVelocity * Time.fixedDeltaTime);
    }

    // colliding checking
    private void Move(Vector3 movement)
    {
        if (movement == Vector3.zero)
            return;

        Vector3 target = transform.position + movement;

        // restart the game if the player falls off the map
        if (collisionManager.CheckOverlap(
            playerCollider, target, ColliderType.FallCollider, out _))
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex);
            return;
        }

        // stops the player from going through obstacles
        if (collisionManager.CheckOverlap(
            playerCollider, target, ColliderType.Obstacle, out AABBCollider obstacle))
        {
            if (movement.y < 0f &&
                playerCollider.min.y >= obstacle.max.y - 0.05f)
            {
                Land(obstacle.max.y);
            }
            else if (movement.y > 0f)
            {
                verticalVelocity = 0f;
            }

            return;
        }

        if (movement.y < 0f &&
            collisionManager.CheckOverlap(
                playerCollider, target, ColliderType.Ground, out AABBCollider ground))
        {
            Land(ground.max.y);
            return;
        }

        transform.position = target;
        playerCollider.UpdateBounds();
        isGrounded = false;
    }

    // If landed or not on ground
    private void Land(float groundHeight)
    {
        Vector3 position = transform.position;
        position.y = groundHeight + playerCollider.HalfHeight;

        transform.position = position;
        verticalVelocity = 0f;
        isGrounded = true;

        playerCollider.UpdateBounds();
    }
}