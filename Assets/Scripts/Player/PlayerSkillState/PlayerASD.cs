using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerASD : PlayerSkillState
{
    public PlayerASD(TPlayer _player, PlayerStateMachine _stateMachine) : base(_player, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.useASD();
    }
    public override void Exit()
    {
        base.Exit();

    }
    public override void Update(){
        base.Update();
        stateMachine.ChangeState(player.idleState);
    }
}
