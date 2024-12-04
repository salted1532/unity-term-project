using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class BlickTitle : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public float minInterval = 0.5f; // 최소 간격
    public float maxInterval = 1f; // 최대 간격
    public int minFlickers = 2;      // 최소 깜빡임 횟수
    public int maxFlickers = 5;      // 최대 깜빡임 횟수
    public float minAlpha = 0.1f;    // 최소 투명도
    public float maxAlpha = 1f;      // 최대 투명도
    public float flickerDuration = 0.05f; // 깜빡임 지속 시간

    public string currentSceneName;

    public bool isGameOver;

    private Color originalColor;
    private Coroutine flickerCoroutine;

    void Start()
    {
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro 컴포넌트를 연결하세요!");
            enabled = false; // 스크립트 비활성화
            return;
        }

        isGameOver = false;
        currentSceneName = SceneManager.GetActiveScene().name;
        originalColor = textMeshPro.color; // 원래 색상 저장

        if(currentSceneName == "SampleScene")
        {
            textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        }
    }

    public void StartBlink()
    {
        flickerCoroutine = StartCoroutine(FlickerRoutine());
    }

    void OnEnable()
    {
        if(currentSceneName == "Main Menu")
        {
            StartBlink();
        }
    }

    void OnDisable()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }
    }

    public void GameOverStart_BlickTitle()
    {
        isGameOver = true;
    }

    public bool isGameOver_BlickTitle()
    {
        return isGameOver;
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

