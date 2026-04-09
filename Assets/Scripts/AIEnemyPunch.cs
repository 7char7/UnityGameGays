using UnityEngine;

public class AIEnemyPunch : MonoBehaviour
{
    [Header("References")]
    public EnemyPunchFist leftFist;
    public EnemyPunchFist rightFist;
    public Transform player;

    [Header("Settings")]
    public float punchRange = 2.5f;
    public float punchCooldown = 1.0f;

    private float cooldownTimer;
    private bool useRightHand = true;

    void Update()
    {
        if (player == null)
            return;

        cooldownTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= punchRange && cooldownTimer <= 0f)
        {
            Punch();
            cooldownTimer = punchCooldown;
        }
    }

    private void Punch()
    {
        if (useRightHand && rightFist != null)
            rightFist.Punch();
        else if (leftFist != null)
            leftFist.Punch();

        useRightHand = !useRightHand;
    }
}