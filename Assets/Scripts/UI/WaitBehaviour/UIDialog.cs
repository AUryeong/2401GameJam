using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIDialog : WaitBehaviour
    {
        private Dialog dialog;

        [SerializeField] private TextMeshProUGUI nameText;

        [SerializeField] private Image scriptBase;
        [SerializeField] private TextMeshProUGUI scriptText;

        private static readonly WaitForSeconds WAIT_FOR_TYPING = new(0.1f);
        private static readonly WaitForSeconds WAIT_FOR_COMMA = new(0.5f);
        private static readonly WaitForSeconds WAIT_FOR_DIALOG = new(0.5f);

        public void SetDialog(Dialog dialog)
        {
            this.dialog = dialog;
            nameText.text = string.Format(dialog.nameText, DataManager.Instance.playerName);
            scriptBase.gameObject.SetActive(!(string.IsNullOrEmpty(dialog.scriptText) || string.IsNullOrWhiteSpace(dialog.scriptText)));
            scriptText.text = string.Empty;
        }

        public override IEnumerator Wait()
        {
            if (dialog.cameraPos != CameraType.None)
                CameraManager.Instance.ChangeDisplay(dialog.cameraPos);

            string str = string.Format(dialog.scriptText, DataManager.Instance.playerName);
            for (int i = 1; i <= str.Length; i++)
            {
                scriptText.text = str[..i];
                SoundManager.Instance.PlaySound("Dialog", ESoundType.Sfx, 1, 1.5f);
                if (str[i - 1] is ',' or '.')
                    yield return WAIT_FOR_COMMA;
                yield return WAIT_FOR_TYPING;
            }

            scriptText.text = str;

            yield return WAIT_FOR_DIALOG;

            if (dialog.waitSeconds > 0)
                yield return new WaitForSeconds(dialog.waitSeconds);

            if (string.IsNullOrEmpty(dialog.functionClassName) || string.IsNullOrWhiteSpace(dialog.functionClassName)) yield break;

            yield return UIManager.Instance.Get(dialog.functionClassName.Trim()).Wait();
        }
    }
}