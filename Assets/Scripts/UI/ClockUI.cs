using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour 
{
    private Transform clockMinuteTransform;
    private Text timeText;

    private void Awake() 
    {
        clockMinuteTransform = transform.Find("minuteHand");
        timeText = transform.Find("timeText").GetComponent<Text>();
    }

    private void Update() 
    {
        const float degreePerSecond = -360.0f / 10.0f; // One turn in 10s, backward

        clockMinuteTransform.eulerAngles = new Vector3(0, 0, -degreePerSecond * GameManager.Instance.GetTimer());
        timeText.text = string.Format("{0:0.00}", GameManager.Instance.GetTimer());
    }
}
