using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Shop shop;

    [SerializeField] private Button playButton;
    [SerializeField] private TMP_Text playButtonText;

    private void Start()
    {
        playButton.onClick.AddListener(Play);
        ShowBestScore();
        ShowHealth();
        UpdateUI();
    }

    public void EnterRef(int count)
    {
        PlayerData data = SaveSystem.singleton.GetCachedData();

        if (SaveSystem.singleton != null)
        {
            data.health += count;
        }

        StartCoroutine(SaveSystem.singleton.SaveDataAsync(data));
    }

    private void ShowBestScore()
    {
        if (SaveSystem.singleton == null)
        {
            scoreText.gameObject.SetActive(false);
            return;
        }

        scoreText.text = $"Лучший счёт:\n{SaveSystem.singleton.GetCachedData().score}";
    }

    private void ShowHealth()
    {
        if (SaveSystem.singleton == null)
        {
            healthText.gameObject.SetActive(false);
            return;
        }

        healthText.text = SaveSystem.singleton.GetCachedData().health <= 0 ? $"Новая энергия будет доступна завтра!" : $"энергии: {SaveSystem.singleton.GetCachedData().health}<sprite name=\"energy\">";
    }

    public void UpdateUI()
    {
        if (SaveSystem.singleton == null) return;

        ShowHealth();

        PlayerData data = SaveSystem.singleton.GetCachedData();

        if (data.health <= 0)
        {
            playButton.interactable = false;
            playButtonText.text = "Нет энергии";

            if(!string.IsNullOrEmpty(data.lockDate))
            {
                if ((DateTime.Now - DateTime.Parse(data.lockDate)).TotalHours >= 12)
                {
                    data.lockDate = null;
                    data.health += 10;
                    StartCoroutine(SaveSystem.singleton.SaveDataAsync(data));
                }
            }
            else
            {
                data.lockDate = DateTime.Now.ToString();
                StartCoroutine(SaveSystem.singleton.SaveDataAsync(data));
            }

            

            return;
        }

        if (data.score < shop.GetCurrentCoin().price)
        {
            playButton.interactable = false;
            playButtonText.text = $"Не хватает ({shop.GetCurrentCoin().price - SaveSystem.singleton.GetCachedData().score})";
            return;
        }

        playButton.interactable = true;
        playButtonText.text = "Играть";
    }

    public void Play()
    {
        Audio.singleton.PlayCachedAudio("Click", 0.5f);
        Player.coinSkin = shop.GetCurrentCoin();
        SceneTransition.singleton.ChangeScene("Main");
    }
}
