using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bullet;
    public GameObject powerUp;
    public HUD hudInterface;
    public float health;
    public float bulletSpeed;
    public float reloadTime;
    public float bulletDamage;
    public float bodyDamage;

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
            hudInterface.enemyCount -= 1;

            if (Random.Range(1,4) == 1)
            {
                PowerUpDrop(Random.Range(0, 6));
            }
            
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AlliedBullet"))
        {
            health -= collision.GetComponent<AlliedBullet>().GetDamage();
            Destroy(collision.gameObject);
        }
    }

    IEnumerator Shoot(float speed, float reload, float damage)
    {
        yield return new WaitForSeconds(reload);

        GameObject enemyBullet = Instantiate(bullet);
        enemyBullet.GetComponent<EnemyBullet>().AddSpeed(speed);
        enemyBullet.GetComponent<EnemyBullet>().AddDamage(damage);
        enemyBullet.transform.position = new Vector2(transform.position.x, transform.position.y);

        StartCoroutine(Shoot(speed, reload, damage));
    }

    private void PowerUpDrop(int type)
    {
        GameObject droppedPowerUp = Instantiate(powerUp);
        droppedPowerUp.transform.position = new Vector2(transform.position.x, transform.position.y);

        switch (type)
        {
            case 0:
                droppedPowerUp.GetComponent<PowerUp>().tripleShot = true;
                break;
            case 1:
                droppedPowerUp.GetComponent<PowerUp>().heavyShot = true;
                break;
            case 2:
                droppedPowerUp.GetComponent<PowerUp>().speedBoost = true;
                break;

        }
    }
}
