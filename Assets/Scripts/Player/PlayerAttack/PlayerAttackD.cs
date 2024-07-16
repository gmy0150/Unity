using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackD : PlayerAttackState
{
    public PlayerAttackD(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName, float _attackSpeed, int _damage,float _attackRange, float _deleteCombo,int _comboCount) : base(_player, _stateMachine, _animBoolName, _attackSpeed, _damage,_attackRange,_deleteCombo,_comboCount)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // enemyLayer =  LayerMask.GetMask ("Enemy");
        var = false;

    }
    public override void Exit()
    {
        base.Exit();


    }
    public override void Update(){
        base.Update();
    }
}
