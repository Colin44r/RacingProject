using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private Hud hud;
    [SerializeField] private GameObject goldCoins;
    [SerializeField] private Transform[] spawnPoints;

    private int spawnPoint = 0;

    private void Awake()
    {
        SetInstance();
    }

    //private void Start()
    //{
    //    GoldCoinRupee();
    //}

    private void SetInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
       
        DontDestroyOnLoad(gameObject);
    }

    public static GameManager Instance()
    { 
        return instance;
    }

    //public void SpawnRupee()
    //{
    //    GameObject temp = Instantiate(goldCoins, spawnPoints[Random.Range(0, spawnPoints.Length)].position, transform.rotation);

    //    temp.gameObject.name = "Rupee";
    //}

    public Hud GetHud() 
    { 
        return hud; 
    }
}
