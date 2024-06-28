using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    protected float lastTimeAttacked;
    protected float deletecombo = 3;
    protected bool isATK;
    protected bool endanim;

    protected PlayerStateMachine stateMachine;
    protected TPlayer player;
    protected Rigidbody2D rb;
    protected float xInput;
    private string animBoolName;
    protected bool triggerCalled;
    protected bool moveTrigger;
    public PlayerState(TPlayer _player,PlayerStateMachine _stateMachine, string _animBoolName){
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    public PlayerState(TPlayer _player,PlayerStateMachine _stateMachine){
        this.player = _player;
        this.stateMachine = _stateMachine;
    }
    public virtual void Enter(){
        if(animBoolName != null)
        player.anim.SetBool(animBoolName,true);
        rb = player.rigid;
        triggerCalled = false;
        moveTrigger = false;
        isATK =false;
    }
    public virtual void Update(){
        if(!isATK)
            xInput = Input.GetAxisRaw("Horizontal");
        player.anim.SetFloat("yVelocity",rb.velocity.y);

    }
    public virtual void Exit(){
        player.anim.SetBool(animBoolName,false);
    }
    public virtual void AnimationFinsihTrigger(){
        triggerCalled = true;
    }
    public virtual void AnimationMoveTrigger(){
        moveTrigger = true;
    }
    
}
