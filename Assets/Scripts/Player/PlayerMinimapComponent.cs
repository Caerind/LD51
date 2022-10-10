using UnityEngine;

public class PlayerMinimapComponent : MonoBehaviour
{
    [SerializeField] private GameObject circleBlue;
    [SerializeField] private GameObject circleWhite;

    private Soldier soldier = null;

    private void Awake()
    {
        soldier = GetComponentInParent<Soldier>();
    }

    private void Update()
    {
        General general = soldier.GetGeneral();
        if (general != null)
        {
            if (this == general.GetNextSelectedSoldier())
            {
                circleWhite.SetActive(true);
                circleBlue.SetActive(true);
            }
            else if (this == general.GetSelectedSoldier())
            {
                circleWhite.SetActive(true);
                circleBlue.SetActive(false);
            }
            else
            {
                circleWhite.SetActive(false);
                circleBlue.SetActive(true);
            }
        }
    }
}
