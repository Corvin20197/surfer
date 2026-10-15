using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject gameOverPanel; // Перетащи сюда GameOverPanel из Hierarchy

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        Time.timeScale = 1f; // На старте время идет нормально
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Останавливаем время игры
        Time.timeScale = 0f;

        // Показываем панель проигрыша
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // Метод для кнопки рестарта
    public void RestartGame()
    {
        Time.timeScale = 1f; // Обязательно возвращаем время перед перезагрузкой!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}