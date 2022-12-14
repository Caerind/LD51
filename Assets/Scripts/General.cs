using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class General : MonoBehaviour
{
    protected List<Soldier> soldiers;
    protected Soldier selectedSoldier;
    protected Soldier nextSelectedSoldier;

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

        ChooseNextSoldierRandomly();
    }

    public virtual bool IsPlayerGeneral()
    {
        return false;
    }

    public int GetNextSelectedIndex()
    {
        if (soldiers == null)
        {
            return 777;
        }
        if (soldiers.Count == 0)
        {
            return 888;
        }
        if (nextSelectedSoldier == null)
        {
            return 999;
        }
        return soldiers.IndexOf(nextSelectedSoldier);
    }

    public List<Soldier> GetSoldiers()
    {
        return soldiers;
    }

    public int GetSelectableSoldiersCount()
    {
        int count = 0;
        if (soldiers != null)
        {
            foreach (var s in soldiers)
            {
                if (s != null && s.isSelectable)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public Soldier GetSelectedSoldier()
    {
        return selectedSoldier;
    }

    public Soldier GetNextSelectedSoldier()
    {
        return nextSelectedSoldier;
    }

    public void SelectNextPlayer()
    {
        for (int i = 0; i < soldiers.Count; ++i)
        {
            Soldier soldier = soldiers[i];
            if (soldier == nextSelectedSoldier)
            {
                selectedSoldier = soldier;
                selectedSoldier.SetMainSoldier(true);
                nextSelectedSoldier = null;
            }
            else
            {
                if (nextSelectedSoldier == null && soldier.isSelectable)
                {
                    nextSelectedSoldier = soldier;
                }
                soldier.SetMainSoldier(false);
            }
        }
        if (nextSelectedSoldier == null && soldiers.Count > 0)
        {
            nextSelectedSoldier = soldiers[0];
        }
    }

    public List<Soldier> GetAvailableSoldiersForSelection()
    {
        List<Soldier> result = new List<Soldier>(Mathf.Max(soldiers.Count - 1, 1));
        for (int i = 0; i < soldiers.Count; ++i)
        {
            Soldier soldier = soldiers[i];
            if (soldier != selectedSoldier && soldier.isSelectable)
            {
                result.Add(soldier);
            }
        }
        if (result.Count == 0 && soldiers.Count > 0)
        {
            result.Add(soldiers[0]);
        }
        return result;
    }

    public void RemoveRefToSoldier(Soldier soldier)
    {
        if (nextSelectedSoldier == soldier)
        {
            int indexOf = soldiers.IndexOf(soldier);
            indexOf = (indexOf + 1) % soldiers.Count;
            nextSelectedSoldier = soldiers[indexOf];
        }
        soldiers.Remove(soldier);
    }

    protected void ChooseNextSoldierRandomly()
    {
        List<Soldier> availables = GetAvailableSoldiersForSelection().Randomize().ToList();
        if (availables.Count > 0)
        {
            nextSelectedSoldier = availables[0];
        }
    }
}
