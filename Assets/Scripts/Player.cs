using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private HUD hudInterface;

    [Header("Player Settings")]
    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private float defaultMoveSpeed;
    private float moveSpeed;
    [SerializeField] private float defaultBulletSpeed;
    private float bulletSpeed;
    [SerializeField] private float defaultReloadTime;
    private float reloadTime;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float invencibilityTime;

    [Header("Power Up Settings")]
    [SerializeField] private float powerUpTime;
    [SerializeField] private bool tripleShot;
    [SerializeField] private bool heavyShot;
    [SerializeField] private bool speedBoost;
    [SerializeField] private float heavyBulletMultiplier;
    [SerializeField] private float heavyBulletReloadDelayMultiplier;
    [SerializeField] private float heavyBulletSlownessMultiplier;
    [SerializeField] private float speedBoostMovementMultiplier;
    [SerializeField] private float speedBoostReloadMultiplier;
    [SerializeField] private float speedBoostBulletMultiplier;
    [SerializeField] private float healthPackValue;

    private SpriteRenderer sprite;
    private Rigidbody2D rigidBody;
    private bool damagePermission;
    private bool shootPermission;

    private Vector2 direction;

    private void Awake()
    { 
        sprite = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //StartCoroutine(Test());
        rigidBody.gravityScale = 0;

        health = maxHealth;
        moveSpeed = defaultMoveSpeed;
        bulletSpeed = defaultBulletSpeed;
        reloadTime = defaultReloadTime;

        damagePermission = true;
        shootPermission = true;
    }

    
    void Update()
    {
        hudInterface.health = health;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        //float positionX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        //float positionY = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //transform.Translate(positionX, positionY, 0.0f);

        if (Input.GetButton("Fire1") && shootPermission)
        {
            if (tripleShot)
            {
                TripleShoot(bulletSpeed, bulletDamage);
            }
            else if (heavyShot)
            {
                HeavyShoot(bulletSpeed, bulletDamage, heavyBulletMultiplier, heavyBulletSlownessMultiplier);
            }
            else
            {
                Shoot(bulletSpeed, bulletDamage);
            }
        }
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = direction.normalized * moveSpeed;
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
        friendlyBullet.GetComponent<AlliedBullet>().AddSpeed(speed);
        friendlyBullet.GetComponent<AlliedBullet>().AddDamage(damage);
        friendlyBullet.transform.position = new Vector2(transform.position.x, transform.position.y);

        StartCoroutine(ShootCoroutine(reloadTime));
    }

    private void TripleShoot(float speed, float damage)
    {
        GameObject[] bullets = new GameObject[3];

        GameObject friendlyBulletTop = Instantiate(bullet);
        GameObject friendlyBulletMiddle = Instantiate(bullet);
        GameObject friendlyBulletBot = Instantiate(bullet);

        friendlyBulletTop.GetComponent<AlliedBullet>().ActivateTopMovement();
        friendlyBulletBot.GetComponent<AlliedBullet>().ActivateBotMovement();

        bullets[0] = friendlyBulletTop;
        bullets[1] = friendlyBulletMiddle;
        bullets[2] = friendlyBulletBot;

        foreach (var bullet in bullets)
        {
            bullet.GetComponent<AlliedBullet>().AddSpeed(speed);
            bullet.GetComponent<AlliedBullet>().AddDamage(damage);
            bullet.transform.position = new Vector2(transform.position.x, transform.position.y);
        }

        StartCoroutine(ShootCoroutine(reloadTime));
    }

    private void HeavyShoot(float speed, float damage, float damageMultiplier, float slowMultiplier)
    {
        GameObject friendlyBullet = Instantiate(bullet);
        friendlyBullet.GetComponent<AlliedBullet>().AddSpeed(speed / slowMultiplier);
        friendlyBullet.GetComponent<AlliedBullet>().AddDamage(damage * damageMultiplier);
        friendlyBullet.transform.localScale = Vector3.one;
        friendlyBullet.transform.position = new Vector2(transform.position.x, transform.position.y);

        StartCoroutine(ShootCoroutine(reloadTime * heavyBulletReloadDelayMultiplier));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet") && damagePermission)
        {
            health -= collision.GetComponent<EnemyBullet>().GetDamage();
            Destroy(collision.gameObject);
            StartCoroutine(InvincibleFrames());
        }

        if (collision.gameObject.CompareTag("Enemy") && damagePermission)
        {
            health -= collision.GetComponent<Enemy>().bodyDamage;
            StartCoroutine(InvincibleFrames());
        }

        if (collision.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);
            PowerUpActivate(collision.GetComponent<PowerUp>().PowerUpActivate());
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
        StopCoroutine("TripleShotActivate");
        tripleShot = true;
        yield return new WaitForSeconds(time);
        tripleShot = false;
    }

    private IEnumerator HeavyShotActivate(float time)
    {
        StopCoroutine("HeavyShotAcrivate");
        tripleShot = true;
        yield return new WaitForSeconds(time);
        tripleShot = false;
    }

    private IEnumerator SpeedBoostActivate(float time, float speedMultiplier, float reloadMultiplier, float bulletSpeedMultiplier)
    {
        moveSpeed = defaultMoveSpeed;
        reloadTime = defaultReloadTime;
        bulletSpeed = defaultBulletSpeed;
        StopCoroutine("SpeedBoostActivate");

        moveSpeed *= speedMultiplier;
        reloadTime /= reloadMultiplier;
        bulletSpeed *= bulletSpeedMultiplier;
        yield return new WaitForSeconds(time);

        moveSpeed = defaultMoveSpeed;
        reloadTime = defaultReloadTime;
        bulletSpeed = defaultBulletSpeed;
    }

    IEnumerator Test()
    {
        print(tripleShot);
        yield return new WaitForSeconds(1);
        StartCoroutine(Test());
    }

    private void Heal(float value)
    {
        if (health + value > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += value;
        }

    }

    private void PowerUpActivate(string keyWord)
    {
        switch (keyWord)
        {
            case ("TRIPLE"):
                StartCoroutine(TripleShotActivate(powerUpTime));
                break;
            case ("HEAVY"):
                StartCoroutine(HeavyShotActivate(powerUpTime));
                break;
            case ("BOOST"):
                StartCoroutine(SpeedBoostActivate(powerUpTime, speedBoostMovementMultiplier, speedBoostReloadMultiplier, speedBoostBulletMultiplier));
                break;
            case ("HEAL"):
                Heal(healthPackValue);
                break;
        }
    }
}
