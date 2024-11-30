public class RedMine : Mine
{
    private void Start()
    {
        damage = 10;
    }

    protected override void OnTriggerActionPlayer()
    {
        base.OnTriggerActionPlayer();
        TogglePlayerSpeed();
    }

    private void TogglePlayerSpeed()
    {
        ShipController.Instance.ToggleSpeed();
    }

    public override MineType GetMineType()
    {
        return MineType.RedMine;
    }
}
