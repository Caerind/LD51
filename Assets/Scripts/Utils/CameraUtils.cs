using UnityEngine;

public static class CameraUtils
{
#if UNITY_EDITOR
    public static Camera editorCamera => UnityEditor.SceneView.lastActiveSceneView.camera;
#endif // UNITY_EDITOR

    public static Bounds OrthographicBounds(Camera camera, float sizeZ = 2000f)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2f;
        return new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, sizeZ));
    }

    public static void DrawGizmos(Camera camera)
    {
        Matrix4x4 temp = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(camera.transform.position, camera.transform.rotation, Vector3.one);
        if (camera.orthographic)
        {
            float spread = camera.farClipPlane - camera.nearClipPlane;
            float center = (camera.farClipPlane + camera.nearClipPlane) * 0.5f;
            Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(camera.orthographicSize * 2 * camera.aspect, camera.orthographicSize * 2, spread));
        }
        else
        {
            Gizmos.DrawFrustum(new Vector3(0, 0, (camera.nearClipPlane)), camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
        }
        Gizmos.matrix = temp;
    }
}