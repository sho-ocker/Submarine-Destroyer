using UnityEngine;

public class PurpleMine : Mine
{
    [SerializeField] private float explosionRadius = 5f;

    private void Start()
    {
        damage = 20;
    }

    protected override void OnTriggerActionDepthCharge()
    {
        base.OnTriggerActionDepthCharge();
        ExplodeNearbyMines();
    }

    private void ExplodeNearbyMines()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            Mine mine = collider.GetComponent<Mine>();
            if (mine != null && mine != this)
            {
                mine.TriggerExplosion();
            }
        }
    }

    public override MineType GetMineType()
    {
        return MineType.PurpleMine;
    }
}
