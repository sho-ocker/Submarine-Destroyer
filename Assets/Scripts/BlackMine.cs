public class BlackMine : Mine
{
    private void Start()
    {
        damage = 20;
    }

    protected override void OnTriggerActionPlayer()
    {
        base.OnTriggerActionPlayer();
    }

    public override MineType GetMineType()
    {
        return MineType.BlackMine;
    }
}
