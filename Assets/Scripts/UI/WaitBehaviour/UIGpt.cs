using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIGpt : TVWaitBehaviour
    {
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private TextMeshProUGUI gptText;
        
        private const string QUESTION_TEXT = "'서울 디지텍 고등학교' 학생 '{0}'에 대해 알려줘";
        private const string GPT_TEXT = "{0}은(는) 서울 디지텍 고등학교 설립부터 있던 학생입니다. {0}은(는) 천재적인 전략과 강력한 무력을 바탕으로 서울 디지텍 고등학교의 명성을 위해 500개가 넘는 학교를 재패하며 학교를 홍보하였습니다. 또한, {0}은(는) 프로젝트를 일부로 망가트리길 좋아하는 엄의 기질을 가지고 있습니다. {0}의 엄 능력은 갑옷 엄인 김유찬 또한 경외할 정도입니다. {0}의 이야기는 오늘날에도 여전히 서울 디지텍 고등학교에서 전설로 전해지고 있습니다.";
        
        private IEnumerator CreateGpt()
        {
            SoundManager.Instance.PlaySound("Alarm");
            
            questionText.text = string.Format(QUESTION_TEXT, DataManager.Instance.playerName);
            gptText.text = string.Empty;

            yield return new WaitForSeconds(1);
            
            var waitForTyping = new WaitForSeconds(0.035f);
            string str = string.Format(GPT_TEXT, DataManager.Instance.playerName);
            for (int i = 1; i <= str.Length; i += Random.Range(1, 6))
            {
                gptText.text = str.Substring(0, i);
                yield return waitForTyping;
            }
            gptText.text = str;
            yield return new WaitForSeconds(2);
        }
        public override IEnumerator Wait(string parameter = "") // GPT가 얘기하는동안 플레이어가 쉬지는 않음.
        {
            gameObject.SetActive(true);
            yield return null;
        }

        private void Start()
        {
            StartCoroutine(CreateGpt());
        }
    }
}