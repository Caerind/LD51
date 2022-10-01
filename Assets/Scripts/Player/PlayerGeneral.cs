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
            if (inputs.nextLeft)
            {
                inputs.nextLeft = false;
                nextSelectedIndex = (nextSelectedIndex - 1 + soldiers.Count) % soldiers.Count;
            }
            if (inputs.nextRight)
            {
                inputs.nextRight = false;
                nextSelectedIndex = (nextSelectedIndex + 1) % soldiers.Count;
            }
        }
    }

    public override bool IsPlayerGeneral()
    {
        return true;
    }
}
