using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreAmount;
    [SerializeField] private Slider healthAmount;
    [SerializeField] private TextMeshProUGUI timerText;

    private int score;
    private int health;
    private float timer;

    public void SetScore(int value) 
    {
        score += value;
        scoreAmount.text = score.ToString();
    }

    public void SetHealth(int value) 
    {
        health = value;
        healthAmount.value = health;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 10;
        timerText.text = timer.ToString();
        score = 0;
        scoreAmount.text = score.ToString();
        health = 100;
        healthAmount.value = health; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.J)) 
        {
            score += 100;
            scoreAmount.text = score.ToString();
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            health -= 10;
            healthAmount.value = health; 
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0;
        }
        timerText.text = timer.ToString("F1");
    }
}
