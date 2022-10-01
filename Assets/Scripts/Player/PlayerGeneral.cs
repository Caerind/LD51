using System.Collections.Generic;
using UnityEngine;

public class PlayerGeneral : General
{
    private void Awake()
    {
        FetchSoldiersAndRegister();
    }

    private void Start()
    {
        SelectNextPlayer();
    }

    private void Update()
    {
        Inputs inputs = Inputs.Instance;
        if (inputs != null && soldiers.Count > 0)
        {
            if (inputs.nextLeft || inputs.nextRight)
            {
                List<int> availables = GetAvailableIndexesForSelection();
                int nextIndexInAvailable = availables.IndexOf(nextSelectedIndex);
                if (nextIndexInAvailable < 0)
                {
                    nextIndexInAvailable = 0;
                }

                if (availables.Count > 1)
                {
                    if (inputs.nextLeft)
                    {
                        inputs.nextLeft = false;
                        nextIndexInAvailable = (nextIndexInAvailable - 1 + availables.Count) % availables.Count;
                        nextSelectedIndex = availables[nextIndexInAvailable];
                    }
                    if (inputs.nextRight)
                    {
                        inputs.nextRight = false;
                        nextIndexInAvailable = (nextIndexInAvailable + 1) % availables.Count;
                        nextSelectedIndex = availables[nextIndexInAvailable];
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
