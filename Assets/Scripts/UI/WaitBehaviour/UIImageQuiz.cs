using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIImageQuiz : TVWaitBehaviour
    {
        [SerializeField] private Image titleBase;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image peopleImg;

        public override void Active()
        {
            base.Active();
            titleBase.color = titleBase.color.GetChangeAlpha(0);
            titleText.color = titleText.color.GetChangeAlpha(0);
            peopleImg.color = peopleImg.color.GetChangeAlpha(0);
        }

        public override IEnumerator Wait(string parameter = "")
        {
            var quizTexts = parameter.Split('\\');
            var resourceName = quizTexts[1];

            titleText.text = quizTexts[0];
            peopleImg.sprite = DataManager.Instance.peopleDict[resourceName];
            peopleImg.SetNativeSize();

            yield return new WaitForSeconds(1);
            
            yield return titleBase.DOFade(1, 1).WaitForCompletion();
            yield return titleText.DOFade(1, 1).WaitForCompletion();
            
            SoundManager.Instance.PlaySound("Alarm");
            yield return peopleImg.DOFade(1, 1).WaitForCompletion();
            
            var wait = UIManager.Instance.Get(nameof(UIImageQuizPopup));
            wait.Active();
            yield return wait.Wait(parameter);
        }
    }
}