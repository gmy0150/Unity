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
    }
}
