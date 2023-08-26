using System;
using UnityEngine;

namespace TeamOdd.Ratocalypse.Animation
{
    public class CharacterMotion : CharacterAnimationQueue
    {
        public KeyCode AttacKeyCode = KeyCode.A;
        public KeyCode DamageKeyCode = KeyCode.D;
        public KeyCode DeathKeyCode = KeyCode.F;
        
        public GameObject Mosaic;
        public CharacterMotionTarget Target;
        public DeathMotionAnimator DeathMotionAnimator;
        
        private bool _initialized;
        private Renderer _renderer;
        
        public new void Awake()
        {
            base.Awake();
            Initialize();
            Mosaic.SetActive(false);
        }
        public void Update()
        {   
            if (Input.GetKeyDown(AttacKeyCode))
            {
                InvokeAnimation("Attack",
                    () => OnAnimationStart(CharacterMotionType.Attack),
                    () => OnAnimationEnd(CharacterMotionType.Attack));
            }
            else if (Input.GetKeyDown(DamageKeyCode))
            {
                InvokeAnimation("Damage",
                    () => OnAnimationEnd(CharacterMotionType.Damage));
            }
            else if (Input.GetKeyDown(DeathKeyCode))
            {
                InvokeAnimation("Death",
                    () => OnAnimationEnd(CharacterMotionType.Death));
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

        public void InvokeAnimation(string type, params Action[] callbacks)
        {
            if (!Enum.TryParse(type, out CharacterMotionType parseType))
            {
                throw new ArgumentException("Cannot find proper animation name.");
            }

            Target.InvokeAnimation(parseType, callbacks);
        }

        public void InvokeAnimation(CharacterMotionType type, params Action[] callbacks)
        {
            Target.InvokeAnimation(type, callbacks);
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _renderer = Target.GetComponent<Renderer>();
            Target.SetAnimationQueue(this);
            Target.SetDeathMotionAnimator(DeathMotionAnimator);
            DeathMotionAnimator.SetMosaic(Mosaic);
            DeathMotionAnimator.SetAnimationQueue(this);
            DeathMotionAnimator.SetRenderer(_renderer);
            Mosaic.SetActive(false);
            _initialized = true;
        }
    }
}
