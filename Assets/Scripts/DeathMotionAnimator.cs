using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TeamOdd.Ratocalypse.Animation
{
    public class DeathMotionAnimator : MonoBehaviour
    {
        private const int FadeOutDuration = 3;
        private const int FadeOutSteps = 30;
        private const int PrefabSize = 1;
        private const float FrameDuration = 2.0f / PrefabSize;

        public GameObject Prefab;
        public float ObjectScale = 1.0f;

        private readonly List<GameObject> _prefabList = new List<GameObject>();
        private UnityEvent<CharacterMotionType> _animationStartEvent;
        private UnityEvent<CharacterMotionType> _animationEndEvent;
        private CharacterAnimationQueue _animationQueue;
        private Renderer _renderer;

        public void Awake()
        {
            _prefabList.Clear();
            _renderer = null;
        }

        public void SetAnimationQueue(CharacterAnimationQueue animationQueue)
        {
            _animationQueue = animationQueue;
        }

        public void SetAnimationStartEvent(UnityEvent<CharacterMotionType> animationStartEvent)
        {
            _animationStartEvent = animationStartEvent;
        }

        public void SetAnimationEndEvent(UnityEvent<CharacterMotionType> animationEndEvent)
        {
            _animationEndEvent = animationEndEvent;
        }

        public void SetRenderer(Renderer renderer)
        {
            _renderer = renderer;
        }

        public void StartAnimation()
        {
            ResetPrefabList();
            _animationQueue.AddCallback(() =>
            {
                return AnimationCoroutine();
            });
        }

        private IEnumerator AnimationCoroutine()
        {
            _animationStartEvent.Invoke(CharacterMotionType.Death);

            while (_prefabList.Count != PrefabSize)
            {
                var angle = Quaternion.AngleAxis(90f, Vector3.forward);
                var instance = Instantiate(Prefab, transform.position, angle);
                instance.transform.localScale *= ObjectScale;
                _prefabList.Add(instance);
                yield return new WaitForSeconds(FrameDuration);
                Destroy(instance);
            }

            _prefabList.Clear();
            _animationEndEvent.Invoke(CharacterMotionType.Death);
        }

        private void ResetPrefabList()
        {
            _prefabList.ForEach(Destroy);
            _prefabList.Clear();
        }

        private IEnumerator FadeOutCoroutine()
        {
            yield return null;
        }
    }
}
