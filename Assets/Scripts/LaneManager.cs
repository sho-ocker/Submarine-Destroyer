using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance { get; private set; }

    [SerializeField] private Transform[] lanes;
    [SerializeField] private GameObject submarinePrefab;
    [SerializeField] private float spawnOffset = 7f;

    private bool[] laneOccupied;
    List<int> availableLanes = new List<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        laneOccupied = new bool[lanes.Length];
    }

    private void Start()
    {
        SpawnSubmarines(3);
    }

    public void SpawnSubmarines(int numberOfSubmarines)
    {
        UpdateAvailableLanes();

        int submarinesToSpawn = Mathf.Min(availableLanes.Count, numberOfSubmarines);

        for (int i = 0; i < submarinesToSpawn; i++)
        {
            if (availableLanes.Count == 0)
            {
                break;
            }

            int laneIndex = availableLanes[Random.Range(0, availableLanes.Count)];
            StartCoroutine(SpawnSubmarine(laneIndex));
            availableLanes.Remove(laneIndex);
        }
    }

    private IEnumerator SpawnSubmarine(int laneIndex)
    {
        if (!laneOccupied[laneIndex] && availableLanes.Count > 1)
        {
            laneOccupied[laneIndex] = true;

            int flipChance = Random.Range(1, 3);
            GameObject submarine = Instantiate(submarinePrefab, lanes[laneIndex].position + Vector3.right * Random.Range(-spawnOffset, spawnOffset), Quaternion.identity);
            SubmarineController submarineScript = submarine.GetComponent<SubmarineController>();

            if (submarineScript != null)
            {
                submarineScript.Initialize(laneIndex);
            }

            if (flipChance == 1)
            {
                submarineScript.Flip();
            }
        }
        yield return null;
    }

    public void FreeLane(int laneIndex)
    {
        if (laneIndex >= 0 && laneIndex < laneOccupied.Length)
        {
            laneOccupied[laneIndex] = false;
        }
    }

    private void UpdateAvailableLanes()
    {
        availableLanes.Clear();
        for (int i = 0; i < lanes.Length; i++)
        {
            if (!laneOccupied[i])
            {
                availableLanes.Add(i);
            }
        }
    }
}
