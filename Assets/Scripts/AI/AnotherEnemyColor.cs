using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherEnemyColor : MonoBehaviour
{
    public Color overlayColor; // 원하는 오버레이 색상
    private Material material;

    void Start()
    {
        // 메터리얼 가져오기
        material = GetComponent<Renderer>().material;
        material.color = overlayColor;
    }

    public void ChangeColor(Color newColor)
    {
        material.color = newColor;
    }
}
