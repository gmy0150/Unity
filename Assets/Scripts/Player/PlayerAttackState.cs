using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        player.ResetTimer();

    }
    public override void Exit()
    {
        base.Exit();

    }
    public override void Update(){
    base.Update();

        StopA();
    }
    void StopA(){
        
        if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
            // 추가 공격 입력이 있었다면 같은 상태를 재진입
            stateMachine.ChangeState(player.idleState);
        }
        
    }
}
