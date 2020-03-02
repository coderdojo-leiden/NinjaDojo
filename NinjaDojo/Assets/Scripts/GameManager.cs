using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private Text coinTxt;

    private int collectedCoins;

    public int CollectedCoins
    {
        get
        {
            return collectedCoins;
        }
        set
        {
            coinTxt.text = value.ToString();
            this.collectedCoins = value;
        }
    }


    public GameObject CoinPrefab
    {
        get
        {
            return coinPrefab;
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
