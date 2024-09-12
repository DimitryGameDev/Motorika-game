using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] private float parryRange;
    [SerializeField] private float parryWindow;
    [SerializeField] private float parryForce;
    [SerializeField] private float parryDamage;

    [SerializeField] private Material parryMaterial;
    private Renderer enemyRenderer;

    private HashSet<Collider> ourEnemies = new HashSet<Collider>();
    
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
        //enemiesCollider = Physics.OverlapSphere(transform.position, parryRange);

        ResetDamage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        
        enemiesCollider = other.GetComponents<Collider>();

        foreach (var enemyCollider in enemiesCollider)
        {
            HandleParry(enemyCollider);
        }
    }

    private void HandleParry(Collider enemyCollider)
    {
        var destructible = enemyCollider.GetComponent<Destructible>();
        enemyRenderer = enemyCollider.GetComponent<Renderer>();

        if (destructible != null)
        {
            ParryTime();
            isParry = true;
            if (parryTimer > 0 && isParry )
            {
               if (Input.GetKey(KeyCode.W))
               {
                    var result = destructible.ParryDamage();
                    if (!result)
                    {
                        playerDestructible.ApplyDamage(50);
                        player.Parry(parryForce);
                        Debug.Log("Успели парировать. Вы наносите урон.");
                    }
               }
                Debug.Log("Окно открылось. Вы наносите урон.");
            }
            else if (!ourEnemies.Contains(enemyCollider))
            {   
                player.Parry(parryForce);
                playerDestructible.ApplyDamage(50);
                ourEnemies.Add(enemyCollider);
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
        ourEnemies.Clear();
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