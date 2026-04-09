using UnityEngine;
using UnityEngine.AI;

public class AIEnemyMovement : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;

    [Header("Ranges")]
    public float attackRange = 2.5f;   // when to stop and punch
    public float tooCloseRange = 1.2f;  // when to back away

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        HandleMovement(distance);
        KeepUpright();
    }

    private void HandleMovement(float distance)
    {
        if (distance > attackRange)
        {
            // Move toward player
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else if (distance < tooCloseRange)
        {
            // Back away if too close
            Vector3 directionAway = (transform.position - player.position).normalized;
            Vector3 retreatPosition = transform.position + directionAway * 2f;

            agent.isStopped = false;
            agent.SetDestination(retreatPosition);
        }
        else
        {
            // In ideal punching range → stop
            agent.isStopped = true;
        }
    }

    private void KeepUpright()
    {
        Vector3 euler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }
}