using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterUser
{
    public string username;
    public string userid;
    public string sitedomain;

    public EnterUser(string userid, string username, string sitedomain = "")
    {
        this.userid = userid;
        this.username = username;
        this.sitedomain = sitedomain;
    }
}

public class Auth : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText;
    private EnterUser user;

#if UNITY_EDITOR
    private void Start()
    {
        //this.SendMessage("EnterRef", 10);
        user = new EnterUser("0", "Developer");

        SaveSystemSetup();
    }
#endif

    public void EnterData(string data)
    {
        user = JsonUtility.FromJson<EnterUser>(data);

        SaveSystemSetup();
    }

    private void SaveSystemSetup()
    {
        if (string.IsNullOrEmpty(user.userid))
        {
            statusText.text = "Параметры пользователя не заданы.";
            return;
        }

        SaveSystem saveSystem = new GameObject("SaveSystem", typeof(SaveSystem)).GetComponent<SaveSystem>();

        if (user.sitedomain != string.Empty)
            saveSystem.SetCustomDomain(user.sitedomain);

        StartCoroutine(saveSystem.LoadDataAsync(int.Parse(user.userid), OnGetPlayerDataFromServer));
    }

    private void OnGetPlayerDataFromServer(PlayerDataResult result)
    {
        if (result.status == PlayerDataResult.ResultStatus.Error)
        {
            statusText.text = "Не удалось загрузить данные пользователя.";
            return;
        }    

        if (RBDeviceType.isMobile())
        {
            statusText.text = "Приложение не поддерживается на десктопе.";
            return;
        }

        if (result.status == PlayerDataResult.ResultStatus.UserNotFound)
        {
            Debug.Log("New User");
            PlayerData newUser = new PlayerData(int.Parse(user.userid), user.username);

            StartCoroutine(SaveSystem.singleton.SaveDataAsync(newUser));
            Debug.Log("Save new User...");
        }

        if (SceneTransition.singleton != null)
            SceneTransition.singleton.ChangeScene("Menu");
        else
            SceneManager.LoadScene("Menu");
    }
}