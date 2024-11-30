using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum MineType
{
    BlackMine,
    RedMine,
    PurpleMine,
    BlueMine,
    SpecialMine,
    YellowMine
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [Header("Mines Pool")]
    [SerializeField] private List<GameObject> minePrefabs;
    [SerializeField] private int initialMinePoolSize = 10;
    [SerializeField] private int maxMinePoolSize = 15;
    private Dictionary<MineType, ObjectPool<GameObject>> minePools;

    [Header("Depth Charges Pool")]
    [SerializeField] private GameObject depthChargePrefab;
    [SerializeField] private int initialDepthChargePoolSize = 10;
    [SerializeField] private int maxDepthChargePoolSize = 20;
    private ObjectPool<GameObject> depthChargePool;

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
    }

    private void Start()
    {
        InitializeDepthChargePool();
        InitializeMinePools();
    }

    private void InitializeDepthChargePool()
    {
        depthChargePool = new ObjectPool<GameObject>(
                    CreateDepthCharge,
                    OnGetDepthCharge,
                    OnReleaseDepthCharge,
                    OnDestroyDepthCharge,
                    false,
                    initialDepthChargePoolSize,
                    maxDepthChargePoolSize
                );

        for (int i = 0; i < initialDepthChargePoolSize; i++)
        {
            depthChargePool.Release(CreateDepthCharge());
        }
    }

    private void InitializeMinePools()
    {
        minePools = new Dictionary<MineType, ObjectPool<GameObject>>();
        foreach (MineType mineType in System.Enum.GetValues(typeof(MineType)))
        {
            GameObject minePrefab = minePrefabs[(int)mineType];

            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                () => CreateMine(minePrefab),
                OnGetMine,
                OnReleaseMine,
                OnDestroyMine,
                false,
                initialMinePoolSize,
                maxMinePoolSize
            );

            //Every Mine pool will have 10 inital mines but the YellowMine and SpecialMine could only have 1 
            //because there can only be 1 on the screen at a time (unless the plane interval is shortened manually)
            for (int i = 0; i < initialMinePoolSize; i++)
            {
                pool.Release(CreateMine(minePrefab));
            }

            minePools.Add(mineType, pool);
        }
    }

    private GameObject CreateMine(GameObject prefab)
    {
        return Instantiate(prefab, parent: transform);
    }

    private void OnGetMine(GameObject mine)
    {
        mine.SetActive(true);
    }

    private void OnReleaseMine(GameObject mine)
    {
        mine.SetActive(false);
    }

    private void OnDestroyMine(GameObject mine)
    {
        Destroy(mine);
    }

    public GameObject GetMine(MineType mineType)
    {
        if (minePools.TryGetValue(mineType, out ObjectPool<GameObject> pool))
            return pool.Get();
        return null;
    }

    public void ReleaseMine(MineType mineType, GameObject mine)
    {
        if (minePools.TryGetValue(mineType, out ObjectPool<GameObject> pool))
            pool.Release(mine);
    }

    private GameObject CreateDepthCharge()
    {
        return Instantiate(depthChargePrefab, parent: transform);
    }

    private void OnGetDepthCharge(GameObject depthCharge)
    {
        depthCharge.SetActive(true);
    }

    private void OnReleaseDepthCharge(GameObject depthCharge)
    {
        depthCharge.SetActive(false);
        depthCharge.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
    }

    private void OnDestroyDepthCharge(GameObject depthCharge)
    {
        Destroy(depthCharge);
    }

    public GameObject GetDepthCharge()
    {
        return depthChargePool.Get();
    }

    public void ReleaseDepthCharge(GameObject depthCharge)
    {
        depthChargePool.Release(depthCharge);
    }
}
