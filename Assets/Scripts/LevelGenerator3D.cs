using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator3D : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public GameObject[] modulePrefabs;

    [Header("Parámetros Modulares (Inspector)")]
    public int initialModulesCount = 8;
    public float verticalStep = 3.5f;
    public float horizontalSpreadX = 2.5f;
    public float forwardStepZ = 4.0f;
    public float recycleDistanceBehind = 12f;

    [Header("Curva de Dificultad Progresiva")]
    public float minScaleMultiplier = 0.55f;
    public float scaleReductionPerStep = 0.015f;
    public float extraGapPerStep = 0.04f;

    private readonly Queue<GameObject> activeModules = new Queue<GameObject>();
    private Vector3 nextSpawnPos;
    private int spawnedCount;

    void Start()
{
    if (player != null)
    {
        nextSpawnPos = new Vector3(player.position.x, player.position.y - 2.2f, player.position.z);
    }
    else
    {
        nextSpawnPos = new Vector3(0f, -2.2f, 0f);
    }

    for (int i = 0; i < initialModulesCount; i++)
    {
        SpawnModule(i == 0);
    }
}

    void Update()
    {
        if (player == null || activeModules.Count == 0) return;

        // Genera un nuevo módulo cuando el jugador se acerca al último generado
        if (player.position.z + (initialModulesCount * forwardStepZ * 0.6f) >= nextSpawnPos.z ||
            player.position.y - (initialModulesCount * verticalStep * 0.6f) <= nextSpawnPos.y)
        {
            SpawnModule(false);
        }

        // Recicla módulos que quedaron muy atrás
        GameObject oldest = activeModules.Peek();
        if (oldest != null && (player.position.z - oldest.transform.position.z > recycleDistanceBehind ||
            oldest.transform.position.y - player.position.y > recycleDistanceBehind))
        {
            activeModules.Dequeue();
            Destroy(oldest);
        }
    }

    private void SpawnModule(bool isFirstModule)
    {
        if (modulePrefabs == null || modulePrefabs.Length == 0) return;

        GameObject prefab = modulePrefabs[Random.Range(0, modulePrefabs.Length)];
        GameObject instance = Instantiate(prefab, nextSpawnPos, Quaternion.identity);

        float difficultyFactor = spawnedCount;
        float scaleMult = isFirstModule
            ? 1.2f
            : Mathf.Max(minScaleMultiplier, 1f - (difficultyFactor * scaleReductionPerStep));

        instance.transform.localScale = new Vector3(
            prefab.transform.localScale.x * scaleMult,
            prefab.transform.localScale.y,
            prefab.transform.localScale.z * scaleMult
        );

        activeModules.Enqueue(instance);
        spawnedCount++;

        float currentVerticalGap = verticalStep + Mathf.Min(difficultyFactor * extraGapPerStep, 2.5f);
        float randomX = isFirstModule ? 0f : Random.Range(-horizontalSpreadX, horizontalSpreadX);

        nextSpawnPos = new Vector3(
            randomX,
            nextSpawnPos.y - currentVerticalGap,
            nextSpawnPos.z + forwardStepZ
        );
    }
}