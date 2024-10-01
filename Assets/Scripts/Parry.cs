using UnityEngine;
using UnityEngine.Events;

public class Parry : MonoSingleton<Parry>
{
    public event UnityAction AttackAnimEvent;
    public event UnityAction IdleAnimEvent;

    [SerializeField] private float parryWindow;
    [SerializeField] private float parryForce;
    [SerializeField] private int playerDamage;

    private Player player;
    private Destructible playerDestructible;

    private Enemy enemy;
    private int enemyDamage;

    private float parryTimer;

    private bool isParry = false;
    private bool enemyIsActive = false;

    private void Start()
    {
        player = GetComponent<Player>();
        playerDestructible = GetComponent<Destructible>();

        parryTimer = parryWindow;
    }

    private void Update()
    {
        ParryTime();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider == null) return;

        enemy = collider.GetComponent<Enemy>();

        if (enemy == null) return;
        enemyDamage = enemy.Damage;

        if (enemy.Parried == Parried.On && !enemyIsActive)
        {
            enemyIsActive = true;
            parryTimer = parryWindow;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (enemy != null && enemyIsActive && !isParry && parryTimer > 0)
        {
            if (Input.GetKey(KeyCode.W))
            {
                PlayerParry();
            }
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        IdleAnimEvent?.Invoke();

        ResetParry();
    }

    private void PlayerParry()
    {
        enemy.ApplyDamage(playerDamage);
        Debug.Log("Успешное парирование, враг получает урон.");

        if (enemy.HitPoints > 0)
            player.Parry(parryForce);
        else
            Debug.Log("Враг уничтожен, продолжаем движение.");

        isParry = true;
        ResetParry();
    }

    private void ApplyEnemyDamage()
    {
        AttackAnimEvent?.Invoke();

        playerDestructible.ApplyDamage(enemyDamage);
        Debug.Log("Не успели парировать. Враг наносит урон.");

        if (playerDestructible.HitPoints > 0)
            player.Parry(parryForce);
    }

    public void ParryTime()
    {
        if (enemyIsActive)
        {
            parryTimer -= Time.deltaTime;

            if (parryTimer <= 0 && !isParry)
            {
                ApplyEnemyDamage();
                ResetParry();
            }
        }
    }

    private void ResetParry()
    {
        isParry = false;
        enemyIsActive = false;

        parryTimer = 0;
    }
}