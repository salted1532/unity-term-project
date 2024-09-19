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

    public GameObject bulletPrefab; // �߻��� �Ѿ� ������
    public Transform firePoint; // �Ѿ��� �߻�� ��ġ
    public float bulletSpeed = 20f; // �Ѿ� �ӵ�
    public float attackCooldown = 3f; // ���� ��ٿ� �ð�

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

            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            float distance = Vector3.Distance(transform.position, Player.transform.position);

            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 1f);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    ShootBullet();
                }
                else
                {

                }
            }
        }
    }

    private void ShootBullet()
    {
        // �Ѿ� ������Ʈ�� firePoint ��ġ���� ��ȯ
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // �Ѿ˿� Rigidbody ������Ʈ�� �ִٸ�, ������ �߻�
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed; // �� �������� �߻�
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        nav.isStopped = false;
    }
}
