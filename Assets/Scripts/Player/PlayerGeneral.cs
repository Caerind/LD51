using System.Collections.Generic;
using UnityEngine;

public class PlayerGeneral : General
{
    private void Awake()
    {
        FetchSoldiersAndRegister();
    }

    private void Update()
    {
        Inputs inputs = Inputs.Instance;
        if (inputs != null && soldiers.Count > 0)
        {
            if (inputs.nextLeft || inputs.nextRight)
            {
                List<Soldier> availables = GetAvailableSoldiersForSelection();
                if (availables.Count > 1)
                {
                    if (inputs.nextLeft)
                    {
                        inputs.nextLeft = false;
                        int indexOf = availables.IndexOf(nextSelectedSoldier);
                        indexOf = (indexOf - 1 + availables.Count) % availables.Count;
                        nextSelectedSoldier = availables[indexOf];
                    }
                    if (inputs.nextRight)
                    {
                        inputs.nextRight = false;
                        int indexOf = availables.IndexOf(nextSelectedSoldier);
                        indexOf = (indexOf + 1) % availables.Count;
                        nextSelectedSoldier = availables[indexOf];
                    }
                }
                else
                {
                    inputs.nextLeft = false;
                    inputs.nextRight= false;
                }
            }
        }
    }

    public override bool IsPlayerGeneral()
    {
        return true;
    }
}
