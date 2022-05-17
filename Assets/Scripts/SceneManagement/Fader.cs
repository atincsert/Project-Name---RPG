using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {        
        private CanvasGroup canvasGroup;
        private Coroutine currentlyActiveFade = null;

        private void Awake()
        {            
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmidiate()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);            
        }
        
        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }
        
        public Coroutine Fade(float target, float time)
        {
            // Cancel running coroutines
            if (currentlyActiveFade != null)
            {
                StopCoroutine(currentlyActiveFade);
            }
            // Run fadeout coroutine
            currentlyActiveFade = StartCoroutine(FadeRoutine(target, time));
            return currentlyActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target)) // alpha is not 1
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
                // update alpha
            }
        }
    }
}