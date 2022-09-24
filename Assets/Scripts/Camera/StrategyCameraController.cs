using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : Singleton<CameraController>
{
    [SerializeField] private float m_moveSpeed = 2f;
    [SerializeField] private float m_zoomSpeed = 5f;
    [SerializeField] private float m_zoomAmount = 2f;
    [SerializeField] private float m_minOrthographicSize = 5f;
    [SerializeField] private float m_maxOrthographicSize = 25f;
    [SerializeField] private bool m_invertZoom = true;

    private CinemachineVirtualCamera m_virtualCamera;
    private float m_orthographicSize;
    private float m_targetOrthographicSize;

    private void Awake()
    {
        m_virtualCamera = GetComponent<CinemachineVirtualCamera>();
        m_orthographicSize = m_virtualCamera.m_Lens.OrthographicSize;
        m_targetOrthographicSize = m_orthographicSize;
    }

    private void Update()
    {
        m_targetOrthographicSize += Input.mouseScrollDelta.y * m_zoomAmount * (m_invertZoom ? -1f : 1f);
        m_targetOrthographicSize = Mathf.Clamp(m_targetOrthographicSize, m_minOrthographicSize, m_maxOrthographicSize);

        m_orthographicSize = Mathf.Lerp(m_orthographicSize, m_targetOrthographicSize, Time.deltaTime * m_zoomSpeed);

        m_virtualCamera.m_Lens.OrthographicSize = m_orthographicSize;
    }

    private void LateUpdate()
    {
        Vector2 mvt = Vector2.zero;
        mvt.x = Input.GetAxis("Horizontal");
        mvt.y = Input.GetAxis("Vertical");
        transform.Translate(mvt * m_moveSpeed * m_targetOrthographicSize * Time.deltaTime);
    }
}