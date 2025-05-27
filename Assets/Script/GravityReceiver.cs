using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityReceiver : MonoBehaviour
{
    [SerializeField] private PlanetGravitySource gravitySource;
    [SerializeField] private float rotationSpeed = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (gravitySource == null) return;

        // Apply gravity
        Vector3 gravityForce = gravitySource.GetGravity(transform.position, rb.mass);
        rb.AddForce(gravityForce);

        // Align up direction
        Vector3 gravityDirection = -gravitySource.GetGravityDirection(transform.position);
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public void SetGravitySource(PlanetGravitySource newSource)
    {
        gravitySource = newSource;
    }
}
