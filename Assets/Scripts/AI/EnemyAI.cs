using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject Player;

    private NavMeshAgent nav;

    private bool alreadyAttacked = false;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float attackCooldown = 3f;

    private int waypointCount = 1;
    private float waypointRadius = 5f;
    private Vector3[] waypoints;
    private int currentWaypointIndex = 0;

    private int turnDirection;
    private float waypointArrivalTime;
    private float timeLimit = 3f;

    public Animator animator;

    private float currentTime = 0f;
    private double randomSoundTime;

    public int whatEnemyis = 0;

    // Start is called before the first frame update
    void Start()
    {
        randomSoundTime = Random.Range(40,80) * 0.1;
        nav = GetComponent<NavMeshAgent>();
        waypoints = new Vector3[waypointCount];

        turnDirection = Random.Range(0, 4) == 0 ? -1 : 1;

        GenerateWaypoints();
        nav.SetDestination(waypoints[0]);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime>randomSoundTime) {
            SoundManager.Instance.PlaySound3D("MON_FacelessOne_v2_attack_" + Random.Range(1,7), gameObject, 0, 25, false, SoundType.MONSTER_SOUND);
            currentTime = 0;
        }
        if (Player == null)
        {
            Debug.LogWarning("Enemy can't find Player!!");
            return;
        }

        if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance || Time.time - waypointArrivalTime >= timeLimit)
        {

            if (!alreadyAttacked)
            {
                animator.SetTrigger("Walk");

                GenerateWaypoints();
                nav.SetDestination(waypoints[currentWaypointIndex]);


                waypointArrivalTime = Time.time;
            }
        }
    }

    void GenerateWaypoints()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;

        for (int i = 0; i < waypointCount; i++)
        {

            float fraction = (i + 1) / (float)(waypointCount + 1);
            Vector3 baseWaypointPosition = transform.position + directionToPlayer * (distanceToPlayer * fraction);


            Vector3 perpendicularOffset = Vector3.Cross(Vector3.up, directionToPlayer) * waypointRadius * turnDirection;


            Vector3 randomOffset = Random.insideUnitSphere * waypointRadius;
            randomOffset.y = 0;  


            Vector3 finalWaypointPosition = baseWaypointPosition + perpendicularOffset + randomOffset;


            NavMeshHit hit;
            if (NavMesh.SamplePosition(finalWaypointPosition, out hit, waypointRadius, NavMesh.AllAreas))
            {
                waypoints[i] = hit.position;
            }
            else
            {
                waypoints[i] = baseWaypointPosition;  
            }
        }
    }


    public void Attack()
    {
        nav.isStopped = true;
        animator.SetTrigger("Idle");

        if (!alreadyAttacked)
        {
            Vector3 direction = Player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1000f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), 3f);

            animator.SetTrigger("Attack");
            if(whatEnemyis == 1)
            {
                ShootBullet();
            }

        }
    }

    public void ShootBullet()
    {
        SoundManager.Instance.PlaySound3D("FireCast", gameObject);
        Vector3 direction = Player.transform.position - transform.position;
        direction.y -= 2;  
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1000f);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, targetRotation);


        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }
    }

    private void ResetAttack()
    {
        Vector3 direction = Player.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1000f);
        alreadyAttacked = false;
        nav.isStopped = false;
    }

    public void PathFinding()
    {
        if (!alreadyAttacked) 
        {
            nav.SetDestination(Player.transform.position);
        }
    }
 }
