using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class MinimapCameraLightController : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private float minimapIntensity = 0.7f;

    private Camera ownCamera;
    private float previousIntensity;

    private void Awake()
    {
        ownCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera == ownCamera)
        {
            previousIntensity = globalLight.intensity;
            globalLight.intensity = minimapIntensity;
        }
    }

    private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera == ownCamera)
        {
            globalLight.intensity = previousIntensity;
        }
    }

    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }
}