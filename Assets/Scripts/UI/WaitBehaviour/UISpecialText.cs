using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class UISpecialText : WaitBehaviour
{
    [SerializeField] private TextMeshProUGUI specialText;
    public override IEnumerator Wait(string parameter = "")
    {
        specialText.gameObject.SetActive(true);
        specialText.text = parameter;

        specialText.color = specialText.color.GetChangeAlpha(0);
        yield return specialText.DOFade(1, 1.5f).WaitForCompletion();

        yield return new WaitForSeconds(2);

        yield return specialText.DOFade(0, 1.5f).WaitForCompletion();
    }
}
