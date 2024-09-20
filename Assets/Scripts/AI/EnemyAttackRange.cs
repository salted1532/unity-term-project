using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    public int AttackDamage;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 direction = transform.parent.GetComponent<EnemyAI>().Player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1000f);

            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            float distance = Vector3.Distance(transform.position, transform.parent.GetComponent<EnemyAI>().Player.transform.position);

            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 1f);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    transform.parent.GetComponent<EnemyAI>().Attack();
                }
                else
                {
                    transform.parent.GetComponent<EnemyAI>().PathFinding();
                }
            }
        }
    }
}
