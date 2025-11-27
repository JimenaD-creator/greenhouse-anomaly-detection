using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GreenhouseInitializer : MonoBehaviour
{
    [Header("Presets")]
    public GameObject healthyPlantPrefab;
    public GameObject infectedPlantPrefab;

    [Header("Settings")]
    [Range(0, 100)]
    public int infectedCount = 10;

    [Header("Coordinates")]
    public List<Vector3> coordinates = new List<Vector3>(100);

    [Header("MavMeshes")]
    public NavMeshSurface navMeshBot;

    private void Reset()
    {
        coordinates.Clear();
        for (int i = 0; i < 100; i++)
        {
            coordinates.Add(new Vector3(i % 10, 0, i / 10));
        }
    }

    private void Start()
    {
        InitializeGreenhouse();
        navMeshBot.BuildNavMesh();
    }

    public void InitializeGreenhouse()
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < coordinates.Count; i++)
            availableIndices.Add(i);

        HashSet<int> infectedIndices = new HashSet<int>();
        for (int i = 0; i < Mathf.Min(infectedCount, coordinates.Count); i++)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            infectedIndices.Add(availableIndices[randomIndex]);
            availableIndices.RemoveAt(randomIndex);
        }

        for (int i = 0; i < coordinates.Count; i++)
        {
            Vector3 pos = coordinates[i];
            GameObject prefabToSpawn = infectedIndices.Contains(i) ? infectedPlantPrefab : healthyPlantPrefab;
            Instantiate(prefabToSpawn, pos, Quaternion.identity, transform);
        }
    }
    public List<Vector3> GetCoordinates()
    {
        return coordinates;
    }
}
