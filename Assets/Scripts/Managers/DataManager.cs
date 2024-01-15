using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : Singleton<DataManager>
{
    protected override bool IsDontDestroying => true;

    private const string ADDRESS = "https://docs.google.com/spreadsheets/d/1NUUnMQfpXFIh2Wmgy-hX5WjjPMgybgbNWJ_tj2-QqAQ";
    private const string RANGE = "A2:K";
    private const long DEFAULT_SHEET_ID = 853365801L;

    public string playerName = "서경훈";
    
    [SerializeField] private List<Dialogs> dialogList = new();
    private Dictionary<string, Dialogs> dialogDict = new();

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

        foreach (var dialogs in dialogList)
            dialogDict.Add(dialogs.sheetName, dialogs);

        LoadData();
    }

    public Dialogs GetDialogs(string dialogName)
    {
        return dialogDict[dialogName];
    }

    public void LoadData()
    {
        StartCoroutine(LoadDataCoroutine());
    }

    private IEnumerator LoadDataCoroutine()
    {
        var defaultWww = UnityWebRequest.Get($"{ADDRESS}/export?format=tsv&range={RANGE}&gid={DEFAULT_SHEET_ID}");
        yield return defaultWww.SendWebRequest();

        dialogList.Clear();
        
        string sheetData = defaultWww.downloadHandler.text;
        foreach (var line in sheetData.Split('\n'))
        {
            if (string.IsNullOrEmpty(line)) yield break;

            var columns = line.Split('\t');
            dialogList.Add(new Dialogs()
            {
                sheetName = columns[0],
                sheetID = long.Parse(columns[1])
            });
        }

        foreach (var dialogs in dialogList)
        {
            dialogs.dialogs.Clear();

            var www = UnityWebRequest.Get($"{ADDRESS}/export?format=tsv&range={RANGE}&gid={dialogs.sheetID}");
            yield return www.SendWebRequest();

            string text = www.downloadHandler.text;

            var lines = text.Split('\n');
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) yield break;

                var columns = line.Split('\t');
                var dialog = new Dialog();
                dialogs.dialogs.Add(dialog);

                if (!string.IsNullOrEmpty(columns[0]))
                    dialog.nameText = columns[0];

                if (!string.IsNullOrEmpty(columns[1]))
                    dialog.scriptText = columns[1];

                if (!string.IsNullOrEmpty(columns[2]))
                {
                    foreach (var cam in columns[2].Split('/'))
                    {
                        dialog.cameraPos.Add(Utility.GetEnum<CameraType>(cam));
                    }
                }

                if (!string.IsNullOrEmpty(columns[3]))
                    dialog.waitSeconds = float.Parse(columns[3]);

                if (!string.IsNullOrEmpty(columns[4]))
                {
                    var function = columns[4].Split('/');
                    dialog.functionClassName = function[0];
                    dialog.functionParameter = function.Length >= 2 ? function[1] : string.Empty;
                }

                for (int i = 5; i < columns.Length; i += 2)
                {
                    if (string.IsNullOrEmpty(columns[i])) break;

                    dialog.characters.Add(new Dialog.Character()
                    {
                        name = columns[i],
                        face = Utility.GetEnum<Dialog.Character.FaceType>(columns[i + 1])
                    });
                }
            }
        }
    }
}