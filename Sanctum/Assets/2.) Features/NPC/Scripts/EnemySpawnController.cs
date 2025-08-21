using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnController : MonoBehaviour
{
    //     [System.Serializable, ShowInInspector]
    // public class Wave
    // {
    //     public EnemyVariant[] enemyVariants;
    //     public float spawnInterval;
    // }
    //
    // [System.Serializable]
    // public class EnemyVariant
    // {
    //     public GameObject enemyPrefab;
    //     public int enemyCount;
    // }
    //
    // public Wave[] waves;
    // public float waveInterval = 5f;
    // public Vector3 spawnAreaCenter;
    // public Vector3 spawnAreaSize;
    // public bool spawnIn3D = true;
    //
    // private int currentWaveIndex = 0;
    // private int currentEnemyCount = 0;
    //
    // public UnityEvent OnAllWavesCompleted;
    //
    // private bool spawningEnabled = true;
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //     StartCoroutine(SpawnWaves());
    // }
    //
    // IEnumerator SpawnWaves()
    // {
    //     while (spawningEnabled && currentWaveIndex < waves.Length)
    //     {
    //         Wave currentWave = waves[currentWaveIndex];
    //
    //         foreach (EnemyVariant variant in currentWave.enemyVariants)
    //         {
    //             for (int i = 0; i < variant.enemyCount; i++)
    //             {
    //                 SpawnEnemy(variant.enemyPrefab);
    //
    //                 currentEnemyCount++;
    //                 yield return new WaitForSeconds(currentWave.spawnInterval);
    //             }
    //         }
    //
    //         while (currentEnemyCount > 0)
    //         {
    //             yield return null;
    //         }
    //
    //         currentWaveIndex++;
    //         yield return new WaitForSeconds(waveInterval);
    //     }
    //
    //     if (OnAllWavesCompleted != null)
    //     {
    //         OnAllWavesCompleted.Invoke();
    //     }
    // }
    //
    // void SpawnEnemy(GameObject enemyPrefab)
    // {
    //     Vector3 spawnPosition;
    //     if (spawnIn3D)
    //     {
    //         spawnPosition = GetRandomPosition3D();
    //     }
    //     else
    //     {
    //         spawnPosition = GetRandomPosition2D();
    //     }
    //
    //     Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    // }
    //
    // Vector3 GetRandomPosition3D()
    // {
    //     Vector3 randomPosition = spawnAreaCenter +
    //         new Vector3(
    //             Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
    //             Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
    //             Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
    //         );
    //
    //     return randomPosition;
    // }
    //
    // Vector3 GetRandomPosition2D()
    // {
    //     Vector2 randomPosition = spawnAreaCenter +
    //         new Vector2(
    //             Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
    //             Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f)
    //         );
    //
    //     return randomPosition;
    // }
}
