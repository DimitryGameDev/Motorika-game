using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] private float parryRange;
    [SerializeField] private float parryWindow;
    [SerializeField] private float parryForce;
    [SerializeField] private float parryDamage;

    [SerializeField] private Collider playerCollider;

    [SerializeField] private Material parryMaterial;
    private Renderer enemyRenderer;
    private Renderer baseRenderer;

    private HashSet<Collider> damagedEnemies = new HashSet<Collider>();
    private Collider[] enemiesCollider;

    private Player player;
    private Destructible playerDestructible;

    private float parryTimer;
    public float ParryTimer => parryTimer;

    private bool isParry;

    private void Start()
    {
        player = GetComponent<Player>();
        playerDestructible = GetComponent<Destructible>();

        parryTimer = parryWindow;
    }

    private void Update()
    {
        enemiesCollider = Physics.OverlapSphere(transform.position, parryRange);

        foreach (var enemyCollider in enemiesCollider)
        {
            HandleParry(enemyCollider);
        }

        ResetDamage();
    }

    private void HandleParry(Collider enemyCollider)
    {
        var destructible = enemyCollider.GetComponent<Destructible>();
        enemyRenderer = enemyCollider.GetComponent<Renderer>();
        baseRenderer = enemyRenderer;

        if (destructible != null)
        {
            ParryTime();
            isParry = true;
            if (parryTimer > 0 && isParry)
            {
                enemyRenderer.material = parryMaterial;
                //player.TakeDamage(destructible);
                //if(enemy != null ){
                // player.Parry(parryForce);
                // Check damage (per once)
                //}
                //else 
                // player.TakeDamage(destructible);
                Debug.Log("Успели парировать. Вы наносите урон.");
            }
            else if (!damagedEnemies.Contains(enemyCollider))
            {
                //TODO
                //Change mat logical 
                enemyRenderer = baseRenderer;
                
                player.Parry(parryForce);
                playerDestructible.ApplyDamage(50);
                damagedEnemies.Add(enemyCollider);
                parryTimer = parryWindow;
                Debug.Log("Не успели парировать. Враг наносит урон.");
            }
        }
    }

    private void ChangeMaterial(Renderer renderer, Color color)
    {
        renderer.material.color = color;
    }

    private void ResetDamage()
    {
        damagedEnemies.Clear();
    }

    private void ParryTime()
    {
        if (isParry)
        {
            if (parryTimer > 0)
            {
                parryTimer -= Time.deltaTime;
            }
            else
            {
                isParry = false;
                parryTimer = 0;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, parryRange);
    }
#endif
}