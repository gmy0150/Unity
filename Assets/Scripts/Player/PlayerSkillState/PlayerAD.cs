using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAD : PlayerSkillState
{
    public PlayerAD(TPlayer _player, PlayerStateMachine _stateMachine) : base(_player, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.detailAD();
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
