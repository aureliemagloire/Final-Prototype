using UnityEngine;

public class ArtifactSpawner2D : MonoBehaviour
{
    public GameObject[] artifactPrefabs;  // Drag Pink & Green here
    public Transform player;              // Your player object
    public float spawnRadius = 5f;        // How far from player to spawn
    public int totalArtifacts = 6;        // How many to spawn

    void Start()
{
    StartCoroutine(SpawnArtifactsGradually());
}

System.Collections.IEnumerator SpawnArtifactsGradually()
{
    for (int i = 0; i < totalArtifacts; i++)
    {
        Vector2 spawnPos = GetSpawnPosition();

        int prefabIndex = i % artifactPrefabs.Length;
        GameObject artifact = Instantiate(artifactPrefabs[prefabIndex], spawnPos, Quaternion.identity);
        artifact.SetActive(true);

        yield return new WaitForSeconds(5f); // delay between each spawn
    }
}


    Vector2 GetSpawnPosition()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector2 spawnPos = (Vector2)player.position + randomDir * spawnRadius;

        // behind the player
        Vector2 toSpawn = (spawnPos - (Vector2)player.position).normalized;
        Vector2 forward = player.right;
        float dot = Vector2.Dot(forward, toSpawn);

        if (dot > 0.3f)
            spawnPos = (Vector2)player.position - toSpawn * spawnRadius;

        return spawnPos;
    }
}
