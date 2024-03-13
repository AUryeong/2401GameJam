using System;
using System.IO;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThumbnailManager : MonoBehaviour
{
    [SerializeField] private UIThumbnail originThumbnail;
    [SerializeField] private RectTransform thumbnailParent;

    [SerializeField] private Button exitButton;

    private void Awake()
    {
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(Reset);
        
        GetArtWorks(new DirectoryInfo($"{Application.dataPath}/Thumbnail"));
    }

    private void Reset()
    {
        SceneManager.LoadScene("OutGame");
    }
    
    private void GetArtWorks(DirectoryInfo dir)
    {
        foreach (var fileInfo in dir.GetFiles())
        {
            if (Path.GetExtension(fileInfo.FullName) != ".png") continue;
            
            Texture2D texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(File.ReadAllBytes(fileInfo.FullName));
            Sprite value = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0f, 0f));
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            Instantiate(originThumbnail, thumbnailParent).Init(fileNameWithoutExtension, value);
        }
    }
}