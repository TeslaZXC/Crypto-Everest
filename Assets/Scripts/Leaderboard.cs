using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject page;
    [SerializeField] private Transform content;
    [SerializeField] private LBItem LBItem;

    private void Start()
    {
        openButton.onClick.AddListener(() => OpenLeaderboard(true));
        closeButton.onClick.AddListener(() => OpenLeaderboard(false));

        if (SaveSystem.singleton == null)
        {
            openButton.gameObject.SetActive(false);
        }
    }

    private void OpenLeaderboard(bool option)
    {
        Audio.singleton.PlayCachedAudio("Click", 0.5f);
        page.SetActive(option);

        if (option == true)
        {
            StartCoroutine(SaveSystem.singleton.GetBestPlayers(LeaderboardLoaded));
        }
        else
        {
            for (int i = 0; i < content.transform.childCount; i++)
            {
                GameObject child = content.transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }
    }

    private void LeaderboardLoaded(List<PlayerData> players)
    {
        int index = 0;

        foreach (PlayerData player in players)
        {
            index++;
            GameObject item = Instantiate(LBItem.gameObject, content);
            item.GetComponent<LBItem>().SetInfo(index, player.username, player.score);
        }
    }
}
