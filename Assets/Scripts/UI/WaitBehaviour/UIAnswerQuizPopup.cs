using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class UIAnswerQuizPopup : WaitBehaviour
    {
        [SerializeField] private Image quizBase;
        [SerializeField] private TextMeshProUGUI titleText;

        [SerializeField] private TMP_InputField inputField;
        private string inputAnswer = string.Empty;

        public override void Active()
        {
            base.Active();
            quizBase.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
                TypingText();
        }

        public override IEnumerator Wait(string parameter = "")
        {
            SoundManager.Instance.PlaySound("Quiz", SoundType.Sfx);
            SoundManager.Instance.PlaySound("Quiz_Bgm", SoundType.Bgm, 0.2f);

            UIManager.Instance.Get(nameof(UIDialog)).DeActive();

            yield return new WaitForSeconds(2);

            inputAnswer = string.Empty;
            quizBase.gameObject.SetActive(true);

            var quizTexts = parameter.Split('\\');
            var answer = quizTexts[1];

            titleText.text = quizTexts[0];

            inputField.gameObject.SetActive(true);

            int cameraChangeIdx = 0;
            var waitForChangeCamera = new WaitForSeconds(4.5f);
            while (inputAnswer.IsEmptyOrWhiteSpace())
            {
                cameraChangeIdx = (cameraChangeIdx + 1) % 3;
                switch (cameraChangeIdx)
                {
                    case 0:
                        CameraManager.Instance.ChangeDisplay(new List<CameraType>() { CameraType.Alice, CameraType.Other });
                        break;
                    case 1:
                        CameraManager.Instance.ChangeDisplay(CameraType.Player);
                        break;
                    case 2:
                        CameraManager.Instance.ChangeDisplay(CameraType.Default);
                        break;
                }

                yield return waitForChangeCamera;
            }

            yield return new WaitForSeconds(0.5f);

            CameraManager.Instance.ChangeDisplay(CameraType.Default);

            var uiSpecialText = UIManager.Instance.Get(nameof(UISpecialText));
            uiSpecialText.Active();
            yield return uiSpecialText.Wait($"{DataManager.Instance.PlayerName}의 선택 <#000000>{inputAnswer}");

            yield return new WaitForSeconds(0.5f);

            var uiDialog = UIManager.Instance.Get(nameof(UIDialog)) as UIDialog;
            uiDialog.Active();
            uiDialog.SetDialog(new Dialog()
            {
                nameText = "후와",
                scriptText = $"{DataManager.Instance.PlayerName}님께서 말씀해주신, {inputAnswer}.",
                waitSeconds = 1f,
                cameraPos = new List<CameraType>() { CameraType.MC },
                characters = new List<Dialog.Character>()
                {
                    new()
                    {
                        name = "Huwa",
                        face = Dialog.Character.FaceType.Surprise
                    },
                    new()
                    {
                        name = "Alice",
                        face = Dialog.Character.FaceType.Surprise
                    }
                }
            });
            yield return uiDialog.Wait();
            SoundManager.Instance.PlaySound("", SoundType.Bgm);
            bool flag = inputAnswer.Trim() == answer.Trim();
            foreach (var dialog in DataManager.Instance.GetDialogs($"{answer}_{(flag ? "정답" : "실패")}").dialogs)
            {
                uiDialog.SetDialog(dialog);
                yield return uiDialog.Wait();
            }
        }

        public void TypingText()
        {
            inputAnswer = inputField.text;
            inputField.gameObject.SetActive(false);
            quizBase.gameObject.SetActive(false);
            SoundManager.Instance.PlaySound("Alarm");
        }
    }
}