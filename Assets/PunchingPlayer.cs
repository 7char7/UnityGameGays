using UnityEngine;

public class PunchingPlayer : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    public Rigidbody leftFist;
    public Rigidbody rightFist;

    [Header("Fist Settings")]
    public float hoverDistance = 1f;         // Default: 1
    public float horizontalOffset = -0.3f;   // Default: -0.3
    public float followStrength = 80f;       // Default: 80
    public float damping = 10f;              // Default: 10
    public float rotationSpeed = 10f;        // Rotation speed remains 10

    [Header("Punch Settings")]
    public float punchForce = 20f;           // Default: 20
    public float maxVelocity = 20f;          // Default: 20
    //hi
    void Start()
    {
        leftFist.useGravity = false;
        rightFist.useGravity = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Punch(rightFist);

        if (Input.GetMouseButtonDown(1))
            Punch(leftFist);
    }

    void FixedUpdate()
    {
        MoveAndRotateFist(leftFist, -horizontalOffset);
        MoveAndRotateFist(rightFist, horizontalOffset);
    }

    private void Punch(Rigidbody fist)
    {
        Vector3 punchDir = playerCamera.forward;
        fist.AddForce(punchDir * punchForce, ForceMode.Impulse);

        if (fist.velocity.magnitude > maxVelocity)
            fist.velocity = fist.velocity.normalized * maxVelocity;
    }

    private void MoveAndRotateFist(Rigidbody fist, float offset)
    {
        // --- Position ---
        Vector3 targetPos = playerCamera.position + playerCamera.forward * hoverDistance + playerCamera.right * offset;
        Vector3 toTarget = targetPos - fist.position;

        Vector3 force = toTarget * followStrength - fist.velocity * damping;
        fist.AddForce(force, ForceMode.Acceleration);

        // --- Rotation ---
        Quaternion targetRot = Quaternion.LookRotation(playerCamera.forward, Vector3.up);
        fist.rotation = Quaternion.Slerp(fist.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
    }
}
