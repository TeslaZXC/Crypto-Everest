using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PromocodeData
{
    public string code;
    public int created;

    public PromocodeData()
    {
        this.code = GenerateKey.CreateKey();
        this.created = SaveSystem.singleton.GetCachedData().id;
    }
}


public class Promocodes : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createCodePageButton;
    [SerializeField] private Button createCodeButton;
    [SerializeField] private Button enterCodePageButton;
    [SerializeField] private Button enterCodeButton;
    [SerializeField] private TMP_Text tipText;
    [SerializeField] private TMP_Text createCodeFieldText;
    [SerializeField] private TMP_InputField enterCodeFieldText;

    [Header("Pages")]
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject thisPage;
    [SerializeField] private GameObject localMainPage;
    [SerializeField] private GameObject localCreateCodePage;
    [SerializeField] private GameObject localEnterCodePage;
    private List<GameObject> localPages = new List<GameObject>();
    private Menu menu;

    private void Start()
    {
        menu = GetComponentInParent<Menu>();
        InitializingButtons();
        InitializingPages();
    }

    private void InitializingPages()
    {
        localPages.Add(localMainPage);
        localPages.Add(localCreateCodePage);
        localPages.Add(localEnterCodePage);
    }

    private void InitializingButtons()
    {
        openButton.onClick.AddListener(() => OpenPromocodes(true));
        closeButton.onClick.AddListener(() => OpenPromocodes(false));
        createCodePageButton.onClick.AddListener(() => OpenPage(localCreateCodePage));
        enterCodePageButton.onClick.AddListener(() => OpenPage(localEnterCodePage));
        createCodeButton.onClick.AddListener(() => StartCoroutine(CreateCode()));
        enterCodeButton.onClick.AddListener(() => StartCoroutine(UseCode()));
    }

    private void OpenPage(GameObject page)
    {
        Audio.singleton.PlayCachedAudio("Click", 0.5f);
        foreach (var item in localPages)
        {
            item.SetActive(false);
        }

        page.SetActive(true);
    }

    private void OpenPromocodes(bool option)
    {
        Audio.singleton.PlayCachedAudio("Click", 0.5f);

        if (!option && !localMainPage.activeInHierarchy)
        {
            OpenPage(localMainPage);
            return;
        }

        thisPage.SetActive(option);
        tipText.gameObject.SetActive(option);
        mainPage.SetActive(!option);
    }

    private IEnumerator CreateCode()
    {
        Audio.singleton.PlayCachedAudio("Click", 0.5f);

        if (SaveSystem.singleton != null)
        {
            PromocodeData promocode = new PromocodeData();
            var json = JsonUtility.ToJson(promocode);
            var url = $"{SaveSystem.singleton.GetServerURI()}/api/create-promocode.php";

            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);

            UnityWebRequest www = new UnityWebRequest(url, "POST");
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (!string.IsNullOrEmpty(www.downloadHandler.text))
                promocode.code = www.downloadHandler.text;

            createCodeFieldText.text = promocode.code;

            www?.Dispose();
        }
    }

    private IEnumerator UseCode()
    {
        Audio.singleton.PlayCachedAudio("Click", 0.5f);

        if(enterCodeFieldText.text == string.Empty || enterCodeFieldText.text.Length > 6)
        {
            enterCodeFieldText.text = "Пустое значение.";
            yield break;
        }

        if (SaveSystem.singleton != null)
        {
            string code = enterCodeFieldText.text.ToUpper();
            var url = $"{SaveSystem.singleton.GetServerURI()}/api/enter-promocode.php?code={code}&id={SaveSystem.singleton.GetCachedData().id}";

            UnityWebRequest www = null;
            try
            {
                www = UnityWebRequest.Get(url);
                yield return www.SendWebRequest();

                string responseText = www.downloadHandler.text;
                Debug.Log(responseText);
                switch (responseText)
                {
                    case "Success":
                        OnUseCodeSuccess();
                        break;
                    case "Failed to load data":
                        enterCodeFieldText.text = "Не удалось загрузить данные.";
                        break;
                    case "No promocode found":
                        enterCodeFieldText.text = "Промокод не найден.";
                        break;
                    case "You cannot use your promocode":
                        enterCodeFieldText.text = "Нельзя использовать свой промокод.";
                        break;
                }
            }
            finally
            {
                www?.Dispose();
            }
        }
    }

    private void OnUseCodeSuccess()
    {
        Audio.singleton.PlayCachedAudio("Coin", 0.5f);
        enterCodeFieldText.text = "<color=green>Успешно</color>";
        PlayerData data = SaveSystem.singleton.GetCachedData();
        data.health += 10;
        menu.UpdateUI();
        StartCoroutine(SaveSystem.singleton.SaveDataAsync(data));
    }
}
