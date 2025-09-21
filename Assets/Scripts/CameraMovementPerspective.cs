using UnityEngine;


public class CameraMovementPerspective : MonoBehaviour
{
    [Header("Movement Speeds")]
    [Tooltip("The speed at which the camera moves using WASDQE keys.")]
    public float moveSpeed = 30.0f;
    [Tooltip("The speed at which the camera zooms using the mouse scroll wheel.")]
    public float zoomSpeed = 50.0f;

    [Header("Movement Boundaries")]
    [Tooltip("The minimum (X, Y, Z) coordinates the camera can move to.")]
    public Vector3 minBounds;
    [Tooltip("The maximum (X, Y, Z) coordinates the camera can move to.")]
    public Vector3 maxBounds;

    void Update()
    {
        HandleKeyboardMovement();
        HandleZoom();
        ApplyBoundaryClamping();
    }

    /// <summary>
    /// Handles camera movement using WASDQE keys.
    /// </summary>
    private void HandleKeyboardMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        // Forward/Backward movement
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= transform.forward;
        }

        // Left/Right movement
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += transform.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= transform.right;
        }

        // Up/Down movement
        if (Input.GetKey(KeyCode.E))
        {
           
            moveDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            
            moveDirection -= Vector3.up;
        }

        // Normalize to prevent faster diagonal movement and apply speed
        // Time.deltaTime makes the movement frame-rate independent
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }


    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

       
        if (scroll != 0.0f)
        {
            transform.position += transform.forward * scroll * zoomSpeed;
        }
    }


    private void ApplyBoundaryClamping()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.Clamp(currentPosition.x, minBounds.x, maxBounds.x);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minBounds.y, maxBounds.y);
        currentPosition.z = Mathf.Clamp(currentPosition.z, minBounds.z, maxBounds.z);

        transform.position = currentPosition;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.25f); // Green, semi-transparent

        // Calculate the center and size of the bounding box
        Vector3 center = (minBounds + maxBounds) / 2;
        Vector3 size = new Vector3(
            maxBounds.x - minBounds.x,
            maxBounds.y - minBounds.y,
            maxBounds.z - minBounds.z
        );

        // Draw the wireframe cube
        Gizmos.DrawCube(center, size);
    }
}

