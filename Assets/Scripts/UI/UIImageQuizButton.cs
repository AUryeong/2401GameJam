using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIImageQuizButton : MonoBehaviour
    {
        private Button button;
        private int index;
        
        [SerializeField] private TextMeshProUGUI optionText;

        private UIImageQuizPopup popup;

        private void Awake()
        {
            popup = UIManager.Instance.Get(nameof(UIImageQuizPopup)) as UIImageQuizPopup;
            
            button = GetComponent<Button>();
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(SelectButton);
        }

        public void SetButton(int idx, string text)
        {
            index = idx;
            optionText.text = text;
        }

        private void SelectButton()
        {
            popup.Select(index);
        }
    }
}