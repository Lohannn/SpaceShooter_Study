using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    [Header("Player Settings")]
    public float health;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float reloadTime;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float invencibilityTime;

    [Header("Bullet Settings")]
    [SerializeField] private float heavyBulletMultiplier;
    [SerializeField] private float heavyBulletReloadDelayMultiplier;

    [Header("Shoot Styles")]
    [SerializeField] private bool tripleShot;
    [SerializeField] private bool heavyShot;

    private SpriteRenderer sprite;
    private BoxCollider2D collider;
    private bool shootPermission;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

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

    private IEnumerator ShootCoroutine(float reload)
    {
        shootPermission = false;
        yield return new WaitForSeconds(reload);
        shootPermission = true;
    }

    public IEnumerator InvincibleFrames()
    {
        sprite.color = Color.black;
        collider.enabled = false;
        yield return new WaitForSeconds(invencibilityTime);
        sprite.color = Color.white;
        collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            health -= collision.GetComponent<EnemyBullet>().damage;
            Destroy(collision.gameObject);
            StartCoroutine(InvincibleFrames());
        }
    }
}
