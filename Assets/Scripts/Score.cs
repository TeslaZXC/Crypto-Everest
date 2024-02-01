using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }
    private TMP_Text scoreText;
    private int score;
    
    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        Instance = this;
    }

    public void AddScore(int amount)
    {
        if (Player.instance.canPlay)
        {
            score += amount;
            scoreText.text = score.ToString();
        }
    }

    public int GetScore() => score;
}