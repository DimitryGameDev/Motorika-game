using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewWolf : Enemy
{
    private Animator mAnimator;
    
    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        Parry.Instance.AttackAnimEvent += AttackAnimation;
        Parry.Instance.IdleAnimEvent += IdleAnimation;

    }
    private void OnDestroy()
    {
        Parry.Instance.AttackAnimEvent -= AttackAnimation;
        Parry.Instance.IdleAnimEvent -= IdleAnimation;
    }
    private void AttackAnimation()
    {
       
        if (mAnimator != null)
        {
           
                Debug.Log("123");
                mAnimator.SetTrigger("Attack");
            
        }
            
    }
    private void IdleAnimation()
    {
        mAnimator.SetTrigger("Walk");
    }
}
