using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAS : PlayerSkillState
{
    public PlayerAS(TPlayer _player, PlayerStateMachine _stateMachine) : base(_player, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.detailAS();
    }
    public override void Exit()
    {
        base.Exit();

    }
    public override void Update(){
        base.Update();
        if(!player.isAS)
            stateMachine.ChangeState(player.idleState);
    }
}
