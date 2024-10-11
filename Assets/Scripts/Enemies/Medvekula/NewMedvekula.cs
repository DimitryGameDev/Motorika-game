using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMedvekula : Enemy
{
    // public GameObject projectilePrefab; // Префаб снаряда
    private Turret turret;
    public Transform player; // Игрок
    public float shootingInterval = 2f; // Интервал между выстрелами
    public float teleportDistance = 2f; // Расстояние для телепортации
    public float shootingRange = 10f; // Дальность стрельбы
    public float teleportDuration = 1f;
    private float nextShootTime;
    private float timer;
    [SerializeField] private float lifetime;
    public float Lifetime => lifetime;
    [SerializeField] private float teleportTime;
    private void Start()
    {
        timer = teleportTime;
        turret = GetComponentInChildren<Turret>();

    }
    private void Update()
    {
        // Проверяем расстояние до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        ShootAtPlayer();
        // Если игрок в пределах досягаемости и время для следующего выстрела
       
        Debug.Log(IsPlayerBehind());
        // Проверка на то, что игрок за спиной
        if (IsPlayerBehind())
        {
            timer -= Time.deltaTime;
            if (timer <= 0) NextPositionCalculation();
             
     
        }
        else
        {
            timer = teleportTime;
        }
    }

    private void ShootAtPlayer()
    {
        //Vector3 direction = -(player.position - transform.position).normalized;
        //direction.y = 0; // Убедимся, что снаряд движется только по плоскости XY

        //GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward*1, Quaternion.identity);
        if (turret != null) turret.Fire();
        //projectile.transform.position += transform.forward*ProjectileSpeed*Time.deltaTime; // Задайте скорость снаряда
    }

    private bool IsPlayerBehind()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        return directionToPlayer.z > 2; // Проверяем, находится ли игрок за спиной по оси Z
    }

    private void TeleportTimer()
    {
        

    

      

        NextPositionCalculation();

        // Сохраняем текущую координату X


        //  transform.position = targetPosition;
        // Убедитесь, что в конце точно нацелились
    }
    private void NextPositionCalculation()
    {
        
        Vector3 targetPosition = player.position + new Vector3 (0, 0, 1) * teleportDistance;

      

        targetPosition.y = transform.position.y; // Сохраняем текущую высоту противника
        targetPosition.x = transform.position.x; // Сохраняем текущую координату X
        transform.position = targetPosition;
        return;
    }
}