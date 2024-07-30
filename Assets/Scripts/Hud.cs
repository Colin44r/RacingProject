using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreAmount;
    [SerializeField] private TextMeshProUGUI timerText;

    private int score;
    private float timer;

    public void SetScore(int value) 
    {
        score += value;
        scoreAmount.text = score.ToString();
    }

    void Start()
    {
        timer = 0;
        timerText.text = timer.ToString();
        score = 0;
        scoreAmount.text = score.ToString();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.J)) 
        {
            score += 100;
            scoreAmount.text = score.ToString();
        }

        timer += Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0;
        }
        timerText.text = timer.ToString("F1");
    }
}
