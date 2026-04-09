using UnityEngine;

public class PunchFist : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    public Transform playerRoot; // 👈 NEW (the object that rotates)
    public Rigidbody rb;

    [Header("Arm Target (move in scene)")]
    public Transform armTarget;

    [Header("Movement")]
    public float followStrength = 120f;
    public float damping = 12f;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    [Header("Punch")]
    public float punchForce = 25f;

    [Header("Curve")]
    public float curveStrength = 20f;
    public float rotationInfluence = 2f; // 🔥 how much turning affects curve

    private bool isPunching;
    private float punchTimer;

    private float lastYaw;
    private float yawVelocity;

    void Start()
    {
        rb.useGravity = false;

        if (armTarget != null)
            rb.position = armTarget.position;

        // Initialize rotation tracking
        lastYaw = playerRoot.eulerAngles.y;
    }

    void FixedUpdate()
    {
        TrackRotation();

        FollowArmTarget();
        RotateTowardCamera();
        ApplyCurve();
    }

    // 🔥 Track how fast the player is turning
    private void TrackRotation()
    {
        float currentYaw = playerRoot.eulerAngles.y;

        float delta = Mathf.DeltaAngle(lastYaw, currentYaw);

        yawVelocity = delta / Time.fixedDeltaTime;

        lastYaw = currentYaw;
    }

    public void Punch()
    {
        isPunching = true;
        punchTimer = 0.3f;

        Vector3 forwardDir = playerCamera.forward;

        // slight hook bias based on hand side
        float side = Mathf.Sign(armTarget.localPosition.x);
        Vector3 hookBias = playerCamera.right * side * 0.2f;

        Vector3 punchDir = (forwardDir + hookBias).normalized;

        rb.AddForce(punchDir * punchForce, ForceMode.Impulse);
    }

    private void FollowArmTarget()
    {
        if (armTarget == null)
            return;

        Vector3 toTarget = armTarget.position - rb.position;

        Vector3 force = toTarget * followStrength - rb.velocity * damping;

        rb.AddForce(force, ForceMode.Acceleration);
    }

    private void ApplyCurve()
    {
        if (!isPunching)
            return;

        punchTimer -= Time.fixedDeltaTime;

        if (punchTimer <= 0f)
        {
            isPunching = false;
            return;
        }

        // 🔥 Use player rotation instead of mouse input
        float turn = yawVelocity * rotationInfluence;

        // Clamp so it doesn’t explode
        turn = Mathf.Clamp(turn, -curveStrength, curveStrength);

        Vector3 curveForce = playerCamera.right * turn;

        rb.AddForce(curveForce, ForceMode.Acceleration);
    }

    private void RotateTowardCamera()
    {
        Quaternion targetRotation = Quaternion.LookRotation(playerCamera.forward, Vector3.up);

        rb.rotation = Quaternion.Slerp(
            rb.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    private void OnDrawGizmos()
    {
        if (armTarget == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(armTarget.position, 0.05f);
        Gizmos.DrawLine(transform.position, armTarget.position);
    }
}