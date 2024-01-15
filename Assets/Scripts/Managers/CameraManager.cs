using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CameraType
{
    None,
    Default,
    MC,
    Player,
    TV,
    Other,
    Alice,
    Quiz
}

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Camera originCamera;

    private List<Camera> mainCameras = new();
    [SerializeField] private List<Camera> cameraList;

    protected override void OnCreated()
    {
        base.OnCreated();
        foreach (var cam in cameraList)
            cam.enabled = false;

        mainCameras.Add(originCamera);
    }

    private void DisableCameras()
    {
        foreach (var cam in mainCameras)
            cam.gameObject.SetActive(false);
    }

    private List<Camera> GetCamera(List<CameraType> cameraTypes)
    {
        DisableCameras();
        var distCameras = cameraTypes.Distinct().ToList();
        if (mainCameras.Count < distCameras.Count)
        {
            for (int i = mainCameras.Count; i < distCameras.Count; i++)
            {
                var cam = Instantiate(originCamera);
                mainCameras.Add(cam);
            }
        }

        var resultCameras = new List<Camera>();
        int indexAdder = 0;
        for (int i = 0; i < distCameras.Count; i++)
        {
            int count = cameraTypes.Count(x => x == distCameras[i]);
            var result = mainCameras[i];
            
            result.gameObject.SetActive(true);
            result.rect = new Rect((1f / cameraTypes.Count) * indexAdder, 0, 1f / cameraTypes.Count * count, 1);
            resultCameras.Add(result);
            
            indexAdder += count;
        }

        return resultCameras;
    }

    public void ChangeDisplay(CameraType type)
    {
        var cam = cameraList[(int)type - 1];
        var mainCamera = GetCamera(new List<CameraType>() { type })[0];
        mainCamera.cullingMask = cam.cullingMask;
        mainCamera.transform.position = cam.transform.position;
        mainCamera.transform.rotation = cam.transform.rotation;
    }

    public void ChangeDisplay(List<CameraType> types)
    {
        var mainCameras = GetCamera(types);
        for (var i = 0; i < mainCameras.Count; i++)
        {
            var cam = cameraList[(int)types[i] - 1];
            mainCameras[i].cullingMask = cam.cullingMask;
            mainCameras[i].transform.position = cam.transform.position;
            mainCameras[i].transform.rotation = cam.transform.rotation;
        }
    }
}