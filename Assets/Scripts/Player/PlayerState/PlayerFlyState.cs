using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyState : PlayerState
{
    protected static int jumpcount;
    public PlayerFlyState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        if(boss.happydoor&&!boss.TransPattern){
            player.Holding();
            if(!player.facingRight&&xInput != 0){
                player.SetVelocity(xInput * (player.moveSpeed - 4),rb.velocity.y);
            }else if(player.facingRight&&xInput != 0){
                player.SetVelocity(xInput * (player.moveSpeed + 4),rb.velocity.y);
            }
        }else{
            rb.velocity = new Vector2(xInput * player.moveSpeed, rb.velocity.y);
        }
        if(player.IsGroundDetected()){
            stateMachine.ChangeState(player.idleState);
            jumpcount = 0;
        }
        if(!player.IsGroundDetected()&&Input.GetKeyDown(KeyCode.A)&&!attacked){
            player.JumpSStart();
        }
        if(!player.IsGroundDetected()&&Input.GetKeyDown(KeyCode.S)&&!attacked){
            player.JumpAStart();
        }
        if(!player.IsGroundDetected()&&Input.GetKeyDown(KeyCode.D)&&!attacked){
            player.JumpDStart();
        }
    }
    
}
