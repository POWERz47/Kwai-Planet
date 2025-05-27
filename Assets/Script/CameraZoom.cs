using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("Set Up")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoomDistance = 2f;
    [SerializeField] private float maxZoomDistance = 50f;
    [SerializeField] private float smoothTime = 0.2f;

    private float targetDistance;
    private Vector3 currentVelocity;

    void Start()
    {
        if (target != null && cam != null)
        {
            targetDistance = Vector3.Distance(cam.transform.position, target.position);
        }
    }

    void Update()
    {
        if (target == null || cam == null) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetDistance -= scroll * zoomSpeed;
            targetDistance = Mathf.Clamp(targetDistance, minZoomDistance, maxZoomDistance);
        }

        Vector3 direction = (cam.transform.position - target.position).normalized;
        Vector3 targetPosition = target.position + direction * targetDistance;

        cam.transform.position = Vector3.SmoothDamp(
            cam.transform.position,
            targetPosition,
            ref currentVelocity,
            smoothTime
        );
    }
}
