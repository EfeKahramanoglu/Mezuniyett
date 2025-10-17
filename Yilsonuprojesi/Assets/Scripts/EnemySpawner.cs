// ...existing code...
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    float spawnInterval = 5f; // Her 5 saniyede bir spawn
    float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnEnemyOnRandomGround();
        }
    }

    void SpawnEnemyOnRandomGround()
    {
        timer = 0f;
        // Tüm "Ground" tag'li nesneleri bul
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
        if (grounds.Length == 0 || enemyPrefab == null)
        {
            Debug.LogWarning("Ground bulunamadı veya enemyPrefab atanmadı!");
            return;
        }

        // Sahnedeki "Player" tag'li nesneyi bul ve Transform ataması yap
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform; // Player'ın Transform referansını al
            }
            else
            {
                Debug.LogWarning("Player bulunamadı! Lütfen sahnede 'Player' tag'li bir obje olduğundan emin olun.");
                return;
            }
        }

        // Rastgele bir ground seç
        GameObject selectedGround = grounds[Random.Range(0, grounds.Length)];
        Collider groundCollider = selectedGround.GetComponent<Collider>();
        if (groundCollider == null)
        {
            Debug.LogWarning("Seçilen ground'da Collider yok!");
            return;
        }

        // Collider'ın bounds'ı içinde rastgele bir nokta seç
        Bounds bounds = groundCollider.bounds;
        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.max.y + 0.75f, // Yükseklik: zeminin biraz üstü
            Random.Range(bounds.min.z, bounds.max.z)
        );

        // Enemy prefabını oluştur
        GameObject enemy = Instantiate(enemyPrefab, randomPoint, Quaternion.identity);

        // Enemy script'ine player referansını ata (hem Transform hem GameObject tipine uyumlu şekilde)
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            var field = enemyScript.GetType().GetField("player", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                if (field.FieldType == typeof(Transform))
                    field.SetValue(enemyScript, player);
                else if (field.FieldType == typeof(GameObject))
                    field.SetValue(enemyScript, player.gameObject);
            }
            else
            {
                var prop = enemyScript.GetType().GetProperty("player", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop != null)
                {
                    if (prop.PropertyType == typeof(Transform))
                        prop.SetValue(enemyScript, player);
                    else if (prop.PropertyType == typeof(GameObject))
                        prop.SetValue(enemyScript, player.gameObject);
                }
                else
                {
                    // Eğer Enemy script'i farklı isimde bir alan/propery kullanıyorsa, manuel atama gerekebilir.
                    Debug.LogWarning("Enemy script'inde 'player' alanı veya propertysi bulunamadı. Manuel atama yapın.");
                }
            }
        }
    }
}
// ...existing code...