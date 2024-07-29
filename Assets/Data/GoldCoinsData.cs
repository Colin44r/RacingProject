using UnityEngine;


[CreateAssetMenu(fileName = "GoldCoinsConFig", menuName = "ScriptableObjects/GoldCoinsConFig", order = 0)]
public class GoldCoinsData : ScriptableObject
{
  
    [SerializeField, Min(0)] private float value = 0;
   

  

    public float GetValue()
    {
         return  value;
         
    }

}