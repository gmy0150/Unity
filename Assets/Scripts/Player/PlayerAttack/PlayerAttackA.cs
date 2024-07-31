using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackA : PlayerAttackState
{
    public PlayerAttackA(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName, float _attackSpeed, int _damage, int _shiledDMG, float _attackRange, float _deleteCombo, int _comboCount) : base(_player, _stateMachine, _animBoolName, _attackSpeed, _damage, _shiledDMG, _attackRange, _deleteCombo, _comboCount)
    {
    }

    public override void Enter(){
        base.Enter();
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
