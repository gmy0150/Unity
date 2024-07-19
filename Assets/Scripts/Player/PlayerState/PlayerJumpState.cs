using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerFlyState
{
    public PlayerJumpState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpforce);
        jumpcount++;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
            // rb.velocity = new Vector2(xInput * player.moveSpeed, rb.velocity.y);
        if(rb.velocity.y < 0){
            stateMachine.ChangeState(player.airState);
        }
    }
}
