using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnExplosionEnd;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private int numberOfExplosions;

    [SerializeField]
    private Vector2 explosionsOffset;

    [SerializeField]
    private float explosionsInterval = 0.1f;

    public void Explode()
    {
        StartCoroutine(ExplosionOverTime());
    }

    private IEnumerator ExplosionOverTime()
    {
        WaitForSeconds waitTime = new WaitForSeconds(explosionsInterval);
        for (int i = 0; i < numberOfExplosions; i++)
        {
            Instantiate(explosionPrefab,
                (Vector2)transform.position + Random.insideUnitCircle * explosionsOffset,
                Quaternion.identity);
            yield return waitTime;
        }
        OnExplosionEnd.Invoke();
    }
}
