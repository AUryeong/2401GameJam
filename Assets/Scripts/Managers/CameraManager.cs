using System.Collections.Generic;
using UnityEngine;

public enum CameraType
{
    None,
    Default,
    MC,
    Player,
    TV
}

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<Camera> cameraList;

    protected override void OnCreated()
    {
        base.OnCreated();
        foreach (var cam in cameraList)
            cam.enabled = false;
    }

    public void ChangeDisplay(CameraType type)
    {
        var cam = cameraList[(int)type-1];
        mainCamera.cullingMask = cam.cullingMask;
        mainCamera.transform.position = cam.transform.position;
        mainCamera.transform.rotation = cam.transform.rotation;
    }
}