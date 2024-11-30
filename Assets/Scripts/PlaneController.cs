using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [SerializeField] private MineShooter mineShooter;

    [SerializeField] private float speed = 5f;
    public bool isFlying = false;
    public Vector3 direction;

    private bool mineLaunched = false;

    private void Update()
    {
        if (isFlying)
        {
            FlyAcrossScreen();
            if (!mineLaunched)
            {
                DetectAndLaunchMine();
            }
        }
    }

    private void FlyAcrossScreen()
    {
        // Move the object in the specified direction
        transform.position += direction * speed * Time.deltaTime;

        // Calculate the horizontal screen bounds
        float screenBound = Camera.main.aspect * Camera.main.orthographicSize + 1f;

        // Destroy the object if it goes beyond the screen bounds
        if ((direction == Vector3.left && transform.position.x < -screenBound) ||
            (direction == Vector3.right && transform.position.x > screenBound))
        {
            Destroy(gameObject);
        }

    }

    private void DetectAndLaunchMine()
    {
        // Detect player ship below the airplane
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ship"));

        if (hit.collider != null)
        {
            mineShooter.ShootMine();
            mineLaunched = true;
        }
    }
}
