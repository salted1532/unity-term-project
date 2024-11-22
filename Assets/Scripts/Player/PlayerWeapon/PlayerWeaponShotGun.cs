
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditorInternal.ReorderableList;
public class PlayerWeaponShotGun : MonoBehaviour
{
    public bool isReLoading;

    public int MaxBulletCount = 5;

    public int curBoulletCount = 5;

    public float maxReLodingTime = 3.8f;

    public float curReLodingTime;

    public float ShotGunMaxDistance = 100f;
    public float damage = 8f;
    public GameObject PreFebBullet;
    public GameObject bulletMarks;  
    public Transform bulletT;
    public float CooldownTime = 0.65f;
    int ShotBulletCount = 10;
    float spreadRadius = PlayerState.PlayerIsZooming ? 300f : 450f;
    public float spreadAngle = 10f;    // ���� ����
    public void HitScanShotGun()
    {
        Debug.Log("�߻�");
        Instantiate(PreFebBullet, bulletT);

        for (int i = 0; i < ShotBulletCount; ++i)
            {
            Vector2 randomCircle = Random.insideUnitCircle * spreadRadius; // ��ũ�� ���� �� ���� ���� ��
            Vector2 screenPoint = new Vector2(Screen.width / 2f + randomCircle.x, Screen.height / 2f + randomCircle.y);

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);

            if (Physics.Raycast(ray.origin, ray.direction, out hit, ShotGunMaxDistance, ~((1 << 7) | (1 << 9))))
                {
                Instantiate(bulletMarks, hit.point, Quaternion.LookRotation(hit.normal));
                Debug.Log("��Ʈ�� ��ü : " + hit.collider.name);


                if (hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.GetComponent<EnemyHealth>().EnemyTakeDamage(damage);
                    }



                }
            }
        curBoulletCount -= 1;
        if (curBoulletCount <= 0)
        {
            curBoulletCount = MaxBulletCount;

        }
        Debug.Log("���� �Ѿ� �߻� ����!");

    }

}
