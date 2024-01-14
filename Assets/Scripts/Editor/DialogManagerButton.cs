using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataManager))]
public class DialogManagerButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DataManager manager = (DataManager)target;
        if (GUILayout.Button(nameof(manager.LoadData)))
            manager.LoadData();
    }
}