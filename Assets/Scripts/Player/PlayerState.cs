using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    protected bool isATK;
    protected bool endanim;

    protected PlayerStateMachine stateMachine;
    protected TPlayer player;
    protected Rigidbody2D rb;
    protected float xInput;
    private string animBoolName;
    public PlayerState(TPlayer _player,PlayerStateMachine _stateMachine, string _animBoolName){
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter(){
        player.anim.SetBool(animBoolName,true);
        rb = player.rb;
    }
    public virtual void Update(){
        xInput = Input.GetAxisRaw("Horizontal");
        
        player.anim.SetFloat("yVelocity",rb.velocity.y);
        
        if(player.attackACount==0 ){
            player.anim.SetFloat("Sword",0f);
        }
        else if(player.attackACount==1 ){
            player.anim.SetFloat("Sword",0.33f);
        }
        else if(player.attackACount == 2 ){
            player.anim.SetFloat("Sword",0.66f);
        }
        if(player.attackACount >= 3){
            player.attackACount = 0;
        }

    }
    public virtual void Exit(){
        player.anim.SetBool(animBoolName,false);
    }
}
