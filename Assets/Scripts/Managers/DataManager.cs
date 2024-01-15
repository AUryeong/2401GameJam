using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DataManager : Singleton<DataManager>
{
    protected override bool IsDontDestroying => true;

    private const string ADDRESS = "https://docs.google.com/spreadsheets/d/1NUUnMQfpXFIh2Wmgy-hX5WjjPMgybgbNWJ_tj2-QqAQ";
    private const string RANGE = "A2:K";
    private const long SHEET_ID = 0;

    public string playerName = "서경훈";
    public List<Dialog> dialogs = new()
        ;
    public readonly Dictionary<string, Texture> characterFaceDict = new();
    public readonly Dictionary<string, Material> characterMaterials = new();

    protected override void OnCreated()
    {
        base.OnCreated();
        
        characterFaceDict.Clear();
        var textures = Resources.LoadAll<Texture>("Characters");
        foreach (var texture in textures)
            characterFaceDict.Add(texture.name, texture);
        
        characterMaterials.Clear();
        var materials = Resources.LoadAll<Material>("Characters");
        foreach (var material in materials)
            characterMaterials.Add(material.name, material);
        
        LoadData();
    }

    public void ReLoadData()
    {
        StartCoroutine(ReloadDataCoroutine());
    }

    private IEnumerator ReloadDataCoroutine()
    {
        yield return LoadDataCoroutine();
        SceneManager.LoadScene("InGame");
    }

    public void LoadData()
    {
        StartCoroutine(LoadDataCoroutine());
    }

    private IEnumerator LoadDataCoroutine()
    {
        dialogs.Clear();

        var www = UnityWebRequest.Get($"{ADDRESS}/export?format=tsv&range={RANGE}&gid={SHEET_ID}");
        yield return www.SendWebRequest();

        string text = www.downloadHandler.text;

        var lines = text.Split('\n');
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line)) yield break;

            var columns = line.Split('\t');
            var dialog = new Dialog();
            dialogs.Add(dialog);

            if (!string.IsNullOrEmpty(columns[0]))
                dialog.nameText = columns[0];

            if (!string.IsNullOrEmpty(columns[1]) && columns.Length >= 2)
                dialog.scriptText = columns[1];

            if (!string.IsNullOrEmpty(columns[2]) && columns.Length >= 3)
                dialog.cameraPos = Utility.GetEnum<CameraType>(columns[2]);

            if (!string.IsNullOrEmpty(columns[3]) && columns.Length >= 4)
                dialog.waitSeconds = float.Parse(columns[3]);

            if (!string.IsNullOrEmpty(columns[4]) && columns.Length >= 5)
                dialog.functionClassName = columns[4];

            for (int i = 5; i < columns.Length; i += 2)
            {
                if (string.IsNullOrEmpty(columns[i]) || columns.Length <= i+1) break;
                
                dialog.characters.Add(new Dialog.Character()
                {
                    name = columns[i],
                    face = Utility.GetEnum<Dialog.Character.FaceType>(columns[i + 1])
                });
            }
        }
    }
}

[System.Serializable]
public class Dialog
{
    public string nameText;
    public string scriptText;

    public CameraType cameraPos;
    public float waitSeconds;
    public string functionClassName;

    public List<Character> characters = new List<Character>();

    [System.Serializable]
    public class Character
    {
        public string name;
        public FaceType face;
        public enum FaceType
        {
            Default,
            Happy,
            Angry,
            Surprise
        }
    }
}