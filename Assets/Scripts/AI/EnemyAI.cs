using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject Player;
    public GameObject gameObject;

    private NavMeshAgent nav;

    private bool alreadyAttacked = false;

    public GameObject bulletPrefab; // �߻��� �Ѿ� ������
    public Transform firePoint; // �Ѿ��� �߻�� ��ġ
    public float bulletSpeed = 20f; // �Ѿ� �ӵ�
    public float attackCooldown = 3f; // ���� ��ٿ� �ð�

    private int waypointCount = 1;   // ��������Ʈ ����
    private float waypointRadius = 5f;  // �� ��������Ʈ�� �ݰ�
    private Vector3[] waypoints;  // ������ ��������Ʈ �迭
    private int currentWaypointIndex = 0;

    private int turnDirection;
    private float waypointArrivalTime;
    private float timeLimit = 3f;

    public Animator animator;

    private float currentTime = 0f;
    private int randomSoundTime;

    // Start is called before the first frame update
    void Start()
    {
        randomSoundTime = Random.Range(4,8);
        nav = GetComponent<NavMeshAgent>();
        waypoints = new Vector3[waypointCount];

        // ���� �����ϰ� �������� ����, ���������� ���� ����
        turnDirection = Random.Range(0, 4) == 0 ? -1 : 1;

        GenerateWaypoints();  // ù ��������Ʈ ��Ʈ ����
        nav.SetDestination(waypoints[0]);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime>randomSoundTime) {
            SoundManager.Instance.PlaySound3D("MON_FacelessOne_v2_attack_" + Random.Range(1,7), gameObject);
            currentTime = 0;
        }
        if (Player == null)
        {
            Debug.LogWarning("�÷��̾ �������� �ʾҽ��ϴ�.");
            return;
        }

        // ���� ��������Ʈ�� �����߰ų� 3�ʰ� ������
        if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance || Time.time - waypointArrivalTime >= timeLimit)
        {

            if (!alreadyAttacked)
            {
                animator.SetTrigger("Walk");
                // ���ο� ��������Ʈ ���� �� ����
                GenerateWaypoints();
                nav.SetDestination(waypoints[currentWaypointIndex]);

                // ���ο� ��������Ʈ�� �̵� �� �ð��� �ʱ�ȭ
                waypointArrivalTime = Time.time;
            }
        }
    }

    // �÷��̾� �ֺ����� ������ 3���� ��������Ʈ ����
    void GenerateWaypoints()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);  // ���� �÷��̾� ���� �Ÿ�
        Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;  // ������ �÷��̾�� ���ϴ� ����

        for (int i = 0; i < waypointCount; i++)
        {
            // ���� �÷��̾� ������ 1/3, 2/3 ���� ��ó�� ��������Ʈ ����
            float fraction = (i + 1) / (float)(waypointCount + 1);  // 1/3, 2/3 ���� ���� ���
            Vector3 baseWaypointPosition = transform.position + directionToPlayer * (distanceToPlayer * fraction);

            // ���⿡ ���� ��������Ʈ�� ������ ȸ����Ŵ (����/������)
            Vector3 perpendicularOffset = Vector3.Cross(Vector3.up, directionToPlayer) * waypointRadius * turnDirection;

            // �ش� �������� ������ �ݰ� �� ��ǥ ����
            Vector3 randomOffset = Random.insideUnitSphere * waypointRadius;
            randomOffset.y = 0;  // Y���� �������� ���� (2D ��� �̵�)

            // ���� ��������Ʈ ��ġ
            Vector3 finalWaypointPosition = baseWaypointPosition + perpendicularOffset + randomOffset;

            // NavMesh ���� ��ȿ�� ��ġ�� ����
            NavMeshHit hit;
            if (NavMesh.SamplePosition(finalWaypointPosition, out hit, waypointRadius, NavMesh.AllAreas))
            {
                waypoints[i] = hit.position;
            }
            else
            {
                waypoints[i] = baseWaypointPosition;  // ��ȿ���� ������ �⺻ ��ġ ���
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
            //ShootBullet();
        }
    }

    public void ShootBullet()
    {
        SoundManager.Instance.PlaySound3D("FireCast", gameObject);
        Vector3 direction = Player.transform.position - transform.position;
        direction.y -= 2;  // Y ���� -2 ����
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1000f);
        // �Ѿ� ������Ʈ�� firePoint ��ġ���� ��ȯ
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, targetRotation);

        // �Ѿ˿� Rigidbody ������Ʈ�� �ִٸ�, ������ �߻�
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed; // �� �������� �߻�
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
