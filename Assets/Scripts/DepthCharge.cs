using UnityEngine;

public class DepthCharge : MonoBehaviour
{
    [SerializeField] private ExplosionEffect explosionEffect;
    [SerializeField] private int submarineDamage = 10;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Submarine"))
        {
            SubmarineController submarine = other.gameObject.GetComponent<SubmarineController>();
            if (submarine != null)
            {
                submarine.TakeDamage(submarineDamage);
            }
        }

        Explode();
    }


    private void Explode()
    {
        if (explosionEffect != null && gameObject.activeInHierarchy)
        {
            explosionEffect.Explode();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else
        {
            HandleExplosionEnd();
        }
    }

    public void HandleExplosionEnd()
    {
        PoolManager.Instance.ReleaseDepthCharge(gameObject);
    }
}
