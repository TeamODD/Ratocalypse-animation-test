using System;
using UnityEngine;
using UnityEngine.Events;

namespace TeamOdd.Ratocalypse.Animation
{
    public class CharacterMotion : MonoBehaviour
    {
        public KeyCode AttacKeyCode = KeyCode.A;
        public KeyCode DamageKeyCode = KeyCode.D;
        public KeyCode DeathKeyCode = KeyCode.F;

        public CharacterMotionTarget Target;
        public DeathMotionAnimator DeathMotionAnimator;
        public UnityEvent<CharacterMotionType> AnimationStartEvent;
        public UnityEvent<CharacterMotionType> AnimationEndEvent;

        private Renderer _renderer;

        public void Awake()
        {
            Initialize();
        }

        public void Update()
        {
            if (Input.GetKeyDown(AttacKeyCode))
            {
                SetType(CharacterMotionType.Attack);
            }
            else if (Input.GetKeyDown(DamageKeyCode))
            {
                SetType(CharacterMotionType.Damage);
            }
            else if (Input.GetKeyDown(DeathKeyCode))
            {
                SetType(CharacterMotionType.Death);
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

        public void SetType(CharacterMotionType type)
        {
            Target.SetType(type);
        }

        private void Initialize()
        {
            _renderer = Target.GetComponent<Renderer>();
            Target.SetDeathMotionAnimator(DeathMotionAnimator);
            Target.SetAnimationStartEvent(AnimationStartEvent);
            Target.SetAnimationEndEvent(AnimationEndEvent);
            DeathMotionAnimator.SetAnimationStartEvent(AnimationStartEvent);
            DeathMotionAnimator.SetAnimationEndEvent(AnimationEndEvent);
            DeathMotionAnimator.SetRenderer(_renderer);
        }
    }
}
