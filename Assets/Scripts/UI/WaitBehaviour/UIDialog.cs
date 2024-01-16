using System.Collections;
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
                nameText.text = DataManager.Instance.GetFormat(dialog.nameText);
            
            scriptBase.gameObject.SetActive(!dialog.scriptText.IsEmptyOrWhiteSpace());
            scriptText.text = string.Empty;
        }

        public override IEnumerator Wait(string parameter = "")
        {
            if (dialog.cameraPos.Count > 0)
                CameraManager.Instance.ChangeDisplay(dialog.cameraPos);

            foreach (var character in dialog.characters)
            {
                var faceTexture = DataManager.Instance.characterFaceDict[$"{character.name}_{character.face}"];
                DataManager.Instance.characterMaterials[character.name].mainTexture = faceTexture;
            }

            if (!dialog.scriptText.IsEmptyOrWhiteSpace())
            {
                string str = DataManager.Instance.GetFormat(dialog.scriptText);
                int strLength = str.GetTypingLength();
                int prevLength = 0;
                for (int i = 1; i <= strLength; i++)
                {
                    string typingText = str.Typing(i);
                    scriptText.text = typingText;

                    if (Input.GetKey(KeyCode.F))
                        break;

                    if (prevLength < typingText.Length)
                    {
                        SoundManager.Instance.PlaySound("Dialog", SoundType.Sfx, 1, 1.5f);
                        prevLength = typingText.Length;
                    }

                    if (typingText[^1] is ',' or '.')
                        yield return WAIT_FOR_COMMA;
                    yield return WAIT_FOR_TYPING;
                }
                scriptText.text = str;
            }


            if (!Input.GetKey(KeyCode.F))
            {
                yield return WAIT_FOR_DIALOG;

                if (dialog.waitSeconds > 0)
                    yield return new WaitForSeconds(dialog.waitSeconds);
            }

            if (dialog.functionClassName.IsEmptyOrWhiteSpace()) yield break;

            switch (dialog.functionClassName)
            {
                case nameof(Dialog):
                    var uiDialog = UIManager.Instance.Get(nameof(UIDialog)) as UIDialog;
                    uiDialog.Active();
                    foreach (var newDialog in DataManager.Instance.GetDialogs(dialog.functionParameter).dialogs)
                    {
                        uiDialog.SetDialog(newDialog);
                        yield return uiDialog.Wait();
                    }
                    yield break;
                case "Sound":
                    SoundManager.Instance.PlaySound(dialog.functionParameter);
                    yield break;
                case "DisableTV":
                    UIManager.Instance.DeActiveTV();
                    yield break;
            }
            
            var uiWait = UIManager.Instance.Get(dialog.functionClassName.Trim());
            
            uiWait.Active();
            yield return uiWait.Wait(dialog.functionParameter);
        }
    }
}