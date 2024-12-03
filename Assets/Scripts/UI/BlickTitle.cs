using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlickTitle : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public float minInterval = 2f;
    public float maxInterval = 5f;
    public int minFlickers = 1;
    public int maxFlickers = 3;
    public float minAlpha = 0.3f;
    public float maxAlpha = 0.8f;
    public float flickerDuration = 0.1f;

    private Color originalColor;
    private Coroutine flickerCoroutine;

    void Start()
    {
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro 컴포넌트를 연결하세요!");
            return;
        }

        originalColor = textMeshPro.color; // 원래 색상 저장
    }

    void OnEnable()
    {
        // 오브젝트가 활성화될 때 코루틴 시작
        flickerCoroutine = StartCoroutine(FlickerRoutine());
    }

    void OnDisable()
    {
        // 오브젝트가 비활성화될 때 코루틴 중지
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            int flickerCount = Random.Range(minFlickers, maxFlickers + 1);
            for (int i = 0; i < flickerCount; i++)
            {
                float randomAlpha = Random.Range(minAlpha, maxAlpha);
                textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, randomAlpha);
                yield return new WaitForSeconds(flickerDuration);
                textMeshPro.color = originalColor;
                yield return new WaitForSeconds(flickerDuration);
            }
        }
    }
}
