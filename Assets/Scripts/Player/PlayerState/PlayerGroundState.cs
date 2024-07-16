using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    private float keyDelay = 0.03f;
    private float APressTime = -1f;
    private float SPressTime = -1f;
    private float DPressTime = -1f;
    private bool isAPressed = false;
    private bool isSPressed = false;
    private bool isDPressed = false;
    private float delayA = 0.04f;
    private float delayS = 0.04f;
    private float delayD = 0.04f;
    bool skillAD;
    bool skillAS;
    bool skillSD;
    bool skillASD;

    public PlayerGroundState(TPlayer _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        float currentTime = Time.time;
        if(Input.GetKeyDown(KeyCode.Space)&&!Input.GetKey(KeyCode.DownArrow)&&player.IsGroundDetected()){
            stateMachine.ChangeState(player.jumpState);
        }
        if(!Input.GetKeyDown(KeyCode.Space)&&!player.IsGroundDetected()){
            stateMachine.ChangeState(player.airState);
        }
        if(Input.GetKeyDown(KeyCode.Space)&&Input.GetKey(KeyCode.DownArrow)){
            player.checkDownPlatform();
        }
        if(Input.GetKeyDown(KeyCode.A)){
            APressTime = currentTime;
            isAPressed = true;
        }
        if(Input.GetKeyDown(KeyCode.S)){
            SPressTime = currentTime;
            isSPressed = true;
        }
        if(Input.GetKeyDown(KeyCode.D)){
            DPressTime = currentTime;
            isDPressed = true;
        }
        if(
        CheckKeyPressed(APressTime,SPressTime, DPressTime, keyDelay)&&player.CoolTime("SkillASD")){
            stateMachine.ChangeState(player.skillASD);
            skillASD = true;
            ResetKeyPressTimes();
        }
        else if(
        CheckKeyPressed(APressTime, DPressTime, keyDelay)&&!skillASD&&player.CoolTime("SkillAD")){
            stateMachine.ChangeState(player.skillAD);
            skillAD = true;
            ResetKeyPressTimes();
        }else if(
        CheckKeyPressed(APressTime, SPressTime, keyDelay)&&!skillASD&&!skillAD&&player.CoolTime("SkillAS")){
            stateMachine.ChangeState(player.skillAS);
            skillAS = true;
            ResetKeyPressTimes();
        }else if(
        CheckKeyPressed(SPressTime, DPressTime, keyDelay)&&!skillASD&&!skillAD&&!skillAS&&player.CoolTime("SkillSD")){
            stateMachine.ChangeState(player.skillSD);
            skillSD = true;
            ResetKeyPressTimes();
        }
        else{
            if(isAPressed && currentTime - APressTime >= delayA){
                if(player.IsGroundDetected()&&!attacked){
                    stateMachine.ChangeState(player.attackAState);
                }
            isAPressed = false;
            }
            if(isDPressed && currentTime - DPressTime >= delayD){
                if(player.IsGroundDetected()&&!attacked){
                    stateMachine.ChangeState(player.attackDState);

                }
                isDPressed = false;
            }if(isSPressed && currentTime - SPressTime >= delayS){
                if(player.IsGroundDetected()&&!attacked){
                    stateMachine.ChangeState(player.attackSState);
                }
                isSPressed = false;
            }
        }
    }
        
    bool CheckKeyPressed(float pressTimeA, float pressTimeD, float delayA){
        return pressTimeA > 0 && pressTimeD > 0 && Mathf.Abs(pressTimeA - pressTimeD) <= delayA;
    }
    bool CheckKeyPressed(float pressTimeA,float pressTimeS ,float pressTimeD, float delay){
        return pressTimeA > 0 && pressTimeD > 0 && pressTimeS > 0 &&
               Mathf.Abs(pressTimeA - pressTimeD) <= delay &&
               Mathf.Abs(pressTimeA - pressTimeS) <= delay &&
               Mathf.Abs(pressTimeD - pressTimeS) <= delay;
    }
    
    void ResetKeyPressTimes()
    {
        APressTime = -1f;
        SPressTime = -1f;
        DPressTime = -1f;
        isAPressed = false;
        isSPressed = false;
        isDPressed = false;
        skillAD = false;
        skillASD = false;
        skillAS = false;
        skillSD = false;

    }
}
