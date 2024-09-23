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

    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public Transform firePoint; // 총알이 발사될 위치
    public float bulletSpeed = 20f; // 총알 속도
    public float attackCooldown = 3f; // 공격 쿨다운 시간

    public int waypointCount = 3;   // 웨이포인트 개수 (3개)
    public float waypointRadius = 3f;  // 각 웨이포인트의 반경
    private Vector3[] waypoints;  // 생성된 웨이포인트 배열
    private int currentWaypointIndex = 0;

    private int turnDirection;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        waypoints = new Vector3[waypointCount];

        // 적이 랜덤하게 왼쪽으로 돌지, 오른쪽으로 돌지 결정
        turnDirection = Random.Range(0, 2) == 0 ? -1 : 1;

        GenerateWaypoints();  // 첫 웨이포인트 세트 생성
        nav.SetDestination(waypoints[0]);

    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            Debug.LogWarning("플레이어가 설정되지 않았습니다.");
            return;
        }

        // 현재 웨이포인트에 도착했는지 확인
        if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance)
        {
            currentWaypointIndex++;

            // 모든 웨이포인트에 도달했으면 플레이어로 직접 이동
            if (currentWaypointIndex >= waypoints.Length)
            {
                nav.SetDestination(Player.transform.position);  // 플레이어로 이동
            }
            else
            {
                nav.SetDestination(waypoints[currentWaypointIndex]);  // 다음 웨이포인트로 이동
            }
        }

    }

    // 플레이어 주변에서 랜덤한 3개의 웨이포인트 생성
    void GenerateWaypoints()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);  // 적과 플레이어 사이 거리
        Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;  // 적에서 플레이어로 향하는 방향

        for (int i = 0; i < waypointCount; i++)
        {
            // 적과 플레이어 사이의 1/3, 2/3 지점 근처에 웨이포인트 생성
            float fraction = (i + 1) / (float)(waypointCount + 1);  // 1/3, 2/3 등의 비율 계산
            Vector3 baseWaypointPosition = transform.position + directionToPlayer * (distanceToPlayer * fraction);

            // 방향에 따라 웨이포인트를 적절히 회전시킴 (왼쪽/오른쪽)
            Vector3 perpendicularOffset = Vector3.Cross(Vector3.up, directionToPlayer) * waypointRadius * turnDirection;

            // 해당 지점에서 랜덤한 반경 내 좌표 선택
            Vector3 randomOffset = Random.insideUnitSphere * waypointRadius;
            randomOffset.y = 0;  // Y축은 변경하지 않음 (2D 평면 이동)

            // 최종 웨이포인트 위치
            Vector3 finalWaypointPosition = baseWaypointPosition + perpendicularOffset + randomOffset;

            // NavMesh 상의 유효한 위치로 설정
            NavMeshHit hit;
            if (NavMesh.SamplePosition(finalWaypointPosition, out hit, waypointRadius, NavMesh.AllAreas))
            {
                waypoints[i] = hit.position;
            }
            else
            {
                waypoints[i] = baseWaypointPosition;  // 유효하지 않으면 기본 위치 사용
            }
        }
    }


    public void Attack()
    {
        nav.isStopped = true;

        if (!alreadyAttacked)
        {
            Vector3 direction = Player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1000f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), 3f);

             ShootBullet();
        }
    }

    private void ShootBullet()
    {
        // 총알 오브젝트를 firePoint 위치에서 소환
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 총알에 Rigidbody 컴포넌트가 있다면, 앞으로 발사
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed; // 앞 방향으로 발사
        }
    }

    private void ResetAttack()
    {
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
