using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [Header("What to spawn")] [Tooltip("Put 1+ enemy prefabs here. If multiple, a random one is chosen each spawn.")]
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [Header("Where to spawn")][Tooltip("Potential spawn locations. The spawner picks a random one each wave.")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Wave settings")]
    [Min(1)] public int maxEnemiesPerWave = 5;
    [Min(1)] public int maxWaves = 5;
    [SerializeField, Min(0f), Tooltip("Delay between enemies within a wave (seconds).")]
    private float perEnemyDelay = 0.25f;
    [SerializeField, Min(0f), Tooltip("Delay between waves (seconds).")]
    private float waveInterval = 3f;

    [Header("Behavior")] [Tooltip("Automatically start spawning on Start().")]
    public bool autoStart = true;
    
    [Tooltip("Disable this spawner after it finishes (saves CPU in big scenes).")]
    public bool disableOnComplete = true;

    [Header("Optional: Lightweight Pool")]
    public bool usePooling = true;
    [Min(0)]
    public int poolSize = 25;

    // --- internal state ---
    Coroutine _runner;
    readonly Queue<GameObject> _pool = new Queue<GameObject>();
    bool _poolInitialized;

    void Start()
    {
        if (autoStart) StartSpawning();
    }

    void OnDisable()
    {
        if (_runner != null) StopCoroutine(_runner);
    }

    // Public controls
    public void StartSpawning()
    {
        if (_runner != null) return;
        if (!ValidateSetup()) return;

        if (usePooling && !_poolInitialized)
        {
            WarmPool();
            _poolInitialized = true;
        }

        _runner = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (_runner == null) return;
        StopCoroutine(_runner);
        _runner = null;
    }

    // Core loop
    IEnumerator SpawnLoop()
    {
        for (int wave = 0; wave < maxWaves; wave++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Vector3 origin = point ? point.position : transform.position;

            for (int i = 0; i < maxEnemiesPerWave; i++)
            {
                SpawnOne(origin, point ? point.rotation : Quaternion.identity);
                if (perEnemyDelay > 0f)
                    yield return new WaitForSeconds(perEnemyDelay);
                else
                    yield return null; // next frame
            }

            if (wave < maxWaves - 1 && waveInterval > 0f)
                yield return new WaitForSeconds(waveInterval);
        }

        _runner = null;
        if (disableOnComplete) enabled = false;
    }

    void SpawnOne(Vector3 pos, Quaternion rot)
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        if (!prefab) return;

        GameObject go;
        if (usePooling && _pool.Count > 0)
        {
            go = _pool.Dequeue();
            if (go == null)
            {
                // If something destroyed pooled objects, instantiate a new one.
                go = Instantiate(prefab, pos, rot);
            }
            else
            {
                go.transform.SetPositionAndRotation(pos, rot);
                go.SetActive(true);
            }
        }
        else
        {
            go = Instantiate(prefab, pos, rot);
        }

        // Optional: if your enemy has a death/disable event, return it to pool.
        // Example convention: an Enemy component calls ReturnToPool() when it dies.
        var returnHook = go.GetComponent<IPoolReturn>();
        if (usePooling && returnHook != null)
            returnHook.SetupReturn(this);
    }

    // Called by enemies to re-enqueue themselves
    public void ReturnToPool(GameObject obj)
    {
        if (!usePooling || obj == null) return;
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }

    // Pool warmup
    void WarmPool()
    {
        if (poolSize <= 0 || enemyPrefabs.Count == 0) return;
        // Use the first prefab for pool warmup (cheap + simple)
        var prefab = enemyPrefabs[0];
        for (int i = 0; i < poolSize; i++)
        {
            var go = Instantiate(prefab, transform.position, Quaternion.identity);
            go.SetActive(false);
            _pool.Enqueue(go);
        }
    }

    bool ValidateSetup()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.LogWarning($"{name}: No enemyPrefabs assigned.");
            return false;
        }
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning($"{name}: No spawnPoints assigned; will use spawner's transform.");
            // Allow running; will spawn at spawner origin.
        }
        return true;
    }

    // Editor helper: draw spawn points
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (spawnPoints != null)
        {
            foreach (var t in spawnPoints)
            {
                if (!t) continue;
                Gizmos.DrawWireSphere(t.position, 0.5f);
                Gizmos.DrawLine(transform.position, t.position);
            }
        }
    }
}

/// <summary>
/// Optional interface your enemy script can implement to return to the spawner's pool.
/// Call pool.ReturnToPool(gameObject) when the enemy dies/despawns.
/// </summary>
public interface IPoolReturn
{
    void SetupReturn(EnemySpawnController spawner);
}
