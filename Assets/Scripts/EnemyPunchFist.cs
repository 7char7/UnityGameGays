using UnityEngine;

public class EnemyPunchFist : MonoBehaviour
{
    [Header("References")]
    public Transform target;     // Player
    public Transform armTarget;  // Where the fist rests (attached to enemy arm)
    public Rigidbody rb;

    [Header("Follow Settings")]
    public float followStrength = 120f;
    public float followDamping = 12f;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    [Header("Punch Settings")]
    public float punchForce = 25f;
    public float curveStrength = 15f;
    public float punchDuration = 0.25f;

    private bool isPunching;
    private float punchTimer;

    void Start()
    {
        rb.useGravity = false;

        if (armTarget != null)
            rb.position = armTarget.position;
    }

    void FixedUpdate()
    {
        FollowArm();
        RotateTowardTarget();
        HandlePunchCurve();
    }

    public void Punch()
    {
        if (target == null)
            return;

        isPunching = true;
        punchTimer = punchDuration;

        Vector3 directionToTarget = (target.position - transform.position).normalized;

        // Blend forward + target direction for a hook feel
        Vector3 punchDirection = (transform.forward + directionToTarget).normalized;

        rb.AddForce(punchDirection * punchForce, ForceMode.Impulse);
    }

    private void FollowArm()
    {
        if (armTarget == null)
            return;

        Vector3 toTarget = armTarget.position - rb.position;

        Vector3 force = toTarget * followStrength - rb.velocity * followDamping;

        rb.AddForce(force, ForceMode.Acceleration);
    }

    private void RotateTowardTarget()
    {
        if (target == null)
            return;

        Vector3 direction = (target.position - transform.position).normalized;

        // Keep rotation flat (important for clean boxing feel)
        direction.y = 0f;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);

        rb.rotation = Quaternion.Slerp(
            rb.rotation,
            targetRot,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    private void HandlePunchCurve()
    {
        if (!isPunching)
            return;

        punchTimer -= Time.fixedDeltaTime;

        if (punchTimer <= 0f)
        {
            isPunching = false;
            return;
        }

        if (target == null)
            return;

        // Curve direction (creates hook motion)
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        Vector3 curve = Vector3.Cross(directionToTarget, Vector3.up);

        rb.AddForce(curve * curveStrength, ForceMode.Acceleration);
    }

    private void OnDrawGizmos()
    {
        if (armTarget == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(armTarget.position, 0.05f);
        Gizmos.DrawLine(transform.position, armTarget.position);
    }
}