using UnityEngine;
using TMPro; // Убедись, что подключен TextMeshPro (если используешь его)

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;   // Текст очков
    public TextMeshProUGUI coinText;    // Текст монет (перетащи сюда свой CoinText)

    [Header("Game Stats")]
    private float score = 0f;
    private int coinCount = 0;

    [Header("Speed Settings")]
    public float baseSpeed = 10f;
    public float maxSpeed = 25f;
    public float accelerationRate = 0.2f; // Скорость роста сложности

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // Увеличиваем очки за пройденное расстояние
        score += GetCurrentSpeed() * Time.deltaTime;

        if (scoreText != null)
        {
            scoreText.text = "Очки: " + Mathf.FloorToInt(score);
        }
    }

    // Метод для получения текущей динамической скорости (используется в PlayerController)
    public float GetCurrentSpeed()
    {
        // Скорость плавно растет со временем
        float current = baseSpeed + (score * 0.01f * accelerationRate);
        return Mathf.Min(current, maxSpeed);
    }

    // Метод для добавления монетки (вызывается из скрипта Coin.cs)
    public void AddCoin()
    {
        coinCount++;
        if (coinText != null)
        {
            coinText.text = "Монеты: " + coinCount;
        }
    }
}