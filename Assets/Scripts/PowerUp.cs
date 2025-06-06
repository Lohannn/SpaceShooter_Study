using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public bool tripleShot = false;
    public bool heavyShot = false;
    public bool speedBoost = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    public string PowerUpActivate()
    {
        if (tripleShot)
        {
            return "TRIPLE";
        }
        else if (heavyShot)
        {
            return "HEAVY";
        }
        else if (speedBoost)
        {
            return "BOOST";
        }
        else
        {
            return "HEAL";
        }
    }
}
