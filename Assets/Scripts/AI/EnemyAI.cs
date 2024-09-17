using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(Player.transform.position);

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            Attack();
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
}
