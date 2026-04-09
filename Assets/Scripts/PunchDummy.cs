using UnityEngine;

public class PunchDummy : MonoBehaviour
{
    public float wobbleStrength = 10f;    // How much it wobbles when hit
    public float returnSpeed = 5f;        // How fast it returns upright
    public float uprightStrength = 50f;   // How strongly it stays upright

    private Rigidbody rb;
    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        // Keep bottom planted hih
        Vector3 pos = rb.position;
        pos.y = 1f; // height of capsule bottom
        rb.position = pos;

        // Apply an upright torque (spring back to standing)
        Quaternion current = transform.rotation;
        Quaternion target = initialRotation;

        Quaternion delta = target * Quaternion.Inverse(current);
        delta.ToAngleAxis(out float angle, out Vector3 axis);

        // Torque to return upright
        if (angle > 180f) angle -= 360f;

        rb.AddTorque(axis * angle * uprightStrength);

        // Add damping to stop endless wobble
        rb.angularVelocity *= (1f - Time.fixedDeltaTime * returnSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If hit by fists, wobble opposite direction of impact
        if (collision.rigidbody != null)
        {
            Vector3 hitForce = collision.relativeVelocity * collision.rigidbody.mass;
            Vector3 torque = Vector3.Cross(Vector3.up, hitForce);

            rb.AddTorque(torque * wobbleStrength, ForceMode.Impulse);
        }
    }
}
