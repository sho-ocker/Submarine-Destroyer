using UnityEngine;

public abstract class Mine : MonoBehaviour
{
    protected int damage;

    [SerializeField] protected bool isSubmarine = true;
    [SerializeField] private ExplosionEffect explosionEffect;

    public abstract MineType GetMineType();

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnTriggerActionPlayer();
            ApplyDamageToPlayer();
            TriggerExplosion();
        }
        else if (other.gameObject.CompareTag("DepthCharge"))
        {
            OnTriggerActionDepthCharge();
            TriggerExplosion();
        } else if (isSubmarine && other.gameObject.CompareTag("SeaLine") || !isSubmarine && other.gameObject.CompareTag("Wall"))
        {
            HandleExplosionEnd();
        }
        else if (!isSubmarine && other.gameObject.CompareTag("Submarine"))
        {
            TriggerExplosion();
            other.gameObject.GetComponent<SubmarineController>().TakeDamage(damage);
        }
    }

    protected virtual void OnTriggerActionPlayer() { }

    protected virtual void OnTriggerActionDepthCharge() { }

    protected void ApplyDamageToPlayer()
    {
        ShipController.Instance.TakeDamage(damage);
    }

    public void HandleExplosionEnd()
    {
        PoolManager.Instance.ReleaseMine(GetMineType(), gameObject);
    }

    public void TriggerExplosion()
    {
        explosionEffect.Explode();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }

    public void SetShooter(bool isSubmarine)
    {
        this.isSubmarine = isSubmarine;
    }
}

