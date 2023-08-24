using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamOdd.Ratocalypse.Animation
{
    public class DeathMotionAnimator : MonoBehaviour
    {
        private const int FadeOutDuration = 3;
        private const int FadeOutSteps = 30;
        private const int PrefabSize = 1;
        private const float FrameDuration = 1.0f / PrefabSize;

        public GameObject Prefab;
        public float ObjectScale = 1.0f;

        private readonly List<GameObject> _prefabList = new List<GameObject>();
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

        public void SetRenderer(Renderer renderer)
        {
            _renderer = renderer;
        }

        public void StartAnimation(params Action[] callbacks)
        {
            ResetPrefabList();
            _animationQueue.AddCallback(() => { return AnimationCoroutine(callbacks); });
        }

        private IEnumerator AnimationCoroutine(params Action[] callbacks)
        {
            var attackAnimationEndCallback = callbacks[0];

            while (_prefabList.Count != PrefabSize)
            {
                var angle = Quaternion.AngleAxis(90f, Vector3.forward);
                var instance = Instantiate(Prefab, transform.position, angle);
                instance.transform.localScale *= ObjectScale;
                _prefabList.Add(instance);
                yield return new WaitForSeconds(FrameDuration);
                Destroy(instance);
            }

            yield return StartCoroutine(FadeOutCoroutine());

            _prefabList.Clear();
            attackAnimationEndCallback();
        }

        private void ResetPrefabList()
        {
            _prefabList.ForEach(Destroy);
            _prefabList.Clear();
        }

        private IEnumerator FadeOutCoroutine()
        {
            // TODO
            yield return null;
        }
    }
}
