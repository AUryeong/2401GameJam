using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITitle : WaitBehaviour
    {
        [SerializeField] private Image titleImage;

        public override IEnumerator Wait()
        {
            UIManager.Instance.DeActiveTV();
            CameraManager.Instance.ChangeDisplay(CameraType.Default);
            SoundManager.Instance.PlaySound("Title");

            titleImage.rectTransform.anchoredPosition = titleImage.rectTransform.anchoredPosition.GetChangeY(840);
            yield return titleImage.rectTransform.DOAnchorPosY(-840, 6).SetEase(Ease.Linear).WaitForCompletion();

            yield return new WaitForSeconds(1);
        }
    }
}