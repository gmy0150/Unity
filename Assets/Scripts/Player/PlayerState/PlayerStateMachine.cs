using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState{get; private set;}

    public void Initalize(PlayerState _startState){
        currentState = _startState;
        currentState.Enter();
    }
    public void ChangeState(PlayerState _newState){
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
    public void DashState(PlayerState _newState){
        currentState = _newState;
        currentState.Enter();
    }
    public bool GetState(PlayerState _thisState){
        if(_thisState == currentState){
            return true;
        }
        else
            return false;
    }

} 
