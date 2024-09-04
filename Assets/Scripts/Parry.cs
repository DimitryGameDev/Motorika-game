
using System.Collections;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public float parryRange = 0.1f;
    public float parryWindow = 0.2f;
    public float parryForce = 10f;
    public float parryDamage = 10f;
  
    private Destructible destructible;
    private float parryTimer = 1;
    public float ParryTimer => parryTimer;
    private bool isParry;

    [SerializeField] private float damageToPlayer;
    [SerializeField] private float damageToEnemy;
    [SerializeField] private LayerMask layerMask;
    public float knockbackForce = 5.0f;

    public Material parryMaterial;
    // Сила отталкивания
   


    private bool hasParried = false;
    private void Start()
    {
        destructible = GetComponent<Destructible>();
    }
    private void Update()
    {
        CheckForEnemiesAndParry();
       
    }
    


    
   

    private void CheckForEnemiesAndParry()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, parryRange, layerMask);

        foreach (Collider enemy in enemies)
        {
            if (!hasParried)
            {
                StartCoroutine(ParryTime(enemy));
            }
        }
    }

    private IEnumerator ParryTime(Collider enemy)
    {
        float elapsedTime = 0f;
        hasParried = false;

        while (elapsedTime < parryTimer)
        {
            if (hasParried)
            {
                DealDamageToEnemy(enemy);
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!hasParried)
        {
            ReceiveDamageFromEnemy(enemy);
        }
    }

    private void DealDamageToEnemy(Collider enemy)
    {
        Debug.Log("Успешное парирование! Наносим урон врагу.");
        if (Input.GetKeyDown(KeyCode.S)) // Замените на нужную кнопку
        {
            var _destructible = enemy.GetComponent<Destructible>();
            hasParried = true;
            _destructible.ApplyDamage(50);
        }
        // Логика нанесения урона врагу
        
    }

    private void ReceiveDamageFromEnemy(Collider enemy)
    {
        Debug.Log("Не успели парировать. Враг наносит урон.");
        // Логика получения урона от врага
        destructible.ApplyDamage(50);
        // Отталкивание игрока
        Vector3 knockbackDirection = (transform.position - enemy.transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(-transform.forward * knockbackForce, ForceMode.Impulse);
    }
}


