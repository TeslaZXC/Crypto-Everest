using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Coin{
    public string id;
    public int price;
    public GameObject model;
    public Vector3 scale;
    public Vector3 rotation = new Vector3(0f, 0f, -90f);
}

public class Shop : MonoBehaviour
{
    [SerializeField] private List<Coin> coinsList = new List<Coin>();
    [SerializeField] private Vector3 spawnPosition;
    

    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private TMP_Text playText;
    [SerializeField] private float rotationSpeed = 50.0f;
    private Menu menu;

    private GameObject currentCoin;
    private int index;

    public void Start()
    {
        menu = GetComponentInParent<Menu>();
        if (SaveSystem.singleton == null)
        {
            Spancoin(0);
            rightButton.gameObject.SetActive(false);
            leftButton.gameObject.SetActive(false);
            return;
        }

        ChangeCoin();

        rightButton.onClick.AddListener(() => Right());
        leftButton.onClick.AddListener(() => Left());
    }

    void Update()
    {
        if (currentCoin != null)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            currentCoin.transform.Rotate(0, rotationAmount, 0);
        }
    }

    public void Right()
    {
        index++;
        Audio.singleton.PlayCachedAudio("Click", 0.5f);

        if (index <= coinsList.Count - 1)
        {
            ChangeCoin();
        }
        else
        {
            index = 0;
            ChangeCoin();
        }
    }

    public void Left()
    {
        index--;
        Audio.singleton.PlayCachedAudio("Click", 0.5f);

        if (index < 0)
        {
            index = coinsList.Count - 1;
        }

        ChangeCoin();
    }

    public void ChangeCoin()
    {
        if (currentCoin != null)
        {
            Destroy(currentCoin);
        }

        menu.UpdateUI();
        Spancoin(index);
    }

    public Coin GetCurrentCoin() => coinsList[index];

    private void Spancoin(int index)
    {
        GameObject coinSpawn = Instantiate(coinsList[index].model);
        coinSpawn.transform.position = spawnPosition;
        coinSpawn.transform.localScale = coinsList[index].scale;
        currentCoin = coinSpawn;
    }
}