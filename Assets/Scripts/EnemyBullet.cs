using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed;
    private float damage;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float posX = -1 * speed * Time.deltaTime;

        transform.Translate(posX, 0.0f, 0.0f);

        if (transform.position.x >= 9.0f || transform.position.x <= -9.0f ||
            transform.position.y >= 5.0f || transform.position.y <= -5.0f)
        {
            Destroy(gameObject);
        }
    }

    public void AddSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void AddDamage(float newDamage)
    {
        damage = newDamage;
    }

    public float GetDamage()
    {
        return damage;
    }
}
