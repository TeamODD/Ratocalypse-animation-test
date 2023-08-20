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
    private Coroutine _animationCoroutine;

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
        AttachNewAnimationCoroutine(CharacterMotionType.Attack, AttackFeedbacks);
    }

    private void SetDamage()
    {
        AttachNewAnimationCoroutine(CharacterMotionType.Damage, DamageFeedbacks);
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

    private void AttachNewAnimationCoroutine(CharacterMotionType type, MMFeedbacks feedbacks)
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _animationCoroutine = StartCoroutine(DispatchAnimationEventCoroutine(type, feedbacks));
    }

    private IEnumerator DispatchAnimationEventCoroutine(CharacterMotionType type, MMFeedbacks feedbacks)
    {
        _animationStartEvent.Invoke(type);
        yield return feedbacks.PlayFeedbacksCoroutine(transform.position);
        _animationEndEvent.Invoke(type);
    }

    private static IEnumerator PlayDelayedFeedbacks(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
