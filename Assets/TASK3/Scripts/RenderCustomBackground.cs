using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class RenderCustomBackground : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private CameraEvent cameraEvent = CameraEvent.BeforeForwardOpaque;
    [SerializeField] private Texture texture;
    [SerializeField] private Material mat;

    private void Start()
    {
        var buffer = new CommandBuffer();
        buffer.Blit(texture, BuiltinRenderTextureType.CameraTarget, mat);
        cam.AddCommandBuffer(cameraEvent, buffer);
    }

    private void Reset()
    {
        cam = GetComponent<Camera>();
    }
}
