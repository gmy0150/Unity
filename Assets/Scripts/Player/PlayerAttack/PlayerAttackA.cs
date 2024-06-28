using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackA : PlayerAttackState
{
    public PlayerAttackA(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter(){
        base.Enter();

        enemyLayer =  LayerMask.GetMask ("Enemy");
        isATK = true;
        var = false;
        if(comboCounter > 2 || Time.time > lastTimeAttacked + deletecombo)
            comboCounter = 0;
        player.anim.SetInteger("ComboCounter",comboCounter);
    }
    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastTimeAttacked = Time.time;
        player.StartCoroutine("BusyFor");
    }
    public override void Update(){
        base.Update();

    }
    

}
