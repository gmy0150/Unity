using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerFlyState
{
    public PlayerAirState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(xInput * player.moveSpeed, rb.velocity.y);
        if(!Input.GetKeyDown(KeyCode.Space)&&!player.IsGroundDetected()){
            stateMachine.ChangeState(player.airState);
        }
    }
}
