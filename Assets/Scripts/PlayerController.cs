using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    private Animator animator;
    private Vector3 normalCenter;

    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int SlideHash = Animator.StringToHash("Slide");

    private Vector3 moveDirection;

    [Header("Movement Settings")]
    public float laneDistance = 2.5f; // Расстояние между линиями (-2.5, 0, 2.5)
    private int targetLane = 1; // 0 - левая, 1 - центр, 2 - правая

    public float jumpForce = 6f;     // Сила прыжка
    public float gravity = 20f;      // Гравитация
    public float laneChangeSpeed = 15f; // Скорость смены полосы

    [Header("Slide Settings")]
    private bool isSliding = false;
    public float slideDuration = 0.5f; // Длительность подката
    private float normalHeight;
    public float slideHeight = 0.5f;   // Высота коллайдера при подкате

    private Vector3 normalScale;
    public float slideScaleY = 0.4f;   // Сплющивание персонажа визуально

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }

        normalHeight = controller.height;
        normalCenter = controller.center;

        // Ищем Animator на модели или её дочерних объектах
        animator = GetComponentInChildren<Animator>();
        normalScale = transform.localScale;
    }

    void Update()
    {
        // 1. Управление сменой полосы (Лево / Право)
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            targetLane--;
            if (targetLane < 0) targetLane = 0;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            targetLane++;
            if (targetLane > 2) targetLane = 2;
        }

        // Вычисляем целевую позицию по X для нужной линии
        float targetX = 0f;
        if (targetLane == 0) targetX = -laneDistance;
        else if (targetLane == 1) targetX = 0f;
        else if (targetLane == 2) targetX = laneDistance;

        // Плавно интерполируем текущую позицию X к целевой
        float currentX = transform.position.x;
        float newX = Mathf.Lerp(currentX, targetX, Time.deltaTime * laneChangeSpeed);

        // 2. Получаем текущую динамическую скорость бега из ScoreManager
        float currentForwardSpeed = 10f;
        if (ScoreManager.Instance != null)
        {
            currentForwardSpeed = ScoreManager.Instance.GetCurrentSpeed();
        }
        float moveZ = currentForwardSpeed;

        // 3. Прыжок, Подкат и гравитация
        if (controller.isGrounded)
        {
            moveDirection.y = -0.5f;

            // Прыжок (W, Стрелка вверх или Пробел)
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !isSliding)
            {
                moveDirection.y = jumpForce;
                if (animator != null)
                {
                    animator.ResetTrigger(SlideHash);
                    animator.SetTrigger(JumpHash);
                }
            }

            // Подкат (S или Стрелка вниз)
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding)
            {
                StartCoroutine(SlideRoutine());
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Вычисляем смещение по X за этот кадр
        float xMoveDelta = newX - currentX;

        // Собираем итоговый вектор движения
        Vector3 finalMove = new Vector3(xMoveDelta / Time.deltaTime, moveDirection.y, moveZ);

        controller.Move(finalMove * Time.deltaTime);
    }

    // Корутина для логики подката
    private IEnumerator SlideRoutine()
    {
        isSliding = true;

        // Уменьшаем коллайдер, сохраняя его нижнюю точку
        controller.height = slideHeight;
        controller.center = new Vector3(
            normalCenter.x,
            normalCenter.y - (normalHeight - slideHeight) / 2f,
            normalCenter.z
        );

        // Запускаем визуальную анимацию подката
        if (animator != null)
        {
            animator.ResetTrigger(JumpHash);
            animator.SetTrigger(SlideHash);
        }

        yield return new WaitForSeconds(slideDuration);

        // Восстанавливаем коллайдер
        controller.height = normalHeight;
        controller.center = normalCenter;

        isSliding = false;
    }

    // 4. Детект столкновения с препятствиями с защитой от ложных срабатываний при спавне/земле
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Игнорируем дорогу и монетки
        if (hit.gameObject.CompareTag("Road") || hit.gameObject.CompareTag("Coin"))
            return;

        // Игнорируем касания земли снизу (приземление или старт игры)
        if (hit.normal.y > 0.5f)
            return;

        // Если врезались в препятствие спереди или сбоку — проигрыш
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}