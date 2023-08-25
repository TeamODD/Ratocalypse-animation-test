using System;
using System.Collections;
using MoreMountains.Feedbacks;
using TeamOdd.Ratocalypse.Animation;
using UnityEngine;

public class CharacterMotionTarget : MonoBehaviour
{
    public MMFeedbacks AttackFeedbacks;
    public MMFeedbacks DamageFeedbacks;
    public GameObject IDLE;
    public GameObject Attacks;
    public GameObject Damaged;

    private DeathMotionAnimator _deathMotionAnimator;
    private CharacterAnimationQueue _animationQueue;

    private bool _initialized;
    void Start()
    {
        IDLE.SetActive(true);
        Attacks.SetActive(false);
        Damaged.SetActive(false);
    }
    public void Awake()
    {
        Initialize();
    }
    public void AttackON()
    {
        IDLE.SetActive(false);
        Attacks.SetActive(true);
        Damaged.SetActive(false);
    }
    public void DamagedON()
    {
        Damaged.SetActive(true);
        Attacks.SetActive(false);
        IDLE.SetActive(false);
    }
    public void SetAnimationQueue(CharacterAnimationQueue coroutineQueue)
    {
        _animationQueue = coroutineQueue;
    }

    public void SetDeathMotionAnimator(DeathMotionAnimator deathMotionAnimator)
    {
        _deathMotionAnimator = deathMotionAnimator;
    }

    public void InvokeAnimation(CharacterMotionType type, params Action[] callbacks)
    {
        switch (type)
        {
            case CharacterMotionType.Attack:
                InvokeAttack(callbacks);
                break;
            case CharacterMotionType.Damage:
                InvokeDamage(callbacks);
                break;
            case CharacterMotionType.Death:
                InvokeDeath(callbacks);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void InvokeAttack(params Action[] callbacks)
    {
        _animationQueue.AddCallback(() => { return PlayAttackAnimation(callbacks); });
    }

    private void InvokeDamage(params Action[] callbacks)
    {
        _animationQueue.AddCallback(() => { return PlayDamageAnimation(callbacks); });
    }

    private void InvokeDeath(params Action[] callbacks)
    {
        _deathMotionAnimator.StartAnimation(callbacks);
    }

    private IEnumerator PlayAttackAnimation(params Action[] callbacks)
    {
        var position = transform.position;
        var attackAnimationMiddleCallback = callbacks[0];
        var attackAnimationEndCallback = callbacks[1];
        var feedbacksCoroutine = StartCoroutine(AttackFeedbacks.PlayFeedbacksCoroutine(position));
        yield return new WaitForSeconds(0.4f);
        AttackON();
        yield return new WaitForSeconds(0.1f);
        attackAnimationMiddleCallback();
        yield return feedbacksCoroutine;
        attackAnimationEndCallback();
        yield return null;
    }

    private IEnumerator PlayDamageAnimation(params Action[] callbacks)
    {
        var position = transform.position;
        var attackAnimationEndCallback = callbacks[0];
        var feedbacksCoroutine = StartCoroutine(AttackFeedbacks.PlayFeedbacksCoroutine(position));
        DamagedON();
        yield return feedbacksCoroutine;
        attackAnimationEndCallback();
        yield return null;
    }

    private void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        AttackFeedbacks.Initialization();
        DamageFeedbacks.Initialization();
        _initialized = true;
    }
}
