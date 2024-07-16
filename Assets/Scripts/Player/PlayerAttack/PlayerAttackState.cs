using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    protected int comboCounter;
    private int combocount;
    protected float lastTimeAttacked;
    protected float deletecombo;
    protected LayerMask enemyLayer;
    protected bool var;
    protected bool sword;
    protected bool hammer;
    protected float attackSpeed;
    protected int damage;
    private float attackRange;
    public PlayerAttackState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName,float _attackSpeed,int _damage,float _attackRange,float _deleteCombo, int _comboCount) : base(_player, _stateMachine, _animBoolName)
    {
        attackSpeed = _attackSpeed;
        damage = _damage;
        attackRange = _attackRange;
        deletecombo = _deleteCombo;
        comboCounter = _comboCount;
    }

    public PlayerAttackState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isATK = true;
        if(combocount >= comboCounter || Time.time > lastTimeAttacked + deletecombo)
            combocount = 0;
        player.anim.SetInteger("ComboCounter",combocount);
        enemyLayer =  LayerMask.GetMask ("Enemy");

    }
    public override void Exit()
    {
        base.Exit();
        isATK = false;
        combocount++;
        lastTimeAttacked = Time.time;
        player.StartCoroutine(BusyFor(attackSpeed));
    }
    public override void Update(){
        base.Update();

        StopA();
    }
    void StopA(){
        if(moveTrigger&&!var){
            var = true;
            player.rigid.AddForce(Vector2.right*xInput * 10,ForceMode2D.Impulse);
            CheckAttack();
        }
        if(triggerCalled){
            stateMachine.ChangeState(player.idleState);
        }
    }
    void CheckAttack(){
        RaycastHit2D hit =Physics2D.Raycast(player.transform.position, player.transform.right, attackRange,enemyLayer);
        if(hit.collider != null){
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null){
                enemy.TakeDamage(damage);
            }
        }
    } 
    protected IEnumerator BusyFor(float seconds)
    {
        attacked = true;
        yield return new WaitForSeconds(seconds);
        attacked = false;
    }
}
