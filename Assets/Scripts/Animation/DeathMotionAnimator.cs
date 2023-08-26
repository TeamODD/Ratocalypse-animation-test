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
        private const float FrameDuration = 1.0f / PrefabSize;

        public GameObject Models;
        public UnityEvent TargetDeadEvent;
        public GameObject Prefab;
        public float ObjectScale = 1.0f;

        private readonly List<GameObject> _prefabList = new List<GameObject>();
        private CharacterAnimationQueue _animationQueue;
        private Renderer _renderer;
        private GameObject _mosaic;

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

        public void SetMosaic(GameObject mosaic)
        {
            _mosaic = mosaic;
        }

        public void StartAnimation(params Action[] callbacks)
        {
            ResetPrefabList();
            _animationQueue.AddCallback(() => { return AnimationCoroutine(callbacks); });
        }

        private void InvokeTargetDeadEvents()
        {
            TargetDeadEvent.Invoke();
        }


        private IEnumerator AnimationCoroutine(params Action[] callbacks)
        {
            _mosaic.SetActive(true);
            var attackAnimationEndCallback = callbacks[0];
            InvokeTargetDeadEvents();
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
            _mosaic.SetActive(false);
            attackAnimationEndCallback();
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
