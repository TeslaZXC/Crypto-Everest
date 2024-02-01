using TMPro;
using UnityEngine;

public class LBItem : MonoBehaviour
{
    [SerializeField] private TMP_Text indexText;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetInfo(int index, string username, int score)
    {
        indexText.text = $"{index}.";
        usernameText.text = username;
        scoreText.text = score.ToString();
    }
}
