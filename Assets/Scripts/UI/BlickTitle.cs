using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlickTitle : MonoBehaviour
{
    public TextMeshPro textMeshPro;       // 3D TextMeshPro 컴포넌트
    public float minInterval = 2f;       // 깜빡임 간격 최소값 (초)
    public float maxInterval = 5f;       // 깜빡임 간격 최대값 (초)
    public int minFlickers = 1;          // 한 번에 최소 깜빡임 횟수
    public int maxFlickers = 3;          // 한 번에 최대 깜빡임 횟수
    public float minAlpha = 0.3f;        // 최소 투명도
    public float maxAlpha = 0.8f;        // 최대 투명도
    public float flickerDuration = 0.1f; // 깜빡임 지속 시간 (초)

    private Color originalColor;

    void Start()
    {
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro 컴포넌트를 연결하세요!");
            return;
        }

        // 원래 색상 저장
        originalColor = textMeshPro.color;

        // 깜빡임 시작
        StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // 랜덤한 대기 시간 (2~5초)
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // 랜덤한 깜빡임 횟수 (1~3회)
            int flickerCount = Random.Range(minFlickers, maxFlickers + 1);
            for (int i = 0; i < flickerCount; i++)
            {
                // 랜덤 투명도 설정
                float randomAlpha = Random.Range(minAlpha, maxAlpha);
                textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, randomAlpha);

                // 깜빡임 지속 시간
                yield return new WaitForSeconds(flickerDuration);

                // 원래 색상으로 복원
                textMeshPro.color = originalColor;

                // 깜빡임 사이 간격 (작게 설정)
                yield return new WaitForSeconds(flickerDuration);
            }
        }
    }
}
