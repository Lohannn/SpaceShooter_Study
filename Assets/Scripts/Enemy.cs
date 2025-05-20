using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    [Header("Enemy Settings")]
    [SerializeField] private float health;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float reloadTime;
    [SerializeField] private float bulletDamage;
    public int bodyDamage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Shoot(bulletSpeed, reloadTime, bulletDamage));
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AlliedBullet"))
        {
            health -= collision.GetComponent<AlliedBullet>().damage;
            Destroy(collision.gameObject);
        }
    }

    IEnumerator Shoot(float speed, float reload, float damage)
    {
        yield return new WaitForSeconds(reload);

        GameObject enemyBullet = Instantiate(bullet);
        enemyBullet.GetComponent<EnemyBullet>().speed = speed;
        enemyBullet.GetComponent<EnemyBullet>().damage = damage;
        enemyBullet.transform.position = new Vector2(transform.position.x, transform.position.y);

        StartCoroutine(Shoot(speed, reload, damage));
    }
}
