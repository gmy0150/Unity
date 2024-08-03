using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpATKState : PlayerState
{
    protected float attackSpeed;
    protected int damage;
    protected int ShiledDamage;
    protected float attackRange;
    protected LayerMask enemyLayer;
    protected LayerMask bossLayer;

    public PlayerJumpATKState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName,float _attackRange,int _damage,int _shiledDMG) : base(_player, _stateMachine, _animBoolName)
    {
        damage = _damage;
        ShiledDamage = _shiledDMG;
        attackRange = _attackRange;
    }
    public PlayerJumpATKState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isATK = true;

        enemyLayer =  LayerMask.GetMask ("Enemy");
        bossLayer = LayerMask.GetMask("Boss");
        player.StartCoroutine(BusyFor());
        
    }
    public override void Exit()
    {
        base.Exit();
        isATK = false;
    }
    public override void Update(){
        base.Update();
        if(boss.happydoor&&!boss.TransPattern){
            player.Holding();
        }
        if(player.IsGroundDetected()&&triggerCalled){
            stateMachine.ChangeState(player.idleState);
        }
        
        // StopA();
    }
    protected IEnumerator BusyFor()
    {
        attacked = true;
        yield return new WaitForSeconds(0.3f);
        attacked = false;
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
