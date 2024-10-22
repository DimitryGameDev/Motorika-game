using UnityEngine;
using UnityEngine.Events;

public class Parry : MonoSingleton<Parry>
{
    public event UnityAction AttackAnimEvent;
    public event UnityAction IdleAnimEvent;

    [SerializeField] private GameObject parrySFXwolf;
    [SerializeField] private GameObject parrySFXezh;
    [SerializeField] private GameObject parrySFXgnom;
    
    [SerializeField] private float parryWindow;
    [SerializeField] private float parryForce;
    [SerializeField] private int playerDamage;

    private Player player;
    private Destructible playerDestructible;

    private Enemy enemy;
    private int enemyDamage;

    private float parryTimer;

    private bool isParry = false;
    public bool IsParry => isParry;
    private bool enemyIsActive = false;

    private void Start()
    {
        player = GetComponent<Player>();
        playerDestructible = GetComponent<Destructible>();

        parryTimer = parryWindow;

        PlayerInputController.Instance.SlideEvent += PlayerParry;
    }

    private void Update()
    {
        ParryTime();
    }

    private void OnDestroy()
    {
        PlayerInputController.Instance.SlideEvent -= PlayerParry;
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
            enemy.ParrySphere(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        IdleAnimEvent?.Invoke();

        if (enemy != null)
            enemy.ParrySphere(false);
        
        //if (!isParry)
        ResetParry();
    }

    private void PlayerParry()
    {
        if (enemy != null && enemyIsActive && !isParry && parryTimer > 0 )
        {
            isParry = true;
            enemy.ApplyDamage(playerDamage);

            if (enemy.HitPoints > 0)
            {
                player.Parry(parryForce);
                
            }

            ResetParry();
            //Invoke(nameof(ResetParry), 1f);
        }
    }

    private void ApplyEnemyDamage()
    {
        AttackAnimEvent?.Invoke();

        playerDestructible.ApplyDamage(enemyDamage);

        if (enemy.TypeSound == Sound.Wolf && parrySFXwolf != null)
        {
            GameObject parrySFXw = Instantiate(parrySFXwolf, transform.position, Quaternion.identity);
        }

        if (enemy.TypeSound == Sound.Ezh && parrySFXezh != null)
        {
            GameObject parrySFXe = Instantiate(parrySFXezh, transform.position, Quaternion.identity);
        }
        
        if (enemy.TypeSound == Sound.Gnom && parrySFXgnom != null)
        {
            GameObject parrySFXg = Instantiate(parrySFXgnom, transform.position, Quaternion.identity);
        }

        if (playerDestructible.HitPoints > 0)
            player.Parry(parryForce);
    }

    public void ParryTime()
    {
        if (enemyIsActive)
        {
            parryTimer -= Time.deltaTime;

            if (parryTimer <= 0 )//&& !isParry)
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