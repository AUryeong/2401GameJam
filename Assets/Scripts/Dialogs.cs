using System.Collections.Generic;

[System.Serializable]
public class Dialogs
{
    public string sheetName;
    public long sheetID;
    public List<Dialog> dialogs = new();
}

[System.Serializable]
public class Dialog
{
    public string nameText;
    public string scriptText;

    public List<CameraType> cameraPos = new();
    public float waitSeconds;
    
    public string functionClassName;
    public string functionParameter;

    public List<Character> characters = new();

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