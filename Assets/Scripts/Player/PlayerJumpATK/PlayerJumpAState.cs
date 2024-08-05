using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAState : PlayerJumpATKState
{
    public PlayerJumpAState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName, float _attackRange, int _damage, int _shiledDMG) : base(_player, _stateMachine, _animBoolName, _attackRange, _damage, _shiledDMG)
    {
    }

    public override void Enter()
    {
        base.Enter();
        var = false;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update(){
        base.Update();
        if(triggerCalled){
            stateMachine.ChangeState(player.idleState);
        }
        StopA();
    }
    void StopA(){
        if(moveTrigger&&!var){
            var = true;
            CheckAttack();
        }
    }
    void CheckAttack()
    {
        RaycastHit2D hit =Physics2D.Raycast(player.transform.position, player.transform.right, attackRange,enemyLayer|bossLayer);
        if(hit.collider != null){
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null){
                enemy.getDamage(damage,ShiledDamage);
            }
            if(hit.collider.tag == "Boss"){
                boss.getDamage(damage,ShiledDamage);
            }
            Debug.Log("화깅ㄴ");
        }
    }
}
