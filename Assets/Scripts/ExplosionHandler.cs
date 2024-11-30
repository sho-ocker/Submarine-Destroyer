using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
