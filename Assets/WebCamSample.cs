using UnityEngine;

public class WebCamSample : MonoBehaviour
{
    [SerializeField] private Renderer meshRenderer;
    private WebCamTexture camTexture;
    private const int CAM_INDEX = 1;

    private void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        camTexture = new WebCamTexture(devices[CAM_INDEX].name);
        
        if (!camTexture.isPlaying) camTexture.Play();
        meshRenderer.material.mainTexture = camTexture;
    }
}