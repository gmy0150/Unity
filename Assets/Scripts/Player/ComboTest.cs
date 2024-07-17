using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTest : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2;
    public ComboTest(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    
    public override void Enter()
    {
        base.Enter();
        rb.velocity = Vector2.zero;

        if(comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow)
        comboCounter = 0;
        player.anim.SetInteger("ComboCounter",comboCounter);
    }
    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastTimeAttacked = Time.time;

    }
    public override void Update()
    {
        base.Update();
        if(triggerCalled){
            stateMachine.ChangeState(player.idleState);
        }
    }
}
