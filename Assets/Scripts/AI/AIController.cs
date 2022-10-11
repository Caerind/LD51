using UnityEngine;

public class AIController : Soldier
{
    [SerializeField] private float fireCooldownBonusAI = 0.5f;
    [SerializeField] private float cacCooldownBonusAI = 0.5f;

    [SerializeField] private AudioSource Deplacement;

    private FakeAgent agent;
    private AIBrain brain;

    private void Awake()
    {
        AwakeSoldier(isPlayer: false);
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDeath += HealthSystem_OnDied;

        brain = GetComponent<AIBrain>();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        OnSoldierDamaged(sender);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        brain.OnDied();

        OnSoldierDied(deadBodyPrefab: PrefabManager.Instance.GetDeadBodyAIPrefab());
    }

    private void Update()
    {
        UpdateSoldier();

        if (IsMainSoldier())
        {
            agent.SetSpeed(GetSpeed());
        }
        else
        {
            UpdateReactions();
        }
    }

    public override void SetMainSoldier(bool mainSoldier)
    {
        base.SetMainSoldier(mainSoldier);

        agent.SetStopped(!mainSoldier);
        if (!mainSoldier)
        {
            StopMoving();
        }
    }

    public void SetFakeAgent(FakeAgent agent)
    {
        this.agent = agent;
    }

    public override bool CanFire()
    {
        return timerAction >= (fireCooldown + fireCooldownBonusAI + (IsMainSoldier() ? 0.0f : fireCooldownBonusReaction));
    }

    public override bool CanCac()
    {
        return timerAction >= (cacCooldown + cacCooldownBonusAI + (IsMainSoldier() ? 0.0f : cacCooldownBonusReaction));
    }

    public void SetTargetPosition(Vector2 position)
    {
        agent.SetDestination(position);
    }

    public void StopMoving()
    {
        isMoving = false;
        animator?.SetFloat(animIDMvt, 0.0f);
        Deplacement.Stop();
    }

    private void LateUpdate()
    {
        Vector2 prevPos = transform.position.ToVector2();
        transform.position = agent.transform.position.ToVector2().ToVector3(); // Update pos (agent & physx)

        if (IsMainSoldier())
        {
            Vector2 mvt = transform.position.ToVector2() - prevPos;
            float mvtMagn = mvt.magnitude;
            animator?.SetFloat(animIDMvt, mvtMagn);
            bool wasMoving = isMoving;
            isMoving = mvtMagn > 0.025f;
            if (wasMoving != isMoving)
            {
                if (isMoving)
                {
                    Deplacement.Play();
                }
                else
                {
                    Deplacement.Stop();
                }
            }

            // TODO : Probably don't keep that
            if (mvt != Vector2.zero)
            {
                SetLookDir(mvt.normalized);
            }

            // Update agent dir for physx
            agent.SetAngle(transform.eulerAngles.z);
        }
    }
}
