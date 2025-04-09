using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MobAI : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshAgent agent;

    public Animator animator;

    public Transform player;

    public LayerMask playerLayer;
    public LayerMask ground;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;


    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange;
    public float attackRange;
    public float swingRange;

    public Transform attackPoint;
    bool playerInSightRange;
    bool playerInAttackRange;
    float timeSpent = 0;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }


    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        if (playerInAttackRange)
        {
            playerInSightRange = true;
        }
        else
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        }


        Rotate();
        if (playerInAttackRange)
        {
            Attack();
        }
        else if (playerInSightRange)
        {
            Chase();
        }
        else
        {
            Patroling();
        }
    }

    void Rotate()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2.5f, ground);
        Vector3 forwardDirection = transform.forward;
        Vector3 rightDirection = transform.right;
        Vector3 forwardProjection = Vector3.ProjectOnPlane(forwardDirection, hit.normal).normalized;
        Vector3 rightProjection = Vector3.ProjectOnPlane(rightDirection, hit.normal).normalized;
        forwardDirection = new Vector3(forwardDirection.x, forwardProjection.y, forwardDirection.z);
        transform.forward = forwardDirection;
        transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Asin(rightProjection.y));
    }

    void Chase()
    {
        animator.SetTrigger("run");
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        animator.SetTrigger("stopRunning");
        agent.SetDestination(transform.position);
        //transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("hit");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    bool CheckForPlayer()
    {
        return Physics.CheckSphere(attackPoint.position, swingRange, playerLayer);
    }
    public void HitPlayer(int damage)
    {
        bool inRange = CheckForPlayer();
        if (inRange)
        {
            PlayerHealth health = player.gameObject.GetComponent<PlayerHealth>();
            health.Health = health.Health - damage;
        }
    }

    public void GetHit(int health)
    {
        //TODO
    }


    public void Die()
    {
        GameObject.Instantiate(Resources.Load<GameObject>("DeadBeetle"), transform.position, transform.rotation);
        GameObject.Destroy(gameObject);
    }


    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void Patroling()
    {
        if (!walkPointSet)
        {
            animator.SetTrigger("stopRunning");
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            animator.SetTrigger("run");
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalk = transform.position - walkPoint;
        if (distanceToWalk.magnitude < 1.5f || timeSpent > 5f)
        {
            walkPointSet = false;
            timeSpent = 0;
        }
        timeSpent += Time.deltaTime;

    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        Vector3 shooter = new Vector3(transform.position.x + randomX, transform.position.y + 20f, transform.position.z + randomZ);
        RaycastHit hit;
        Physics.Raycast(shooter, Vector3.down, out hit, 30f, ground);

        NavMeshHit navMeshHit;
        //Physics.Raycast(walkPoint, -transform.up, 2f, ground)
        if (NavMesh.SamplePosition(hit.point, out navMeshHit, 0.2f, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
            walkPointSet = true;
            walkPoint = navMeshHit.position;
        }
    }

}
