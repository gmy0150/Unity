using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        if(Input.GetKeyDown(KeyCode.Space)&&player.IsGroundDetected()){
            stateMachine.ChangeState(player.jumpState);
        }
        if(!Input.GetKeyDown(KeyCode.Space)&&!player.IsGroundDetected()){
            stateMachine.ChangeState(player.airState);
        }
        if(Input.GetKey(KeyCode.A)&&Input.GetKey(KeyCode.D)&&player.IsGroundDetected()&&player.CoolTime("SkillAD")){
            stateMachine.ChangeState(player.skillAD);
        }
        if(Input.GetKey(KeyCode.A)&&Input.GetKey(KeyCode.S)&&player.IsGroundDetected()&&player.CoolTime("SkillAS")){
            stateMachine.ChangeState(player.skillAS);
        }
        if(Input.GetKey(KeyCode.S)&&Input.GetKey(KeyCode.D)&&player.IsGroundDetected()){
            Debug.Log("??");
            stateMachine.ChangeState(player.skillSD);
        }
        if(Input.GetKey(KeyCode.A)&&Input.GetKey(KeyCode.D)&&Input.GetKey(KeyCode.S)&&player.IsGroundDetected()&&player.CoolTime("SkillASD")){
            Debug.Log("??");
            stateMachine.ChangeState(player.skillASD);
        }

        else if(Input.GetKeyDown(KeyCode.A)&&player.IsGroundDetected()){
            stateMachine.ChangeState(player.attackAState);
        }

        // else if(Input.GetKeyDown(KeyCode.S)&&player.IsGroundDetected()){
        //     stateMachine.ChangeState(player.attackSState);
        // }
        // else if(Input.GetKeyDown(KeyCode.D)&&player.IsGroundDetected()){
        //     stateMachine.ChangeState(player.attackDState);
        // }
    }
}
