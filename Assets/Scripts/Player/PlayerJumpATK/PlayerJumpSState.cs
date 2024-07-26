using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpSState : PlayerJumpATKState
{
    public PlayerJumpSState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName,  int _damage) : base(_player, _stateMachine, _animBoolName , _damage)
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
    }
}
