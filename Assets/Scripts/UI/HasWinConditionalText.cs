using UnityEngine;
using TMPro;

public class HasWinConditionalText : MonoBehaviour
{
    [SerializeField] private string WinText;
    [SerializeField] private string LooseText;

    private void Start()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        if (GameApplication.Instance.GetHasPlayerWin())
        {
            text.text = WinText;
        }
        else
        {
            text.text = LooseText;
        }
    }
}
