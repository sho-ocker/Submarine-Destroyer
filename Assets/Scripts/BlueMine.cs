using UnityEngine;

public class BlueMine : Mine
{
    [SerializeField] private float disableDuration = 6f;

    private void Start()
    {
        damage = 0;
    }

    protected override void OnTriggerActionPlayer()
    {
        base.OnTriggerActionPlayer();
        DisablePlayerShooting();
    }

    private void DisablePlayerShooting()
    {
        ShipController.Instance.DisableShooting(disableDuration);
    }

    public override MineType GetMineType()
    {
        return MineType.BlueMine;
    }
}
