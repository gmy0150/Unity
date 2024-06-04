using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    Vector3 PlayerPos;
    public PlayerAttackState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    bool var;
    public override void Enter()
    {
        PlayerPos = player.transform.position;
        endanim = true;
        isATK =true;
        base.Enter();
        player.ResetTimer();
        player.rb.velocity = Vector2.zero;
        
        Debug.Log("?");
    }
    public override void Exit()
    {
        base.Exit();
        var = false;
    }
    public override void Update(){
    base.Update();

        StopA();
    }
    void StopA(){
        if(Input.GetKeyDown(KeyCode.A)){
            isATK = true;
        }
        if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f&&!player.anim.IsInTransition(0)) {
            // player.SetVelocity(xInput * player.atkspeed,rb.velocity.y);
            if(!var){
                var =true;
                player.rb.AddForce(Vector2.right*xInput * player.atkspeed,ForceMode2D.Impulse);
                // PlayerPos = new Vector3(player.transform.position.x + xInput *  player.atkspeed,player.transform.position.y,player.transform.position.z);
                // player.transform.position = PlayerPos;
                // Debug.Log(PlayerPos);
            }
        }
        if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f&&!player.anim.IsInTransition(0)) {
            // player.SetVelocity(10,rb.velocity.y);


            if(isATK){

                stateMachine.ChangeState(player.attackState);
                isATK = false;
                
                Debug.Log("isatk확인");

            }else{
                stateMachine.ChangeState(player.idleState);
                Debug.Log("확인");
                player.atk = false;
            }

        }
        
    }
}
