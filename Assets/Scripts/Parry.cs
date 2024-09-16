using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] private float parryWindow;
    [SerializeField] private float parryForce;

    [SerializeField] private int playerDamage;
    [SerializeField] private int enemyDamage;

    [SerializeField] private Material parryMaterial;
    private Renderer enemyRenderer;

    private Player player;
    private Destructible playerDestructible;
    private Destructible enemyDestructible;

    private float parryTimer;
    public float ParryTimer => parryTimer; //TODO: Change enemy script Wolf

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

    private void OnTriggerEnter(Collider enemy)
    {
        enemyDestructible = enemy.GetComponent<Destructible>();
        enemyRenderer = enemy.GetComponent<Renderer>();

        if (enemyDestructible != null && !enemyIsActive)
        {
            enemyIsActive = true;
            enemyRenderer.material = parryMaterial;
            parryTimer = parryWindow;
        }
    }

    private void OnTriggerStay(Collider enemy)
    {
        if (enemyDestructible != null && enemyIsActive && !isParry && parryTimer > 0)
        {
            if (Input.GetKey(KeyCode.W))
            {
                PlayerParry();
            }
        }
    }
    private void OnTriggerExit(Collider enemy)
    {
        ResetParry();
    }

    private void PlayerParry()
    {
        enemyDestructible.ApplyDamage(playerDamage);
        Debug.Log("Успешное парирование, враг получает урон.");

        if (enemyDestructible.HitPoints > 0)
            player.Parry(parryForce);
        else
            Debug.Log("Враг уничтожен, продолжаем движение.");

        isParry = true;
        ResetParry();
    }

    private void ApplyEnemyDamage()
    {
        playerDestructible.ApplyDamage(enemyDamage);
        Debug.Log("Не успели парировать. Враг наносит урон.");

        if (playerDestructible.HitPoints > 0)
            player.Parry(parryForce);
    }

    private void ParryTime()
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