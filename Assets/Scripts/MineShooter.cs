using UnityEngine;

public class MineShooter : MonoBehaviour
{
    [SerializeField] private MineType[] possibleMineTypes;

    [SerializeField] private Transform mineSpawnPoint;

    [SerializeField] private bool isSubmarine;

    // Function to return a random mine type from the list
    private MineType GetRandomMineType()
    {
        if (possibleMineTypes.Length > 0)
        {
            // Randomly select a mine type from the possible mine types list
            return possibleMineTypes[Random.Range(0, possibleMineTypes.Length)];
        }
        else
        {
            Debug.LogWarning("No possible mine types defined.");
            return MineType.BlackMine; // Default to BlackMine if no types are defined
        }
    }

    public void ShootMine()
    {
        MineType randMineType = GetRandomMineType();
        GameObject mine = PoolManager.Instance.GetMine(randMineType);

        if (mine != null && mineSpawnPoint != null)
        {
            mine.transform.SetPositionAndRotation(mineSpawnPoint.position, Quaternion.identity);
            mine.GetComponent<Mine>().SetShooter(isSubmarine);
            mine.layer = isSubmarine ? LayerMask.NameToLayer("MineSubmarine") : LayerMask.NameToLayer("MinePlane");
            Rigidbody2D rb = mine.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = isSubmarine ? -0.05f : 0.05f;
            }

            mine.SetActive(true);
        }
    }
}
