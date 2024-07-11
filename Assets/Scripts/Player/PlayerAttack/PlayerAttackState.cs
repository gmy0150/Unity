using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    protected int comboCounter;
    protected float lastTimeAttacked;
    protected float deletecombo = 3;
    protected LayerMask enemyLayer;
    protected bool var;
    protected bool sword;
    protected bool hammer;
    public PlayerAttackState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isATK = true;

    }
    public override void Exit()
    {
        base.Exit();
        isATK = false;
    }
    public override void Update(){
        base.Update();

        StopA();
    }
    void StopA(){
        if(moveTrigger&&!var){
            var = true;
            player.rigid.AddForce(Vector2.right*xInput * player.atkspeed,ForceMode2D.Impulse);
            CheckAttack();
        }
        if(triggerCalled){
            stateMachine.ChangeState(player.idleState);
        }
    }
    void CheckAttack(){
        RaycastHit2D hit =Physics2D.Raycast(player.transform.position, player.transform.right, player.AttackRange,enemyLayer);
        if(hit.collider != null){
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null){
                if(sword)
                    enemy.TakeDamage(10);
                else{
                    enemy.TakeDamage(20);
                }
            }
        }
    } 
}
