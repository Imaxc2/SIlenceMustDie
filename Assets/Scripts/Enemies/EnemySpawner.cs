using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public class EnemyChance
{
    public GameObject Enemy;
    public float Weight;
    public int StartSpawnWave;
}

public class EnemySpawner : MonoBehaviour
{
    public float SpawnRadius = 10f;
    public float SpawnRate = 2f;
    private float updatedSpawnRate;
    public GameObject EnemyPrefab;
    public List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();
    private float _spawnTime;
    public int Wave = 1;
    public float WaveRate = 10f;
    private float _waveTime;
    public TextMeshProUGUI waveText;

    public event Action<int> OnWaveChanged;

    [SerializeField] public List<EnemyChance> PossibleEnemies = new List<EnemyChance>();

    private void Start()
    {
        updatedSpawnRate = SpawnRate / 1 + (Wave / 15);
        Wave = Mathf.Max(1, GameManager.Instance.Data.currentWave - ((GameManager.Instance.Data.currentWave - 1) % 10));
        waveText.text = $"Wave {Wave}";
        _waveTime = WaveRate;
        _spawnTime = SpawnRate;
    }

    private void Update()
    {
        if (EnemyLimitUI.GamePaused) return;
        if (_waveTime <= 0f)
        {
            Wave++;
            _waveTime = WaveRate;
            waveText.text = $"Wave {Wave}";
            updatedSpawnRate = SpawnRate / (Wave / 2);
        }
        if (_spawnTime <= 0f)
        {
            _spawnTime = updatedSpawnRate;
            Vector2 SpawnPosition = UnityEngine.Random.insideUnitCircle.normalized * SpawnRadius;
            GameObject enemy = Instantiate(GetRandomEnemy(), SpawnPosition, Quaternion.identity);

            EnemyBehaviour move = enemy.GetComponent<EnemyBehaviour>();

            move.HP += Wave * 5;
            move.HP *= ((int)Wave / 2) + 1;
            move.spawner = this;
            Vector2 moveDirection = -SpawnPosition;
            moveDirection.x += UnityEngine.Random.Range(-3, 3);
            moveDirection.y += UnityEngine.Random.Range(-3, 3);
            move.MoveDirection = moveDirection.normalized;
            enemies.Add(enemy);
        }
        _spawnTime -= Time.deltaTime;
        _waveTime -= Time.deltaTime;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
        OnWaveChanged?.Invoke(activeEnemies.Count);
    }

    public void RegisterActiveEnemy(GameObject enemy)
    {
        if (!activeEnemies.Contains(enemy))
            activeEnemies.Add(enemy);
        OnWaveChanged?.Invoke(activeEnemies.Count);
    }

    public int GetActiveEnemyCount() => activeEnemies.Count;

    public GameObject GetRandomEnemy()
    {
        float totalWeight = 0f;
        foreach (var e in PossibleEnemies.Where(e => e.StartSpawnWave <= Wave))
            totalWeight += e.Weight;

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        foreach (var e in PossibleEnemies.Where(e => e.StartSpawnWave <= Wave))
        {
            randomValue -= e.Weight;
            if (randomValue <= 0f)
                return e.Enemy;
        }

        if (PossibleEnemies.Count > 0)
            return PossibleEnemies[0].Enemy;

        return null;
    }
}
