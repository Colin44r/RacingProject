using UnityEngine;

public class GoldCoins : MonoBehaviour
{
    private int value;
    [SerializeField]  private GoldCoinsData CoinConfig;
   
 


    // Start is called before the first frame update
    void Start()
    {
       value = 100;
    }

    private void OnDisable()
    {
       // GameManager.Instance().SpawnGoldCoin();
        GameManager.Instance().GetHud().SetScore(value);
    }
}