using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TeamOdd.Ratocalypse.Animation
{
    public class DeathMotionAnimator : MonoBehaviour
    {
        private const int PrefabSize = 6;
        private const int FadeOutDuration = 3;
        private const int FadeOutSteps = 30;

        public GameObject Prefab;

        private readonly List<GameObject> _prefabList = new List<GameObject>();
        private UnityEvent<CharacterMotionType> _animationStartEvent;
        private UnityEvent<CharacterMotionType> _animationEndEvent;
        private Coroutine _animationCoroutine;
        private Renderer _renderer;

        public void Awake()
        {
            _prefabList.Clear();
            _animationCoroutine = null;
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
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            _animationCoroutine = StartCoroutine(AnimationCoroutine());
        }

        private IEnumerator AnimationCoroutine()
        {
            _animationStartEvent.Invoke(CharacterMotionType.Death);

            while (_prefabList.Count != PrefabSize)
            {
                var instance = Instantiate(Prefab, transform.position, Quaternion.identity);
                instance.transform.Rotate(Vector3.forward, 90f);
                _prefabList.Add(instance);
                yield return new WaitForSeconds(0.5f);
                Destroy(instance);
            }

            _prefabList.Clear();
            yield return StartCoroutine(FadeOutCoroutine());
            _animationEndEvent.Invoke(CharacterMotionType.Death);
        }

        private IEnumerator FadeOutCoroutine()
        {
            yield return null;
        }
    }
}
