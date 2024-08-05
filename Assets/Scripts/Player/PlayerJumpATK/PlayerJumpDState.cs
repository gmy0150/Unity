using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpDState : PlayerJumpATKState
{
    float timer;
    bool pass;
    public PlayerJumpDState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName, float _attackRange, int _damage, int _shiledDMG) : base(_player, _stateMachine, _animBoolName, _attackRange, _damage, _shiledDMG)
    {
    }

    public override void Enter()
    {
        base.Enter();
        var =false;
    }
    public override void Exit()
    {
        base.Exit();
        timer = 0;
    }
    public override void Update(){
        base.Update();
        timer += Time.deltaTime;
        if(timer >= 2f){
            ResumeAnim();
            CheckAttack();

            stateMachine.ChangeState(player.idleState);
            if(triggerCalled){
            }
        }
        else if(moveTrigger&&!player.IsGroundDetected()||pass){
            PauseAnim();
        }
        else if(isPaused&&player.IsGroundDetected()){
            ResumeAnim();
            CheckAttack();
            stateMachine.ChangeState(player.idleState);
            if(triggerCalled){

            }
        }
    }
    void CheckAttack(){
        RaycastHit2D hit =Physics2D.Raycast(player.transform.position, player.transform.right, attackRange,enemyLayer|bossLayer);
        if(hit.collider != null){
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null){
                enemy.getDamage(damage,ShiledDamage);
            }
            if(hit.collider.tag == "Boss"){
                boss.getDamage(damage,ShiledDamage);
            }
        }
    }
}
