using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpDState : PlayerJumpATKState
{
    float timer;
    public PlayerJumpDState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName, float _attackRange, int _damage, int _shiledDMG) : base(_player, _stateMachine, _animBoolName, _attackRange, _damage, _shiledDMG)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
        timer = 0;
    }
    public override void Update(){
        base.Update();
        timer += Time.deltaTime;
        if(timer >= 1.5f){
            stateMachine.ChangeState(player.idleState);
        }
        if(player.IsGroundDetected()&&triggerCalled){
            
            stateMachine.ChangeState(player.idleState);
        }
    }
}
