using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITextQuizButton : MonoBehaviour
    {
        private Button button;
        private int index;
        
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private TextMeshProUGUI optionText;

        private UITextQuizPopup popup;

        private void Awake()
        {
            popup = UIManager.Instance.Get(nameof(UITextQuizPopup)) as UITextQuizPopup;
            
            button = GetComponent<Button>();
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(SelectButton);
        }

        public void SetButton(int idx, string text)
        {
            index = idx;
            optionText.text = text;
            numberText.text = (index+1).ToString();
        }

        private void SelectButton()
        {
            popup.Select(index);
        }
    }
}