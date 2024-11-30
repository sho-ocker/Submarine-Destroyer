using UnityEngine;

public interface ISpecialMineBehaviour
{
    void OnPlayerShipCollision();
    void OnSubmarineCollision(SubmarineController submarine);
}

public class SpecialMine : Mine, ISpecialMineBehaviour
{
    private void Start()
    {
        damage = 0;
    }

    protected override void OnTriggerActionPlayer()
    {
        base.OnTriggerActionPlayer();
        OnPlayerShipCollision();
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.gameObject.CompareTag("Submarine"))
        {
            SubmarineController submarine = other.gameObject.GetComponent<SubmarineController>();
            OnSubmarineCollision(submarine);
        }
    }

    public void OnPlayerShipCollision()
    {
        ShipController.Instance.ForceLaunchDepthCharge();
    }

    public void OnSubmarineCollision(SubmarineController submarine)
    {
        submarine.ForceLaunchMine();
        TriggerExplosion();
    }

    public override MineType GetMineType()
    {
        return MineType.SpecialMine;
    }
}
