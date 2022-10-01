using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIGeneral : General
{
    [SerializeField] private float timerRecomputeBest = 1.9f;
    [SerializeField] private Commander commander;

    private float timer;

    private void Awake()
    {
        FetchSoldiersAndRegister();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timerRecomputeBest)
        {
            timer = 0.0f;
            ChooseBestNext();
        }
    }

    private void ChooseBestNext()
    {
        // Random
        List<Soldier> availables = GetAvailableSoldiersForSelection().Randomize().ToList();
        if (availables.Count > 0)
        {
            nextSelectedSoldier = availables[0];
        }
    }
}
