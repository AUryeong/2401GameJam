using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OutGameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button nameButton;

    [SerializeField] private Image fadeInOutImage;
    [SerializeField] private Image[] narrationImages;

    private const float FADE_IN_OUT_DURATION = 2;

    private void Awake()
    {
        nameButton.onClick.RemoveAllListeners();
        nameButton.onClick.AddListener(SelectName);

        fadeInOutImage.gameObject.SetActive(false);

        foreach (var image in narrationImages)
            image.gameObject.SetActive(false);
    }

    private void Start()
    {
        SoundManager.Instance.PlaySound("Wind", ESoundType.Bgm);
    }

    private void SelectName()
    {
        SoundManager.Instance.PlaySound("Button");
        DataManager.Instance.playerName = nameInput.text;
        StartCoroutine(SelectNameCoroutine());
    }

    private IEnumerator SelectNameCoroutine()
    {
        fadeInOutImage.color = fadeInOutImage.color.GetChangeAlpha(0);
        fadeInOutImage.gameObject.SetActive(true);

        foreach (var image in narrationImages)
        {
            yield return fadeInOutImage.DOFade(1, FADE_IN_OUT_DURATION).WaitForCompletion();
            image.gameObject.SetActive(true);
            yield return fadeInOutImage.DOFade(0, FADE_IN_OUT_DURATION).WaitForCompletion();
            yield return new WaitForSeconds(FADE_IN_OUT_DURATION / 2);
        }

        yield return fadeInOutImage.DOFade(1, FADE_IN_OUT_DURATION * 2).WaitForCompletion();
        SoundManager.Instance.PlaySound("", ESoundType.Bgm);
        yield return new WaitForSeconds(FADE_IN_OUT_DURATION / 2);

        SceneManager.LoadScene("InGame");
    }
}