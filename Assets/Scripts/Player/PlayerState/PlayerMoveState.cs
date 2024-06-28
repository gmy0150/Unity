using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        if(!isATK)
            player.SetVelocity(xInput * player.moveSpeed,rb.velocity.y);

        if(xInput == 0&&!player.isBusy){
            stateMachine.ChangeState(player.idleState);
        }
    } 
}
