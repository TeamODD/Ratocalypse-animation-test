using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TeamOdd.Ratocalypse.Animation
{
    public class CharacterMotion : CharacterAnimationQueue
    {
        public KeyCode AttacKeyCode = KeyCode.A;
        public KeyCode DamageKeyCode = KeyCode.D;
        public KeyCode DeathKeyCode = KeyCode.F;

        public CharacterMotionTarget Target;
        public DeathMotionAnimator DeathMotionAnimator;
        public UnityEvent<CharacterMotionType> AnimationStartEvent;
        public UnityEvent<CharacterMotionType> AnimationEndEvent;
        // public CharacterAnimationQueue CoroutineQueue;

        private Renderer _renderer;

        public new void Awake()
        {
            base.Awake();
            Initialize();
        }

        public void Update()
        {
            if (Input.GetKeyDown(AttacKeyCode))
            {
                InvokeAnimation(CharacterMotionType.Attack);
            }
            else if (Input.GetKeyDown(DamageKeyCode))
            {
                InvokeAnimation(CharacterMotionType.Damage);
            }
            else if (Input.GetKeyDown(DeathKeyCode))
            {
                InvokeAnimation(CharacterMotionType.Death);
            }
        }

        public void OnAnimationStart(CharacterMotionType type)
        {
            Debug.Log($"Animation Start: {type}");
        }

        public void OnAnimationEnd(CharacterMotionType type)
        {
            Debug.Log($"Animation End: {type}");
        }

        public void InvokeAnimation(string type)
        {
            if (!Enum.TryParse(type, out CharacterMotionType result))
            {
                throw new ArgumentException("Cannot find proper animation name.");
            }

            InvokeAnimation(result);
        }

        public void InvokeAnimation(CharacterMotionType type)
        {
            Target.SetType(type);
        }

        private void Initialize()
        {
            _renderer = Target.GetComponent<Renderer>();
            Target.SetAnimationQueue(this);
            Target.SetDeathMotionAnimator(DeathMotionAnimator);
            Target.SetAnimationStartEvent(AnimationStartEvent);
            Target.SetAnimationEndEvent(AnimationEndEvent);
            DeathMotionAnimator.SetAnimationQueue(this);
            DeathMotionAnimator.SetAnimationStartEvent(AnimationStartEvent);
            DeathMotionAnimator.SetAnimationEndEvent(AnimationEndEvent);
            DeathMotionAnimator.SetRenderer(_renderer);
        }
    }
}
