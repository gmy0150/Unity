using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPlayer : MonoBehaviour
{
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpforce;
    public float atkspeed;

    [Header("count")]
    
    public int attackACount;
    public float ComboTimer;

    [Header("Collision info")]
    [SerializeField]private Transform groundCheck;
    [SerializeField]private float groundCheckDistance;
    [SerializeField]private Transform WallCheck;
    [SerializeField]private float WallCheckDistance;
    [SerializeField]private LayerMask whatIsGround;
    [SerializeField]private LayerMask whatIsWall;

    public int facingDir {get;private set;} = -1;
    private bool facingRight = true;
    public bool isBusy;
    #region Components
    public Animator anim{get; private set;}
    public Rigidbody2D rb{get;private set;}
    #endregion

    #region States

    public PlayerStateMachine stateMachine {get;private set;}
    public PlayerIdleState idleState{get;private set;}
    public PlayerMoveState moveState{get;private set;}
    public PlayerJumpState jumpState{get;private set;}
    public PlayerAirState airState{get;private set;}
    public PlayerAttackState attackState{get; private set;}

    #endregion
    public float timer;
    private void Awake() {
        stateMachine = new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this, stateMachine,"Idle");
        moveState = new PlayerMoveState(this, stateMachine,"Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        attackState = new PlayerAttackState(this, stateMachine, "AttackA");
    }
    private void Start() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initalize(idleState);
    }
    private void Update() {
        stateMachine.currentState.Update();
        FlipController();
    }
    public IEnumerator BusyFor(){
        isBusy = true;
        yield return new WaitForSeconds(0.2f);
        isBusy = false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinsihTrigger();//함수 호출후 animationFinishTrigger를 작동하게 함

    public void MoveTrigger() => stateMachine.currentState.AnimationMoveTrigger();

    public void SetVelocity(float _xVelocity,float _yVelocity){
        rb.velocity = new Vector2(_xVelocity,_yVelocity);
    }
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down,groundCheckDistance,whatIsGround);
    private void OnDrawGizmos() {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + WallCheckDistance, WallCheck.position.y));
    }
    public void Flip(){
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        
        transform.Rotate(0,180,0);

    }

    public void FlipController(){
        if(rb.velocity.x > 2 && !facingRight){//양수로 가고 facingRight가 false일 때 기본이 false니까 오른쪽을 보고있을 때, 오른쪽 이동일 때
            Flip();
            Debug.Log("1");
        }else if(rb.velocity.x < -2 && facingRight){//음수로 가고 facingRight가 true일 때 왼쪽이동 transform.flip을 쓰면 문제가 되는 부분이 많으니까 이렇게 처리해서 하는 부분 이건 공부를 해야겠다.
            Flip();
            Debug.Log("2");
        }
    }
}
