using UnityEngine;

public class AIController : Soldier
{
    [SerializeField] private float fireCooldownBonusAI = 0.5f;
    [SerializeField] private float cacCooldownBonusAI = 0.5f;

    [SerializeField] private GameObject bloodDeath;
    [SerializeField] private GameObject bloodImpact;
    [SerializeField] private AudioSource Deplacement;

    [SerializeField] private GameObject redCircle;

    public bool isInPlayerZone = false;

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
        if (sender != null)
        {
            Soldier senderSoldier = (Soldier)sender;
            if (senderSoldier != null && senderSoldier.IsPlayerSoldier())
            {
                SetLookAt(senderSoldier.transform.position);
            }

            // Blood
            GameObject part = Instantiate(bloodImpact, transform.position, Quaternion.identity);
            Destroy(part, 2.0f);
        }
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        brain.UnlockPoint();

        GetGeneral().RemoveRefToSoldier(this);

        // Spawn new entity
        Transform pfDeadBody = Resources.Load<Transform>("pfDeadBodyAI");
        Instantiate(pfDeadBody, transform.position, Quaternion.identity).eulerAngles = new Vector3(0.0f, 0.0f, GetLookAngle());

        // Blood
        GameObject part = Instantiate(bloodDeath, transform.position, Quaternion.identity);
        Destroy(part, 2.0f);

        Destroy(gameObject);
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

        UpdateMinimap();
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
            bool wasmooving = isMoving;
            isMoving = mvtMagn > 0.025f;

            if (wasmooving != isMoving)
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

    private void UpdateMinimap()
    {
        if (isInPlayerZone)
        {
            redCircle.SetActive(true);
        }
        else
        {
            redCircle.SetActive(false);
        }
    }
}
