using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITextQuiz : TVWaitBehaviour
    {
        [SerializeField] private Image titleBase;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI scriptText;

        public override void Active()
        {
            base.Active();
            titleBase.color = titleBase.color.GetChangeAlpha(0);
            titleText.color = titleText.color.GetChangeAlpha(0);
            scriptText.color = scriptText.color.GetChangeAlpha(0);
        }

        public override IEnumerator Wait(string parameter = "")
        {
            var quizTexts = parameter.Split('\\');
            var quizSelects = quizTexts[1].Split(',');

            titleText.text = quizTexts[0];
            string script = string.Empty;
            for (int i = 0; i < quizSelects.Length; i++)
                script += $"{i + 1}.{quizSelects[i]}\n";

            scriptText.text = script;
            yield return new WaitForSeconds(1);
            yield return titleBase.DOFade(1, 1).WaitForCompletion();
            yield return titleText.DOFade(1, 1).WaitForCompletion();
            
            SoundManager.Instance.PlaySound("Alarm");
            yield return scriptText.DOFade(1, 1).WaitForCompletion();
            
            var wait = UIManager.Instance.Get(nameof(UITextQuizPopup));
            wait.Active();
            yield return wait.Wait(parameter);
        }
    }
}