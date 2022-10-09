using UnityEngine;

public class AIGeneral : General
{
    [SerializeField] private float timerRecomputeNext = 1.9f;

    private float timer;

    private void Awake()
    {
        FetchSoldiersAndRegister();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timerRecomputeNext)
        {
            timer = 0.0f;
            ChooseNextSoldierRandomly();

            // TODO : Choose next soldier, not randomly
        }
    }
}
