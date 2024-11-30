using System.Collections;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    private int laneIndex;
    private LaneManager laneManager => LaneManager.Instance;

    [SerializeField] private Health health;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minShootInterval = 1f;
    [SerializeField] private float maxShootInterval = 5f;
    [SerializeField] private ExplosionEffect explosionEffect;
    [SerializeField] private MineShooter mineShooter;

    private bool movingRight;
    private bool isAlive = true;

    public void Initialize(int laneIndex)
    {
        this.laneIndex = laneIndex;

        movingRight = transform.right.x > 0;
    }

    private void Update()
    {
        MoveSubmarine();
    }

    private void Start()
    {
        StartCoroutine(ShootMinesAtRandomIntervals());
    }

    private IEnumerator ShootMinesAtRandomIntervals()
    {
        while (true)
        {
            float interval = Random.Range(minShootInterval, maxShootInterval);
            yield return new WaitForSeconds(interval);
            if (isAlive) mineShooter.ShootMine();
        }
    }

    private void MoveSubmarine()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.right);
    }

    public void TakeDamage(int points)
    {
        health.TakeDamage(points);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Flip();
        }
    }

    public void HandleExplosionEnd()
    {
        laneManager.FreeLane(laneIndex);
        Destroy(gameObject);
        laneManager.SpawnSubmarines(1);
    }

    public void OnDeath()
    {
        speed = 0;
        GetComponent<BoxCollider2D>().enabled = false;
        isAlive = false;
        explosionEffect.Explode();
    }

    public void Flip()
    {
        transform.right = -transform.right;
    }

    internal void ForceLaunchMine()
    {
        mineShooter.ShootMine();
    }
}
