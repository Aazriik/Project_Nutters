using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Variables
    [Header("Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Movement")]
    public float moveSpeed = 2f;

    private Vector3 nextPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the next position to pointB
        nextPosition = pointB.position;
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    void FixedUpdate()
    {
        // Move the platform towards the next position
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.fixedDeltaTime);

        // If the platform has reached the next position, switch to the other point
        if (transform.position == nextPosition)
        {
            // Toggle the next position between pointA and pointB
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
        }
    }

    // When the Player is on the platform, make the Player a child of the platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player collides with the platform, set the player as a child of the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    // When the Player leaves the platform, remove the Player as a child of the platform
    private void OnCollisionExit2D(Collision2D collision)
    {
        // If the player exits the platform, remove the player as a child of the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }

}
