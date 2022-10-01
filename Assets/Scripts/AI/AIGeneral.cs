using UnityEngine;

public class AIGeneral : General
{
    private void Awake()
    {
        FetchSoldiersAndRegister();
    }

    private void Start()
    {
        SelectNextPlayer();
    }
}
