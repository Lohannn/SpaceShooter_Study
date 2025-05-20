using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private Player player;

    public bool healthPack = false;
    public bool tripleShot = false;
    public bool heavyShot = false;
    public bool speedBoost = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    public void PowerUpActivate()
    {
        if (healthPack)
        {

        }
        else if (tripleShot)
        {

        }
        else if (heavyShot)
        {

        }
        else if (speedBoost)
        {

        }
    }
}
