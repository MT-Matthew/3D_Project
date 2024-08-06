using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{

    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;

    public float attackRange = 2f;
    public float detectRange = 10f;
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float attackDelay = 2f;
    float counter;

    private bool isPlayerInRange;
    private bool isPlayerDetected;

    bool isDead = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        // player = GameObject.FindGameObjectWithTag("Player").transform;
        player = PlayerState.Instance.playerBody.transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        isPlayerInRange = distanceToPlayer <= attackRange;
        isPlayerDetected = distanceToPlayer <= detectRange;

        counter += Time.deltaTime;

        isDead = GetComponent<Animal>().isDead;

        if (isDead == false)
        {
            if (isPlayerInRange)
            {
                if (counter >= attackDelay)
                {
                    counter = 0;
                    AttackPlayer();
                }
            }
            else if (isPlayerDetected)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();
            }
        }
    }

    private void Patrol()
    {
        // Idle or walk randomly
        if (!agent.hasPath)
        {
            // Random patrol points logic here
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 10f, 1);
            Vector3 finalPosition = hit.position;

            agent.destination = finalPosition;
            agent.speed = walkSpeed;
            animator.SetTrigger("IdleChest");
        }
        else
        {
            animator.SetTrigger("WalkFWD");
            animator.SetBool("isRunning", false);
        }
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        transform.LookAt(player);

        agent.SetDestination(player.position);
        agent.speed = runSpeed;

        // UnityEngine.Debug.Log("Position: " + player.position + "  Speed: " + agent.speed);

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetTrigger("Run");
        }
        else
        {
            animator.SetTrigger("IdleBattle");
            animator.SetBool("isRunning", true);
        }
    }

    private void AttackPlayer()
    {
        agent.isStopped = true;
        transform.LookAt(player);

        // Randomly choose an attack animation
        int attackAnim = Random.Range(0, 2);
        if (attackAnim == 0)
        {
            animator.SetTrigger("Attack01");
        }
        else
        {
            animator.SetTrigger("Attack02");
        }
    }

    public void TakeDamage()
    {
        animator.SetTrigger("GetHit");
    }

    public void Die()
    {
        animator.SetBool("isDead", true);
        animator.SetTrigger("Die");
        // Disable further actions
        isDead = true;
        agent.enabled = false;
        this.enabled = false;
        StartCoroutine(Remove());
    }

    public IEnumerator Remove()
    {
        yield return new WaitForSeconds(3.2f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
