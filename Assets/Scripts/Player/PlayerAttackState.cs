using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    
    public override void Enter()
    {
        isATK =true;
        base.Enter();
        player.ResetTimer();
    }
    public override void Exit()
    {
        base.Exit();

    }
    public override void Update(){
    base.Update();

        StopA();
    }
    void StopA(){
        if(Input.GetKeyDown(KeyCode.A)){
            isATK = true;
        }
        if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f&&!player.anim.IsInTransition(0)) {

            // player.anim.SetBool("AttackA",false);
            // 추가 공격 입력이 있었다면 같은 상태를 재진입

            if(isATK){
                stateMachine.ChangeState(player.attackState);
                isATK = false;
                
                Debug.Log("isatk확인");

            }else{
                stateMachine.ChangeState(player.idleState);
                Debug.Log("확인");

            }

        }
        
    }
}
