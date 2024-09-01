using UnityEngine;

public class ProjectilePorcupine : MonoBehaviour
{
    [SerializeField] private float lifetime;        
    [SerializeField] protected int damage;         

    private float speed;                            

    private float timer;                            // Таймер для отслеживания времени жизни снаряда
    private Vector3 direction;                      // Направление движения снаряда
    private Destructable parent;                    // Объект, который выпустил снаряд (дикобраз)

    // Публичный метод для установки скорости снаряда
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
        // Устанавливаем начальное направление снаряда
        transform.forward = direction;
    }

    private void Update()
    {
        RaycastHit hit;

        // Длина шага на каждом кадре
        float stepLength = Time.deltaTime * speed;
        Vector3 step = direction * stepLength;

        // Отрисовка трассировки луча для отладки
        Debug.DrawRay(transform.position, direction * stepLength, Color.green);

        // Проверка столкновения с объектом на пути движения снаряда
        if (Physics.Raycast(transform.position, direction, out hit, stepLength))
        {
            OnHit(hit);
            OnProjectileLifeEnd(hit.collider, hit.point);
        }

        // Обновление таймера времени жизни
        timer += Time.deltaTime;

        // Уничтожение снаряда после окончания времени жизни
        if (timer > lifetime)
            Destroy(gameObject);

        // Перемещение снаряда в указанном направлении
        transform.position += step;
    }

    // Метод обработки попадания снаряда в объект
    protected virtual void OnHit(RaycastHit hit)
    {
        var destructible = hit.collider.transform.root.GetComponent<Destructable>();

        // Нанесение урона объекту, если он имеет компонент Destructable и не является "родителем" снаряда
        if (destructible != null && destructible != parent)
        {
            destructible.ApplyDamage(damage);
        }
    }

    // Метод обработки конца жизни снаряда (например, при столкновении)
    private void OnProjectileLifeEnd(Collider collider, Vector3 pos)
    {
        Destroy(gameObject);
    }

    // Метод установки родительского объекта (дикобраза), который выпустил снаряд
    public void SetParentShooter(Destructable parent)
    {
        this.parent = parent;
    }
}
