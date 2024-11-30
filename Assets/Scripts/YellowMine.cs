public class YellowMine : Mine
{
    private void Start()
    {
        damage = 0;
    }

    protected override void OnTriggerActionPlayer()
    {
        base.OnTriggerActionPlayer();
        RefillDepthCharges();
    }

    private void RefillDepthCharges()
    {
        ShipController.Instance.RefillDepthCharges();
    }

    public override MineType GetMineType()
    {
        return MineType.YellowMine;
    }
}
