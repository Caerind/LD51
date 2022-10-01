using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour
{
    protected List<Soldier> soldiers;
    protected Soldier selectedSoldier;
    protected int nextSelectedIndex = 0;

    protected void FetchSoldiersAndRegister()
    {
        soldiers = new List<Soldier>(transform.childCount);
        for (int i = 0; i < transform.childCount; ++i)
        {
            Soldier soldier = transform.GetChild(i).GetComponent<Soldier>();
            if (soldier != null)
            {
                soldiers.Add(soldier);
            }
        }

        GameManager.Instance.RegisterGeneral(this);
    }

    public virtual bool IsPlayerGeneral()
    {
        return false;
    }

    public int GetNextSelectedIndex()
    {
        return nextSelectedIndex;
    }

    public List<Soldier> GetSoldiers()
    {
        return soldiers;
    }

    public Soldier GetSelectedPlayer()
    {
        return selectedSoldier;
    }

    public void SelectNextPlayer()
    {
        for (int i = 0; i < soldiers.Count; ++i)
        {
            if (i == nextSelectedIndex)
            {
                selectedSoldier = soldiers[i];
                selectedSoldier.SetMainSoldier(true);
            }
            else
            {
                soldiers[i].SetMainSoldier(false);
            }
        }
    }
}
