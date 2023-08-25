using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamOdd.Ratocalypse.Animation
{
    public class CharacterAnimationQueue : MonoBehaviour
    {
        private readonly Queue<object> _queue = new Queue<object>();
        private Coroutine _coroutine;

        public void Awake()
        {
            Debug.Log("Character Animation Queue Initialized.");
        }

        public Coroutine AwaitStopCoroutine()
        {
            return _coroutine;
        }

        public void StopCoroutineNow()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _queue.Clear();
            _coroutine = null;
        }

        public Coroutine AddCallback(Action callback)
        {
            _queue.Enqueue(callback);

            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(ExhaustQueueItems());
            }

            return _coroutine;
        }

        public Coroutine AddCallback(Func<IEnumerator> coroutineCallback)
        {
            _queue.Enqueue(coroutineCallback);

            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(ExhaustQueueItems());
            }

            return _coroutine;
        }

        private IEnumerator ExhaustQueueItems()
        {
            while (_queue.Count != 0)
            {
                var callback = _queue.Dequeue();
                if (callback is Action action)
                {
                    action();
                    yield return null;
                    continue;
                }
                
                if (callback is Func<IEnumerator> coroutineCallback)
                {
                    var coroutineMethod = coroutineCallback();
                    yield return StartCoroutine(coroutineMethod);
                    continue;
                }

                throw new Exception("Invalid type");
            }

            _coroutine = null;
        }
    }
}
