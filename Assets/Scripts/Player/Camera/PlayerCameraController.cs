using UnityEngine;
using Cinemachine;

public class PlayerCameraController : Singleton<PlayerCameraController>
{
    [SerializeField] private float m_zoomSpeed = 5f;
    [SerializeField] private float m_zoomAmount = 2f;
    [SerializeField] private float m_minOrthographicSize = 5f;
    [SerializeField] private float m_maxOrthographicSize = 25f;
    [SerializeField] private bool m_invertZoom = true;

    private CinemachineVirtualCamera m_virtualCamera;
    private CinemachineCameraShake cinemachinecameraShake;
    private float m_orthographicSize;
    private float m_targetOrthographicSize;

    public void RegisterVCam(CinemachineVirtualCamera vCam)
    {
        m_virtualCamera = vCam;
        if (m_virtualCamera != null)
        {
            cinemachinecameraShake = m_virtualCamera.gameObject.GetComponent<CinemachineCameraShake>();
            m_orthographicSize = m_virtualCamera.m_Lens.OrthographicSize;
            m_targetOrthographicSize = m_orthographicSize;
        }
    }

    private void Update()
    {
        if (m_virtualCamera != null)
        {
            m_targetOrthographicSize += Input.mouseScrollDelta.y * m_zoomAmount * (m_invertZoom ? -1f : 1f);
            m_targetOrthographicSize = Mathf.Clamp(m_targetOrthographicSize, m_minOrthographicSize, m_maxOrthographicSize);

            m_orthographicSize = Mathf.Lerp(m_orthographicSize, m_targetOrthographicSize, Time.deltaTime * m_zoomSpeed);

            m_virtualCamera.m_Lens.OrthographicSize = m_orthographicSize;
        }
    }

    public void SetFollow(Transform transform)
    {
        if (m_virtualCamera != null)
        {
            m_virtualCamera.Follow = transform;
        }
    }

    public void Shake(float Amount, float time)
    {
        cinemachinecameraShake.Shake(Amount, time);

    }
}