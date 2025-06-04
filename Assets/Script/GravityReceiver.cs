// In GravityReceiver.cs

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityReceiver : MonoBehaviour
{
    [SerializeField] private PlanetGravitySource gravitySource;
    [SerializeField] private float rotationSpeed = 5f;

    private Rigidbody rb;
    private bool gravityEnabled = true;
    public PlanetGravitySource CurrentGravitySource => gravitySource;

    public void SetGravityEnabled(bool enabled)
    {
        gravityEnabled = enabled;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (!gravityEnabled) return;
        if (CurrentGravitySource == null) return; // Use the getter here too for consistency

        // Apply gravity
        Vector3 gravityForce = CurrentGravitySource.GetGravity(transform.position, rb.mass);
        rb.AddForce(gravityForce);

        // Align up direction
        Vector3 gravityDirection = -CurrentGravitySource.GetGravityDirection(transform.position);
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public void SetGravitySource(PlanetGravitySource newSource)
    {
        gravitySource = newSource;
    }
}