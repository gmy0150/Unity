using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpDState : PlayerJumpATKState
{
    public PlayerJumpDState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName, int _damage) : base(_player, _stateMachine, _animBoolName, _damage)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("1111");
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update(){
        base.Update();
    }
}
