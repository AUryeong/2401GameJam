using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITitle : WaitBehaviour
    {
        [SerializeField] private Image titleImage;

        public override IEnumerator Wait(string parameter = "")
        {
            UIManager.Instance.DeActiveTV();
            CameraManager.Instance.ChangeDisplay(CameraType.Default);
            SoundManager.Instance.PlaySound("Title");
            yield return new WaitForSeconds(1);

            titleImage.rectTransform.anchoredPosition = titleImage.rectTransform.anchoredPosition.GetChangeY(840);
            yield return titleImage.rectTransform.DOAnchorPosY(-840, 3).SetEase(Ease.Linear).WaitForCompletion();

            yield return new WaitForSeconds(1);
        }
    }
}