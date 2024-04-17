using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public enum State { disabled, idle, activate, run, attack, hurt, deactivate };
    public State CurrentState;
    public Animator Anim;

    static readonly int IdleAnimation = Animator.StringToHash("idle"); 
    static readonly int DisabledAnimation = Animator.StringToHash("disabled");
    static readonly int ActivateAnimation = Animator.StringToHash("activate");
    static readonly int RunAnimation = Animator.StringToHash("run");
    static readonly int AttackAnimation = Animator.StringToHash("attack");
    static readonly int HurtAnimation = Animator.StringToHash("hurt");
    static readonly int DeactivateAnimation = Animator.StringToHash("deactivate");

    void Start()
    {
        SetState(State.idle);
    }

    void Update()
    {
        switch (CurrentState)
        {
            case State.idle:
                Anim.CrossFade(IdleAnimation, 0, 0);
                break;

            case State.disabled:
                Anim.CrossFade(DisabledAnimation, 0, 0);
                break;

            case State.activate:
                Anim.CrossFade(ActivateAnimation, 0, 0);
                break;

            case State.run:
                Anim.CrossFade(RunAnimation, 0, 0);
                break;

            case State.attack:
                Anim.CrossFade(AttackAnimation, 0, 0);
                break;

            case State.hurt:
                Anim.CrossFade(HurtAnimation, 0, 0);
                break;

            case State.deactivate:
                Anim.CrossFade(DeactivateAnimation, 0, 0);
                break;
        }
    }

    IEnumerator HoldStateFor(State targetState, State restState, float duration)
    {
        CurrentState = targetState;
        yield return new WaitForSeconds(duration);
        
        CurrentState = restState;
    }

    public void SetState(State newState)
    {
        if (CurrentState == newState)
        {
            return;
        }

        CurrentState = newState;
    }

    public void BlendState(State newState, State restState, float duration)
    {
        if (CurrentState == newState)
        {
            return;
        }

        StartCoroutine(HoldStateFor(newState, restState, duration));
    }
}
