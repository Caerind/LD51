using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class MinimapCameraLightController : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private float minimapIntensity = 0.7f;

    private float previousIntensity;

    private void Start()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera == gameObject.GetComponent<Camera>())
        {
            previousIntensity = globalLight.intensity;
            globalLight.intensity = minimapIntensity;
        }
    }

    private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera == gameObject.GetComponent<Camera>())
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