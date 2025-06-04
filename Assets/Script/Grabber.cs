using UnityEngine;

public class Grabber : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float grabDistance = 100f;
    [SerializeField] private LayerMask grabbableLayer;

    [Header("Grabbing Behavior")]
    [SerializeField] private float grabSpeed = 10f;
    [SerializeField] private float heldObjectRotationSpeed = 5f;
    [SerializeField] private float minDistanceToPlanetSurface = 0.5f;

    private Transform grabbedObject;
    private Rigidbody grabbedRigidbody;
    private GravityReceiver grabbedGravity;
    private float initialGrabDepth;

    private PlanetGravitySource currentPlanet;
    private SphereCollider planetCollider;

    void Update()
    {
        if (!mainCamera)
        {
            Debug.LogError("Main Camera not assigned in Grabber!");
            enabled = false;
            return;
        }

        if (Input.GetMouseButtonDown(0)) TryGrab();
        if (Input.GetMouseButton(0) && grabbedObject) HoldObject();
        if (Input.GetMouseButtonUp(0) && grabbedObject) ReleaseObject();
    }

    void TryGrab()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, grabDistance, grabbableLayer)) return;

        var rb = hit.rigidbody;
        var gravity = hit.transform.GetComponent<GravityReceiver>();
        var gravitySource = gravity?.CurrentGravitySource;

        if (rb == null || gravity == null || gravitySource == null) return;

        planetCollider = gravitySource.GetComponent<SphereCollider>();
        if (!planetCollider)
        {
            Debug.LogError("PlanetGravitySource must have a SphereCollider.");
            return;
        }

        grabbedObject = hit.transform;
        grabbedRigidbody = rb;
        grabbedGravity = gravity;
        currentPlanet = gravitySource;

        grabbedGravity.SetGravityEnabled(false);
        grabbedRigidbody.isKinematic = true;
        initialGrabDepth = mainCamera.WorldToScreenPoint(grabbedObject.position).z;
    }

    void HoldObject()
    {
        if (!grabbedObject || !currentPlanet || !planetCollider) return;

        // Compute target position from mouse
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = initialGrabDepth;
        Vector3 targetPos = mainCamera.ScreenToWorldPoint(screenPoint);

        // Planet radius with scale considered
        Vector3 planetCenter = currentPlanet.transform.position;
        float scaledRadius = planetCollider.radius * currentPlanet.transform.lossyScale.x;
        float minAllowedDistance = scaledRadius + minDistanceToPlanetSurface;

        Vector3 direction = (targetPos - planetCenter);
        float currentDistance = direction.magnitude;

        if (currentDistance < minAllowedDistance)
        {
            targetPos = planetCenter + direction.normalized * minAllowedDistance;
        }

        // Move and rotate grabbed object
        grabbedObject.position = Vector3.Lerp(grabbedObject.position, targetPos, grabSpeed * Time.deltaTime);

        Vector3 upDirection = (grabbedObject.position - planetCenter).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(grabbedObject.up, upDirection) * grabbedObject.rotation;
        grabbedObject.rotation = Quaternion.Slerp(grabbedObject.rotation, targetRotation, heldObjectRotationSpeed * Time.deltaTime);
    }

    void ReleaseObject()
    {
        Debug.Log($"Released: {grabbedObject.name}");

        if (grabbedRigidbody) grabbedRigidbody.isKinematic = false;
        if (grabbedGravity) grabbedGravity.SetGravityEnabled(true);

        grabbedObject = null;
        grabbedRigidbody = null;
        grabbedGravity = null;
        currentPlanet = null;
        planetCollider = null;
    }
}
