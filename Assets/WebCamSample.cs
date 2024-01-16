using UnityEngine;

public class WebCamSample : MonoBehaviour
{
    private WebCamTexture camTexture;
    private const int CAM_INDEX = 0;

    private void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        camTexture = new WebCamTexture(devices[CAM_INDEX].name);

        if (!camTexture.isPlaying) camTexture.Play();
        GetComponent<MeshRenderer>().material.mainTexture = camTexture;

        var scale = transform.localScale;
        transform.localScale = new Vector3(scale.x * camTexture.texelSize.y / camTexture.texelSize.x, scale.y, scale.z);
    }
}