using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Variables

    [Header("Platform Movement Settings")]
    // relative distance from start position
    [SerializeField] private Vector2 moveOffset = new Vector2(0f, 0f);
    // how fast it moves (higher = faster)
    [SerializeField] private float speed = 2f;
    // use Rigidbody2D.MovePosition when available
    [SerializeField] private bool useRigidBody = true;

    // Component Refs
    Rigidbody2D rb;
    Collider2D col;

    // runtime positions
    Vector2 startPos;
    Vector2 targetPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        // Get the Collider2D component
        col = GetComponent<Collider2D>();

        // Choose reference start position (world)
        startPos = (rb != null) ? rb.position : (Vector2)transform.position;
        targetPos = startPos + moveOffset;
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    void FixedUpdate()
    {
        // Calculate interpolation parameter 0..1 using PingPong for continuous back-and-forth
        float t = Mathf.PingPong(Time.time * speed, 1f);
        Vector2 newPos = Vector2.Lerp(startPos, targetPos, t);

        if (useRigidBody && rb != null)
        {
            // Move through Rigidbody2D to keep physics happy
            rb.MovePosition(newPos);
        }
        else
        {
            // Fallback: move transform directly
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }

    // Draw the travel path in the editor to make configuration easier
    void OnDrawGizmosSelected()
    {
        Vector3 s = Application.isPlaying && startPos != null ? (Vector3)startPos : transform.position;
        Vector3 t = s + (Vector3)moveOffset;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(s, t);
        Gizmos.DrawSphere(s, 0.05f);
        Gizmos.DrawSphere(t, 0.05f);
    }
}
