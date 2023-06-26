using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float swerveSpeed = 5f;
    [SerializeField] private float distanceMultiplier = 0.0005f;

    private Vector2 lastMousePosition;
    private Vector3 targetPosition;


    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = Input.mousePosition;
            float swipeDistance = currentMousePosition.x - lastMousePosition.x;

            targetPosition += distanceMultiplier * swipeDistance * Vector3.right;

            lastMousePosition = currentMousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            targetPosition = transform.position;
        }

        targetPosition.z = transform.position.z;
        transform.Translate(movementSpeed * Time.deltaTime * Vector3.forward, Space.World);
        transform.position = Vector3.Lerp(transform.position, targetPosition, swerveSpeed * Time.deltaTime);
    }
}
