using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpATKState : PlayerState
{
    protected float attackSpeed;
    protected int damage;
    protected LayerMask enemyLayer;

    public PlayerJumpATKState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName,int _damage) : base(_player, _stateMachine, _animBoolName)
    {
        damage = _damage;
    }

    public override void Enter()
    {
        base.Enter();
        isATK = true;

        enemyLayer =  LayerMask.GetMask ("Enemy");
        player.StartCoroutine(BusyFor());
        
    }
    public override void Exit()
    {
        base.Exit();
        isATK = false;
    }
    public override void Update(){
        base.Update();
        if(boss.happydoor){
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

}
