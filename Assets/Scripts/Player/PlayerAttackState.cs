using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float deletecombo = 3;
    bool var;
    public PlayerAttackState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
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
        StopA();
        
    }
    void StopA(){
        if(moveTrigger&&!var){
            var = true;
            player.rb.AddForce(Vector2.right*xInput * player.atkspeed,ForceMode2D.Impulse);

        }
        if(triggerCalled){
            stateMachine.ChangeState(player.idleState);
        }
    }
}
