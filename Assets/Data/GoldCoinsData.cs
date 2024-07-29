using UnityEngine;


[CreateAssetMenu(fileName = "RupeeConFig", menuName = "ScriptableObject/RupeeConFig", order = 0)]
public class GoldCoinsData : ScriptableObject
{
  
    [SerializeField, Min(0)] private float value = 0;
    [SerializeField] private Sprite GoldCoins;

   // public float Value => value;

    public float GetValue()
    {
         return  value;
         
    }

}