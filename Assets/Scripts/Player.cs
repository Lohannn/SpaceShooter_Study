using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    [Header("Player Settings")]
    [SerializeField] private float health;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float reloadTime;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float invencibilityTime;

    [Header("Power Up Settings")]
    [SerializeField] private float powerUpTime;
    [SerializeField] private bool tripleShot;
    [SerializeField] private bool heavyShot;
    [SerializeField] private float heavyBulletMultiplier;
    [SerializeField] private float heavyBulletReloadDelayMultiplier;
    [SerializeField] private float speedBoostMovementMultiplier;
    [SerializeField] private float speedBoostReloadMultiplier;
    [SerializeField] private float speedBoostBulletMultiplier;
    [SerializeField] private float healthPackValue;

    private SpriteRenderer sprite;
    private bool damagePermission;
    private bool shootPermission;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        damagePermission = true;
        shootPermission = true;
    }

    
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        float positionX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float positionY = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        
        transform.Translate(positionX, positionY, 0.0f);

        if (Input.GetButton("Fire1") && shootPermission)
        {
            if (tripleShot)
            {
                TripleShoot(bulletSpeed, bulletDamage);
            }
            else if (heavyShot)
            {
                HeavyShoot(bulletSpeed, bulletDamage);
            }
            else
            {
                Shoot(bulletSpeed, bulletDamage);
            }
        }
    }

    private IEnumerator ShootCoroutine(float reload)
    {
        shootPermission = false;
        yield return new WaitForSeconds(reload);
        shootPermission = true;
    }

    private void Shoot(float speed, float damage)
    {
        GameObject friendlyBullet = Instantiate(bullet);
        friendlyBullet.GetComponent<AlliedBullet>().speed = speed;
        friendlyBullet.GetComponent<AlliedBullet>().damage = damage;
        friendlyBullet.transform.position = new Vector2(transform.position.x, transform.position.y);

        StartCoroutine(ShootCoroutine(reloadTime));
    }

    private void TripleShoot(float speed, float damage)
    {
        GameObject[] bullets = new GameObject[3];

        GameObject friendlyBulletTop = Instantiate(bullet);
        GameObject friendlyBulletMiddle = Instantiate(bullet);
        GameObject friendlyBulletBot = Instantiate(bullet);

        friendlyBulletTop.GetComponent<AlliedBullet>().topMovement = true;
        friendlyBulletBot.GetComponent<AlliedBullet>().botMovement = true;

        bullets[0] = friendlyBulletTop;
        bullets[1] = friendlyBulletMiddle;
        bullets[2] = friendlyBulletBot;

        foreach (var bullet in bullets)
        {
            bullet.GetComponent<AlliedBullet>().speed = speed;
            bullet.GetComponent<AlliedBullet>().damage = damage;
            bullet.transform.position = new Vector2(transform.position.x, transform.position.y);
        }

        StartCoroutine(ShootCoroutine(reloadTime));
    }

    private void HeavyShoot(float speed, float damage)
    {
        GameObject friendlyBullet = Instantiate(bullet);
        friendlyBullet.GetComponent<AlliedBullet>().speed = speed / 2;
        friendlyBullet.GetComponent<AlliedBullet>().damage = damage * heavyBulletMultiplier;
        friendlyBullet.transform.localScale = Vector3.one;
        friendlyBullet.transform.position = new Vector2(transform.position.x, transform.position.y);

        StartCoroutine(ShootCoroutine(reloadTime * heavyBulletReloadDelayMultiplier));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet") && damagePermission)
        {
            health -= collision.GetComponent<EnemyBullet>().damage;
            Destroy(collision.gameObject);
            StartCoroutine(InvincibleFrames());
        }

        if (collision.gameObject.CompareTag("Enemy") && damagePermission)
        {
            health -= collision.GetComponent<Enemy>().bodyDamage;
            StartCoroutine(InvincibleFrames());
        }

        if (collision.gameObject.CompareTag("TripleShotPOWER"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(TripleShotActivate(powerUpTime));
        }
    }

    public IEnumerator InvincibleFrames()
    {
        sprite.color = Color.black;
        damagePermission = false;
        yield return new WaitForSeconds(invencibilityTime);
        sprite.color = Color.white;
        damagePermission = true;
    }

    private IEnumerator TripleShotActivate(float time)
    {
        tripleShot = true;
        yield return new WaitForSeconds(time);
        tripleShot = false;
    }

    private IEnumerator HeavyShotActivate(float time)
    {
        tripleShot = true;
        yield return new WaitForSeconds(time);
        tripleShot = false;
    }

    private IEnumerator SpeedBoostActivate(float time, float speedMultiplier, float reloadMultiplier, float bulletSpeedMultiplier)
    {
        float currentSpeed = moveSpeed;
        float currentReload = reloadTime;
        float currentBulletSpeed = bulletSpeed;
        
        moveSpeed *= speedMultiplier;
        reloadTime /= reloadMultiplier;
        bulletSpeed *= bulletSpeedMultiplier;
        yield return new WaitForSeconds(time);

        moveSpeed = currentSpeed;
        reloadTime = currentReload;
        bulletSpeed = currentBulletSpeed;
    }

    private void Heal(float value)
    {
        health += value;
    }

    private void PowerUpActivate(string keyWord)
    {
        switch (keyWord)
        {
            case ("TRIPLE"):
                break;
        }
    }
}
