using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherEnemyColor : MonoBehaviour
{
    public Color overlayColor; // ���ϴ� �������� ����
    private Material material;

    void Start()
    {
        // ���͸��� ��������
        material = GetComponent<Renderer>().material;
        material.color = overlayColor;
    }

    public void ChangeColor(Color newColor)
    {
        material.color = newColor;
    }
}
