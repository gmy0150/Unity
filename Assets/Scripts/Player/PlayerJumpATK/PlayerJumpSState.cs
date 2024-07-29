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
        Debug.Log("확인중");
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update(){
        base.Update();
        if(triggerCalled){
            Debug.Log(".00");
            PlayerArrowPool.Instance.GetArrow();
            stateMachine.ChangeState(player.idleState);
        }
    }
}
