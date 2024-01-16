using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITextQuizPopup : WaitBehaviour
    {
        [SerializeField] private Image quizBase;
        [SerializeField] private TextMeshProUGUI titleText;

        [SerializeField] private UITextQuizButton originOptionButton;
        private List<UITextQuizButton> optionButtons = new();
        [SerializeField] private RectTransform optionParent;
        private int selectIndex = -1;

        public override void Active()
        {
            base.Active();
            quizBase.gameObject.SetActive(false);
            DisableOption();
        }

        private void DisableOption()
        {
            foreach (var button in optionButtons)
                button.gameObject.SetActive(false);
        }

        private List<UITextQuizButton> GetOption(int count)
        {
            DisableOption();
            if (optionButtons.Count < count)
            {
                for (int i = optionButtons.Count; i < count; i++)
                {
                    var optionButton = Instantiate(originOptionButton, optionParent);
                    optionButtons.Add(optionButton);
                }
            }

            var resultButtons = new List<UITextQuizButton>();
            for (int i = 0; i < count; i++)
            {
                resultButtons.Add(optionButtons[i]);
                optionButtons[i].gameObject.SetActive(true);
            }

            return resultButtons;
        }

        public override IEnumerator Wait(string parameter = "")
        {
            selectIndex = -1;
            quizBase.gameObject.SetActive(true);

            UIManager.Instance.Get(nameof(UIDialog)).DeActive();

            var quizTexts = parameter.Split('\\');
            var quizSelects = quizTexts[1].Split(',');

            titleText.text = quizTexts[0];

            var optionButtonList = GetOption(quizSelects.Length);
            for (var i = 0; i < optionButtonList.Count; i++)
            {
                var optionButton = optionButtonList[i];
                optionButton.SetButton(i, quizSelects[i]);
            }

            int cameraChangeIdx = 0;
            var waitForChangeCamera = new WaitForSeconds(3);
            while (selectIndex < 0)
            {
                cameraChangeIdx = (cameraChangeIdx + 1) % 3;
                switch (cameraChangeIdx)
                {
                    case 0:
                        CameraManager.Instance.ChangeDisplay(CameraType.Player); 
                        break;
                    case 1:
                        CameraManager.Instance.ChangeDisplay(CameraType.Default);
                        break;
                    case 2:
                        CameraManager.Instance.ChangeDisplay(new List<CameraType>() { CameraType.TV, CameraType.Other});
                        break;
                }

                yield return waitForChangeCamera;
            }

            yield return new WaitForSeconds(0.5f);

            CameraManager.Instance.ChangeDisplay(CameraType.Default);

            var uiSpecialText = UIManager.Instance.Get(nameof(UISpecialText));
            uiSpecialText.Active();
            yield return uiSpecialText.Wait($"{DataManager.Instance.PlayerName}의 선택 <#000000>{selectIndex + 1}번 {quizSelects[selectIndex]}");

            yield return new WaitForSeconds(0.5f);

            var uiDialog = UIManager.Instance.Get(nameof(UIDialog)) as UIDialog;
            uiDialog.Active();
            uiDialog.SetDialog(new Dialog()
            {
                nameText = "후와",
                scriptText = $"{DataManager.Instance.PlayerName}님께서 말씀해주신, {selectIndex + 1}번. {quizSelects[selectIndex]}",
                waitSeconds = 1f,
                cameraPos = new List<CameraType>() { CameraType.MC },
                characters = new List<Dialog.Character>() { 
                    new Dialog.Character() { 
                        name = "Huwa", 
                        face = Dialog.Character.FaceType.Surprise
                    },
                    new Dialog.Character() {
                        name = "Alice",
                        face = Dialog.Character.FaceType.Surprise
                    }
                }
            });
            foreach (var dialog in DataManager.Instance.GetDialogs(quizSelects[selectIndex]).dialogs)
            {
                uiDialog.SetDialog(dialog);
                yield return uiDialog.Wait();
            }
        }

        public void Select(int index)
        {
            selectIndex = index;
            quizBase.gameObject.SetActive(false);
            SoundManager.Instance.PlaySound("Alarm");
        }
    }
}