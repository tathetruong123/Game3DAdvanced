using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab của quái
    public Vector3 spawnCenter; // Tâm khu vực sinh
    public float spawnRadius = 10f; // Bán kính khu vực sinh
    public float spawnInterval = 10f; // Thời gian giữa mỗi lần sinh

    private void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    private System.Collections.IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        // Random một vị trí trong vòng tròn
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(spawnCenter.x + randomCircle.x, spawnCenter.y, spawnCenter.z + randomCircle.y);

        // Tạo quái tại vị trí ngẫu nhiên
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
