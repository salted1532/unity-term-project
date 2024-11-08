using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBossAi : MonoBehaviour
{
    public GameObject Player;

    private NavMeshAgent nav;

    private bool alreadyAttacked = false;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float attackCooldown = 0f;

    private int waypointCount = 3;
    private float waypointRadius = 5f;
    private Vector3[] waypoints;
    private int currentWaypointIndex = 0;

    private int turnDirection;
    private float waypointArrivalTime;
    private float timeLimit = 3f;

    public Animator animator;

    private float currentTime = 0f;
    private double randomSoundTime;

    private int generateCount = 0;

    private float Patterncooltime = 0f;
    private bool PatternOn = false;

    // Start is called before the first frame update
    void Start()
    {
        randomSoundTime = Random.Range(40, 80) * 0.1;
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
        Patterncooltime += Time.deltaTime;

        if (currentTime > randomSoundTime)
        {
            SoundManager.Instance.PlaySound3D("MON_FacelessOne_v2_attack_" + Random.Range(1, 7), gameObject, 0, 25, false, SoundType.MONSTER_SOUND);
            currentTime = 0;
        }
        if (Player == null)
        {
            Debug.LogWarning("Enemy can't find Player!!");
            return;
        }

        if(Patterncooltime > 5f)
        {
            PatternOn = true;
        }

        if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance )//|| Time.time - waypointArrivalTime >= timeLimit)
        {
            if(PatternOn == false)
            {
                if (!alreadyAttacked)
                {
                    nav.speed = 3.5f;

                    animator.SetTrigger("Walk");

                    nav.SetDestination(waypoints[currentWaypointIndex]);
                    GenerateWaypoints();



                    waypointArrivalTime = Time.time;
                }
            }
            else if(PatternOn == true) 
            {
                if (!alreadyAttacked)
                {
                    nav.speed = 50f;

                    animator.SetTrigger("Walk");

                    nav.SetDestination(waypoints[currentWaypointIndex]);
                    PatternGenerateWaypoints();

                    waypointArrivalTime = Time.time;
                }
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
    void PatternGenerateWaypoints()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;

        // distanceToPlayer를 3등분한 값을 waypointRadius로 설정
        float waypointRadius = distanceToPlayer / 3;

        for (int i = 0; i < waypointCount; i++)
        {
            float fraction = (i + 1) / (float)(waypointCount + 1);
            Vector3 baseWaypointPosition = transform.position + directionToPlayer * (distanceToPlayer * fraction);

            // 방향에 수직한 오프셋을 생성
            Vector3 perpendicularOffset = Vector3.Cross(Vector3.up, directionToPlayer) * waypointRadius * turnDirection;

            // 거리 안에서 랜덤한 좌표를 생성하고 y축은 0으로 고정
            Vector3 randomOffset = Random.insideUnitSphere.normalized * waypointRadius;
            randomOffset.y = 0;

            // 최종 위치 계산
            Vector3 finalWaypointPosition = baseWaypointPosition + perpendicularOffset + randomOffset;

            // NavMesh 상의 유효한 위치로 설정
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

        // 함수가 호출될 때마다 카운터 증가
        generateCount++;

        // 호출 횟수가 3에 도달하면 "3번 완료" 메시지 출력 후 카운터 초기화
        if (generateCount == 3)
        {
            Debug.Log("3번 완료");
            Attack();
            Invoke(nameof(Attack), attackCooldown + 0.2f);
            Invoke(nameof(Attack), attackCooldown + 0.4f);

            generateCount = 0;

            PatternOn = false;
            Patterncooltime = 0f;
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
            Invoke(nameof(ResetAttack), attackCooldown);

            animator.SetTrigger("Attack");
            //ShootBullet();
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
