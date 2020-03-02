using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState
{
    private float attackTimer;
    private float attackCooldown = 3;
    private bool canAttack = true;

    private Enemy enemy;

    public void Execute()
    {
        Attack();
        if (!enemy.InMeleeRange)
        {
            enemy.ChangeState(new RunState());
        }
        else if (enemy.Target ==null)
        {
            enemy.ChangeState(new IdleState());
        }

    }
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void Exit()
    {

    }
    public void OnTriggerEnter(Collider2D other)
    {

    }

    public void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            canAttack = true;
            attackTimer = 0;
        }

        if (canAttack)
        {
            canAttack = false;
            enemy.MyAnimator.SetTrigger("attack");
        }

    }
}
