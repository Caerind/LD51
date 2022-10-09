using UnityEngine;
using UnityEngine.UI;

public class BoostSceneFader : MonoBehaviour
{
    [SerializeField] private float introFader = 1.0f;
    [SerializeField] private float wait = 1.0f;
    [SerializeField] private float outroFader = 1.0f;
    [SerializeField] private SceneReference outScene;

    private Image image;
    private float timer = 0.0f;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > introFader + wait + outroFader)
        {
            outScene.LoadScene();
        }
        else if (timer > introFader + wait)
        {
            image.color = Color.Lerp(Color.white, Color.black, (timer - introFader - wait) / outroFader);
        }
        else if (timer > introFader)
        {
            image.color = Color.white;
        }
        else
        {
            image.color = Color.Lerp(Color.black, Color.white, timer / introFader);
        }
    }
}
