using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HasWinConditionalImage : MonoBehaviour
{
    [SerializeField] private Sprite WinText;
    [SerializeField] private Sprite LooseText;

    private void Start()
    {
        Image text = GetComponent<Image>();
        if (GameApplication.Instance.GetHasPlayerWin())
        {
            text.sprite = WinText;
        }
        else
        {
            text.sprite = LooseText;
        }
    }
}
