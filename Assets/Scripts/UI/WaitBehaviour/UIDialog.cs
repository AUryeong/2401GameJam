﻿using System.Collections;
using KoreanTyper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIDialog : WaitBehaviour
    {
        private Dialog dialog;

        [SerializeField] private Image nameBase;
        [SerializeField] private TextMeshProUGUI nameText;

        [SerializeField] private Image scriptBase;
        [SerializeField] private TextMeshProUGUI scriptText;

        private static readonly WaitForSeconds WAIT_FOR_TYPING = new(0.035f);
        private static readonly WaitForSeconds WAIT_FOR_COMMA = new(0.5f);
        private static readonly WaitForSeconds WAIT_FOR_DIALOG = new(0.5f);

        public void SetDialog(Dialog dialog)
        {
            this.dialog = dialog;

            nameBase.gameObject.SetActive(!dialog.nameText.IsEmptyOrWhiteSpace());
            if (!dialog.nameText.IsEmptyOrWhiteSpace())
                nameText.text = string.Format(dialog.nameText, DataManager.Instance.playerName);
            
            scriptBase.gameObject.SetActive(!dialog.scriptText.IsEmptyOrWhiteSpace());
            scriptText.text = string.Empty;
        }

        public override IEnumerator Wait()
        {
            if (dialog.cameraPos != CameraType.None)
                CameraManager.Instance.ChangeDisplay(dialog.cameraPos);

            foreach (var character in dialog.characters)
            {
                var faceTexture = DataManager.Instance.characterFaceDict[$"{character.name}_{character.face.ToString()}"];
                DataManager.Instance.characterMaterials[character.name].mainTexture = faceTexture;
            }

            if (!dialog.scriptText.IsEmptyOrWhiteSpace())
            {
                string str = string.Format(dialog.scriptText, DataManager.Instance.playerName);
                int strLength = str.GetTypingLength();
                int prevLength = 0;
                for (int i = 1; i <= strLength; i++)
                {
                    string typingText = str.Typing(i);
                    scriptText.text = typingText;

                    if (prevLength < typingText.Length)
                    {
                        SoundManager.Instance.PlaySound("Dialog", ESoundType.Sfx, 1, 1.5f);
                        prevLength = typingText.Length;
                    }

                    if (typingText[^1] is ',' or '.')
                        yield return WAIT_FOR_COMMA;
                    yield return WAIT_FOR_TYPING;
                }
                scriptText.text = str;
            }

            yield return WAIT_FOR_DIALOG;

            if (dialog.waitSeconds > 0)
                yield return new WaitForSeconds(dialog.waitSeconds);

            if (dialog.functionClassName.IsEmptyOrWhiteSpace()) yield break;

            var uiWait = UIManager.Instance.Get(dialog.functionClassName.Trim());
            
            uiWait.Active();
            yield return uiWait.Wait();
        }
    }
}