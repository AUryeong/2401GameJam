using System.Collections;
using UI;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        StartCoroutine(GameCoroutine());
    }

    private IEnumerator GameCoroutine()
    {
        var uiManager = UIManager.Instance;
        
        var wait = uiManager.Get(nameof(UIIntro));
        wait.Active();
        
        yield return uiManager.FadeOut();
        
        yield return wait.Wait();
        CameraManager.Instance.ChangeDisplay(CameraType.MC);
        yield return uiManager.FadeIn();
        
        wait.DeActive();
        
        yield return uiManager.FadeOut();

        var uiDialog = uiManager.Get(nameof(UIDialog)) as UIDialog;
        uiDialog.Active();
        foreach (var dialog in DataManager.Instance.GetDialogs("시작").dialogs)
        {
            uiDialog.SetDialog(dialog);
            yield return uiDialog.Wait();
        }
    }
}