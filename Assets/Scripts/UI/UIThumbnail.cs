using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIThumbnail : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI viewText;
        [SerializeField] private TextMeshProUGUI commitText;

        private readonly List<string> commits = new()
        {
            "나락 가세요오오오!!!",
            "드디어 나락가네 ㅋㅋ",
            "진짜 나락에 가버린 {0}",
            "{0} 나나나락",
            "꼴아박습니다",
            "원래부터 나락이였으면 개추 ㅋㅋ",
            "시즌 32767번째 나락",
            "그렇게 사라지셨다...",
            "나~락",
            "끝이다. 범부여.",
        };

        public void Init(string fileNameWithoutExtension, Sprite value)
        {
            gameObject.SetActive(true);
            image.sprite = value;
            titleText.text = $"{fileNameWithoutExtension}, 당신도 나락에 갈 수 있다";
            viewText.text = $"조회수 {Random.Range(50, 150)}만회";
            commitText.text = string.Format(commits.SelectOne(), fileNameWithoutExtension);
        }
    }
}