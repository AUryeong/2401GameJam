using System.Collections;
using KoreanTyper;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIIntro : WaitBehaviour
    {
        [SerializeField] private TextMeshProUGUI introText;

        private const char CURSOR_CHAR = '|';
        private const string INTRO_TEXT = "'Cancel Culture'\n: 유명인이나 공적 지위가 있는 사람이 논쟁이 될 만한 행동이나 발언을 했을 때,\nSNS나 인터넷 커뮤니티 등을 통해 대중의 공격을 받고\n지위나 직업을 박탈하려는 캠페인의 대상이 되는 현상, 즉 나락.";
        private const string INTRO_TITLE_TEXT = "\n\n\n나락퀴즈쇼";

        public override void Active()
        {
            base.Active();
            introText.text = string.Empty;
        }
        
        public override IEnumerator Wait(string parameter = "")
        {
            string str = INTRO_TEXT;
            string typingText = string.Empty;

            var waitForCursor = new WaitForSeconds(0.25f);
            var waitForTyping = new WaitForSeconds(0.035f);

            for (int waitCount = 0; waitCount < 4; waitCount++)
            {
                introText.text = typingText + CURSOR_CHAR;
                yield return waitForCursor;
                introText.text = typingText;
                yield return waitForCursor;
            }

            SoundManager.Instance.PlaySound("Keyboard_Typing", SoundType.Bgm);
            int strLength = str.GetTypingLength();
            for (int i = 0; i <= strLength; i++)
            {
                if (Input.GetKey(KeyCode.F)) break;
                    
                typingText = str.Typing(i);
                introText.text = typingText + (i % 10 >= 5 ? CURSOR_CHAR : "");
                yield return waitForTyping;
            }

            introText.text = str;

            SoundManager.Instance.PlaySound("", SoundType.Bgm);

            yield return new WaitForSeconds(2);

            introText.fontStyle = FontStyles.Normal;
            introText.text = str + INTRO_TITLE_TEXT;
            SoundManager.Instance.PlaySound("Ding", SoundType.Sfx);
            
            yield return new WaitForSeconds(2);
        }
    }
}