using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Ссылка на игрока
    public Vector3 offset = new Vector3(0, 3f, -7f); // Смещение камеры относительно игрока
    public float smoothSpeed = 10f; // Скорость следования

    void LateUpdate()
    {
        if (target == null) return;

        // Нам нужно, чтобы камера следовала за игроком по X и Y (или фиксировано по X), 
        // а по Z плавно догоняла его, пока он бежит вперед.
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, target.position.z + offset.z);

        // Опционально: если хочешь, чтобы камера плавно двигалась и по X (при смене линий):
        targetPosition.x = Mathf.Lerp(transform.position.x, target.position.x + offset.x, smoothSpeed * Time.deltaTime);

        // Фиксируем высоту камеры, чтобы прыжки и приседания игрока её не дергали
        targetPosition.y = target.position.y + offset.y;

        transform.position = targetPosition;
    }
}