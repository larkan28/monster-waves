using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerActor : Actor
{
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAbility playerAbility;
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerHurt playerHurt;

    public override void OnSpawn()
    {
        playerInteraction.Init(this);
        playerController.Init(this);
        playerAbility.Init(this);
        playerStats.Init(this);
        playerLevel.Init(this);
        playerHurt.Init(this);

        StopAllCoroutines();
        StartCoroutine(CR_AssignCamera());
    }

    private void Update()
    {
        if (!m_health.IsAlive)
            return;

        playerController.Think();
        playerAbility.Think();
        playerHurt.Think();

        // Testing
        playerLevel.Think();
    }

    private void FixedUpdate()
    {
        if (!m_health.IsAlive)
            return;

        playerInteraction.ThinkFixed();
        playerController.ThinkFixed();
    }

    private IEnumerator CR_AssignCamera()
    {
        yield return new WaitForEndOfFrame();

        if (Camera.main.TryGetComponent<CinemachineBrain>(out var cb))
            cb.ActiveVirtualCamera.Follow = transform;

        playerLevel.LevelUp();
    }
}
