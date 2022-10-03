using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Campaign : MonoBehaviour
{
    public List<Image> campImages;
    public List<TMP_Text> campTexts;

    public TMP_Text title;
    public TMP_Text desc;
    public Image map;

    private void Start()
    {
        int maxReached = 0;

        for (int i = 0; i < campImages.Count; ++i)
        {
            Button b = campImages[i].GetComponent<Button>();

            bool unlocked = GameApplication.Instance.HasUnlockedGameScene(i);

            b.interactable = unlocked;

            if (unlocked)
            {
                if (i == 0) b.onClick.AddListener(() => Select(0));
                if (i == 1) b.onClick.AddListener(() => Select(1));
                if (i == 2) b.onClick.AddListener(() => Select(2));

                maxReached = i;
            }

            campImages[i].sprite = GameApplication.Instance.GetGameScene(i).icon;
            campTexts[i].text = GameApplication.Instance.GetGameScene(i).name;
        }

        Select(maxReached);
    }

    private void Update()
    {
        if (Inputs.Instance.nextLeft)
        {
            GameApplication.Instance.UnlockedGameScene(0);
            GameApplication.Instance.UnlockedGameScene(1);
            GameApplication.Instance.UnlockedGameScene(2);
            Inputs.Instance.nextLeft = false;
        }
    }

    private void Select(int i)
    {
        title.text = GameApplication.Instance.GetGameScene(i).name;
        desc.text = GameApplication.Instance.GetGameScene(i).text;
        map.sprite = GameApplication.Instance.GetGameScene(i).map;

        GameApplication.Instance.SetCurrentScene(i);
    }

    public void Play()
    {
        GameApplication.Instance.GoToCurrentScene();
    }
}
