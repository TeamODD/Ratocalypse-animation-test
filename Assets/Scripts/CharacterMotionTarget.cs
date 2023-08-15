using System;
using System.Collections;
using MoreMountains.Feedbacks;
using TeamOdd.Ratocalypse.Animation;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMotionTarget : MonoBehaviour
{
    public MMFeedbacks AttackFeedbacks;
    public MMFeedbacks DamageFeedbacks;

    private UnityEvent<CharacterMotionType> _animationStartEvent;
    private UnityEvent<CharacterMotionType> _animationEndEvent;
    private DeathMotionAnimator _deathMotionAnimator;
    private Coroutine _coroutine;

    public void Awake()
    {
        Initialize();
    }

    public void SetDeathMotionAnimator(DeathMotionAnimator deathMotionAnimator)
    {
        _deathMotionAnimator = deathMotionAnimator;
    }

    public void SetAnimationStartEvent(UnityEvent<CharacterMotionType> animationStartEvent)
    {
        _animationStartEvent = animationStartEvent;
    }

    public void SetAnimationEndEvent(UnityEvent<CharacterMotionType> animationEndEvent)
    {
        _animationEndEvent = animationEndEvent;
    }

    public void SetType(CharacterMotionType type)
    {
        switch (type)
        {
            case CharacterMotionType.Idle:
                SetIdle();
                break;
            case CharacterMotionType.Attack:
                SetAttack();
                break;
            case CharacterMotionType.Damage:
                SetDamage();
                break;
            case CharacterMotionType.Death:
                SetDeath();
                break;
        }
    }

    private void SetIdle()
    {
        // TODO
    }

    private void SetAttack()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _animationStartEvent.Invoke(CharacterMotionType.Attack);
        _coroutine = StartCoroutine(PlayDelayedFeedbacks(0f, () =>
        {
            AttackFeedbacks.PlayFeedbacks();
            _animationEndEvent.Invoke(CharacterMotionType.Attack);
        }));
    }

    private void SetDamage()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _animationStartEvent.Invoke(CharacterMotionType.Damage);
        _coroutine = StartCoroutine(PlayDelayedFeedbacks(0.5f, () =>
        {
            DamageFeedbacks.PlayFeedbacks();
            _animationEndEvent.Invoke(CharacterMotionType.Damage);
        }));
    }

    private void SetDeath()
    {
        if (_deathMotionAnimator == null)
        {
            throw new NullReferenceException("DeathMotionAnimator is null");
        }

        _deathMotionAnimator.StartAnimation();
    }

    private void Initialize()
    {
        AttackFeedbacks.Initialization();
        DamageFeedbacks.Initialization();
    }

    private static IEnumerator PlayDelayedFeedbacks(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
        yield return null;
    }
}
