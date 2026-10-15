using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private static Transform playerTransform; // Ссылка статик, чтобы искать игрока один раз для всех монет

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }

    void Update()
    {
        // Вращаем монетку
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        // Автоудаление позади игрока (используем кэшированную позицию)
        if (playerTransform != null)
        {
            if (transform.position.z < playerTransform.position.z - 20f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddCoin();
            }
            Destroy(gameObject);
        }
    }
}