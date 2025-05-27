using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlanetGravitySource : MonoBehaviour
{
    [SerializeField] private float gravity = 9.81f;

    public float Gravity => gravity;
    public Vector3 GetGravityDirection(Vector3 position)
    {
        return (transform.position - position).normalized;
    }

    public Vector3 GetGravity(Vector3 position, float mass)
    {
        return GetGravityDirection(position) * gravity * mass;
    }
}
