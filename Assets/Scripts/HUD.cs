using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Text healtText; 
    [SerializeField] private Text enemyCountText;

    public float health;
    public int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healtText.text = $"Health: {health}";
        enemyCountText.text = $"Enemies: {enemyCount}";
    }
}
