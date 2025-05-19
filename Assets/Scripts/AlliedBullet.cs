using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedBullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public bool topMovement;
    public bool botMovement;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float posX = 1 * speed * Time.deltaTime;
        float posY = 0.2f * speed * Time.deltaTime;

        if (topMovement)
        {
            transform.Translate(posX, posY, 0.0f);
        }
        else if (botMovement) 
        {
            transform.Translate(posX, -posY, 0.0f);
        }
        else
        {
            transform.Translate(posX, 0.0f, 0.0f);
        }
        

        if (transform.position.x >= 9.0f || transform.position.x <= -9.0f || 
            transform.position.y >= 5.0f || transform.position.y <= -5.0f)
        {
            Destroy(gameObject);
        }
    }
}
