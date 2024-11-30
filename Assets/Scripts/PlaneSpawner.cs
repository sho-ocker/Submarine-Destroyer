using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    [SerializeField] private GameObject planePrefab;

    private float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnPlane();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnPlane()
    {
        Transform chosenSpawnPoint = Random.value > 0.5f ? leftSpawnPoint : rightSpawnPoint;
        Vector3 spawnPosition = chosenSpawnPoint.position;

        GameObject plane = Instantiate(planePrefab, spawnPosition, Quaternion.identity);
        PlaneController planeController = plane.GetComponent<PlaneController>();
        planeController.isFlying = true;
        planeController.direction = chosenSpawnPoint == leftSpawnPoint ? Vector3.right : Vector3.left;

        if (chosenSpawnPoint == leftSpawnPoint)
        {
            plane.transform.right = -plane.transform.right;
        }
    }
}
