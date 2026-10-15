using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;

    [Header("Score & Speed Settings")]
    public float scoreMultiplier = 10f;
    private float currentScore = 0f;
    private bool isGameActive = true;

    [Header("Speed Progression")]
    public float baseSpeed = 10f;       // Начальная скорость бега
    public float maxSpeed = 25f;        // Максимальная скорость
    public float speedIncreaseRate = 0.05f; // Как быстро растет скорость со временем

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!isGameActive) return;

        currentScore += Time.deltaTime * scoreMultiplier;

        if (scoreText != null)
        {
            scoreText.text = "Очки: " + Mathf.FloorToInt(currentScore).ToString();
        }
    }

    // Метод, который говорит игроку, с какой скоростью сейчас нужно бежать
    public float GetCurrentSpeed()
    {
        // Скорость плавно растет на основе набранных очков, но не превышает maxSpeed
        float calculatedSpeed = baseSpeed + (currentScore * speedIncreaseRate);
        return Mathf.Min(calculatedSpeed, maxSpeed);
    }

    public void StopScore()
    {
        isGameActive = false;
    }

    public int GetFinalScore()
    {
        return Mathf.FloorToInt(currentScore);
    }
}