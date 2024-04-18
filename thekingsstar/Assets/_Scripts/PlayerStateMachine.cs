using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum State
    {
        idle, jump, attack, death
    };
    public Rigidbody2D Rb;
    public State CurrentState;
    public PlayerController Player;
    public Animator Anim;

    static readonly int IdleAnimation = Animator.StringToHash("idle");
    static readonly int DeathAnimation = Animator.StringToHash("hurt");
    static readonly int JumpAnimation = Animator.StringToHash("jump");
    static readonly int AttackAnimation = Animator.StringToHash("attack");

    private void OnEnable()
    {
        StartButton.OnGameBegin += Init;
    }

    private void Init()
    {
        CurrentState = State.idle;
    }

    private void OnDisable()
    {
        StartButton.OnGameBegin -= Init;
    }

    private void Start()
    {
        CurrentState = State.idle;
    }
    private void Update()
    {
        StateAnimUpdate();
        
        if (Player.IsDead && CurrentState != State.death)
        {
            CurrentState = State.death;
        }

        if (CurrentState == State.death)
        {
            return;
        }

        if (Player.IsAttacking && CurrentState != State.attack)
        {
            CurrentState = State.attack;
        }

        if (Player.IsAttacking)
        {
            return;
        }

        if ((Player.IsGrounded && Rb.velocity.y == 0) && CurrentState != State.idle)
        {
            CurrentState = State.idle;
        }
        else if ((!Player.IsGrounded && Rb.velocity.y != 0) && CurrentState != State.jump)
        {
            CurrentState = State.jump;
        }
    }
    void StateAnimUpdate()
    {
        switch (CurrentState)
        {
            case State.idle:
                Anim.CrossFade(IdleAnimation, 0, 0);
                break;

            case State.jump:
                Anim.CrossFade(JumpAnimation, 0, 0);
                break;

            case State.death:
                Anim.CrossFade(DeathAnimation, 0, 0);
                break;

            case State.attack:
                Anim.CrossFade(AttackAnimation, 0, 0);
                break;

        }
    }
}
