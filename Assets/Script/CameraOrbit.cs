using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Set Up")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;

    [Header("Rotation")]
    [SerializeField] private float sensitivity = 10000f;
    [SerializeField] private float inertiaDamping = 2f;

    private Vector3 previousPosition;
    private Vector2 currentVelocity;

    void Update()
    {
        if (target == null || cam == null) return;

        HandleRotation();
    }

    void HandleRotation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 currentMousePos = cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = previousPosition - currentMousePos;

            currentVelocity = new Vector2(direction.x, direction.y) * sensitivity;
            RotateCamera(currentVelocity);

            previousPosition = currentMousePos;
        }
        else if (currentVelocity.magnitude > 0.01f)
        {
            RotateCamera(currentVelocity);
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, Time.deltaTime * inertiaDamping);
        }
    }

    void RotateCamera(Vector2 velocity)
    {
        cam.transform.RotateAround(target.position, cam.transform.right, velocity.y * Time.deltaTime);
        cam.transform.RotateAround(target.position, Vector3.up, -velocity.x * Time.deltaTime);
    }
}
