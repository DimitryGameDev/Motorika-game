    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    [SerializeField] private AbilitiesChanger abilitiesChanger;
    [SerializeField] private float slowDownFactor;
    public float SlowDownFactor => slowDownFactor;
    [SerializeField] private float slowDownDuration; 
    [SerializeField] private float radius; 

    private bool isSlowed = false;
    public bool IsSlowed => isSlowed;
    private float originalTimeScale;

    private int timeLevel;
    
    private Dictionary<GameObject, float> originalSpeeds = new Dictionary<GameObject, float>();
    private void Start()
    {
        timeLevel = PlayerPrefs.GetInt("Ability1");
        
        PlayerInputController.Instance.FirstAbilityEvent += AbilityCheckFirst;
        PlayerInputController.Instance.SecondAbilityEvent += AbilityCheckSecond;
    }

    private void OnDestroy()
    {
        PlayerInputController.Instance.FirstAbilityEvent -= AbilityCheckFirst;
        PlayerInputController.Instance.SecondAbilityEvent -= AbilityCheckSecond;
    }
    public void AbilityCheckFirst()
    {
        if (!isSlowed && abilitiesChanger.PreviousFirstIndex == 1)
        {
            StartCoroutine(SlowDown());
        }
    }
    public void AbilityCheckSecond()
    {
        if (!isSlowed && abilitiesChanger.PrevoiusSecondIndex == 1)
        {
            StartCoroutine(SlowDown());
        }
    }

    private IEnumerator SlowDown()
    {
         isSlowed = true;
         originalTimeScale = Time.timeScale;
         Time.timeScale = slowDownFactor;
         Time.fixedDeltaTime = 0.02f * Time.timeScale; 
    
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        var timer = slowDownDuration + (timeLevel * 0.5f);
        yield return new WaitForSeconds(timer);

        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 

        // reset original velocities
        foreach (var kvp in originalSpeeds)
        {
            var obj = kvp.Key;
            var rb = obj.GetComponent<Rigidbody>(); 
            if (rb != null)
            {
                rb.velocity = rb.velocity.normalized * kvp.Value; 
            }
        }

        isSlowed = false;
        AchievementManager.Instance.TimeScale();
    }

    private void OnDrawGizmosSelected()
    {
      
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
