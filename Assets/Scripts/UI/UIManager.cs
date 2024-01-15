using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class WaitBehaviour : MonoBehaviour
    {
        public abstract IEnumerator Wait();

        public virtual void Active()
        {
            gameObject.SetActive(true);
        }

        public virtual void DeActive()
        {
            gameObject.SetActive(false);
        }
    }

    public abstract class TVWaitBehaviour : WaitBehaviour
    {
    }

    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Image fadeInOutImg;
        [SerializeField] private List<WaitBehaviour> waitBehaviours;
        private Dictionary<string, WaitBehaviour> waitBehaviourDict = new();

        private const float FADE_ALPHA_DURATION = 1f;

        protected override void OnCreated()
        {
            base.OnCreated();
            foreach (var behaviour in waitBehaviours)
            {
                behaviour.gameObject.SetActive(false);
                waitBehaviourDict.Add(behaviour.GetType().Name.Replace("UI", ""), behaviour);
            }
        }

        public WaitBehaviour Get(string waitName)
        {
            return waitBehaviourDict[waitName.Replace("UI", "")];
        }

        public void DeActiveTV()
        {
            foreach (var waitBehaviour in waitBehaviours.OfType<TVWaitBehaviour>())
                waitBehaviour.DeActive();
        }

        public IEnumerator FadeIn()
        {
            fadeInOutImg.color = fadeInOutImg.color.GetChangeAlpha(0);
            yield return fadeInOutImg.DOFade(1, FADE_ALPHA_DURATION).WaitForCompletion();
        }

        public IEnumerator FadeOut()
        {
            fadeInOutImg.color = fadeInOutImg.color.GetChangeAlpha(1);
            yield return fadeInOutImg.DOFade(0, FADE_ALPHA_DURATION).WaitForCompletion();
        }
    }
}