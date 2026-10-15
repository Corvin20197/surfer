using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // Вращаем монетку
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        // Если игрок уехал вперед больше чем на 20 метров от монетки, удаляем её, чтобы не засорять память
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
            Debug.Log("Монетка собрана!");
            Destroy(gameObject);
        }
    }
}