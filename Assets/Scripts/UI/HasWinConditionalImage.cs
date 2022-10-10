using UnityEngine;
using UnityEngine.UI;

public class HasWinConditionalImage : MonoBehaviour
{
    [SerializeField] private Sprite WinImage;
    [SerializeField] private Sprite LooseImage;

    private void Start()
    {
        Image image = GetComponent<Image>();
        if (GameApplication.Instance.GetHasPlayerWin())
        {
            image.sprite = WinImage;
        }
        else
        {
            image.sprite = LooseImage;
        }
    }
}
