using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject powerUp;
    [SerializeField] private HUD hudInterface;

    [Header("Spawn Settings")]
    [SerializeField] private float timeToSpawn;
    [SerializeField] private int enemyQuantity;

    [Header("Enemy Settings")]
    [SerializeField] private float health;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float reloadTime;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float bodyDamage;

    void Start()
    {
        StartCoroutine(EnemySpawn(timeToSpawn));
    }

    private IEnumerator EnemySpawn(float time)
    {
        if (hudInterface.enemyCount < enemyQuantity)
        {
            hudInterface.enemyCount++;
            GameObject spawnedEnemy = Instantiate(enemy);
            spawnedEnemy.transform.position = new Vector2(5.0f, (Random.Range(-4.5f, 4.6f)));

            spawnedEnemy.GetComponent<Enemy>().bullet = bullet;
            spawnedEnemy.GetComponent<Enemy>().powerUp = powerUp;
            spawnedEnemy.GetComponent<Enemy>().hudInterface = hudInterface;

            spawnedEnemy.GetComponent<Enemy>().health = health;
            spawnedEnemy.GetComponent<Enemy>().bulletSpeed = bulletSpeed;
            spawnedEnemy.GetComponent<Enemy>().reloadTime = reloadTime;
            spawnedEnemy.GetComponent<Enemy>().bulletDamage = bulletDamage;
            spawnedEnemy.GetComponent<Enemy>().bodyDamage = bodyDamage;
        }

        yield return new WaitForSeconds(time);
        StartCoroutine(EnemySpawn(time));
    }
}
