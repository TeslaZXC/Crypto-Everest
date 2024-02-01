using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

[Serializable]
public class PlayerData
{
    public int id;
    public int score;
    public string username;
    public int health;
    public int platformJumps;
    public int platformBrokens;
    public int flyJetpack;
    public int jumpBoster;

    public int necessaryPlatformJumps;
    public int necessaryPlatformBrokens;
    public int necessaryFlyJetpack;
    public int necessaryJumpBoster;
    public string lockDate;

    public PlayerData(int id, string username, int score = 0, int health = 10, int platformJumps = 0, int platformBrokens = 0, int flyJetpack = 0, 
        int jumpBoster = 0,int necessaryPlatformJumps= 0,int necessaryPlatformBrokens = 0, int necessaryFlyJetpack = 0,int necessaryJumpBoster = 0)
    {
        this.id = id;
        this.score = score;
        this.username = username;
        this.health = health;
        this.platformJumps = platformJumps;
        this.platformBrokens = platformBrokens;
        this.flyJetpack = flyJetpack;
        this.jumpBoster = jumpBoster;

        this.necessaryJumpBoster= necessaryJumpBoster;
        this.necessaryPlatformBrokens= necessaryPlatformBrokens;
        this.necessaryFlyJetpack = necessaryFlyJetpack;
        this.necessaryJumpBoster = necessaryJumpBoster;

        lockDate = null;
    }
}

public class PlayerDataResult
{
    public PlayerData data;
    public enum ResultStatus
    {
        Succes,
        UserNotFound,
        Error
    }

    public ResultStatus status;
}

public class SaveSystem : MonoBehaviour
{
    private string serverURI = "http://localhost:25565";
    public static SaveSystem singleton { get; private set; }
    private PlayerData cachedData;

    private void Awake()
    {
        singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCustomDomain(string domain)
    {
        serverURI = domain;
        Debug.Log(serverURI);
    }

    public string GetServerURI() => serverURI;

    public IEnumerator SaveDataAsync(PlayerData playerData)
    {
        cachedData = playerData;
        var json = JsonUtility.ToJson(playerData);
        var url = $"{serverURI}/api/save-userdata.php";

        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);

        UnityWebRequest www = new UnityWebRequest(url, "POST");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[SAVE] Failed to save data: {www.error}");
        }
        else
        {
            Debug.Log(string.IsNullOrEmpty(www.downloadHandler.text) ? "[SAVE] Data saved successfully" : $"[SAVE] Failed to save data: {www.downloadHandler.text}");
        }

        www?.Dispose();
    }

    public PlayerData GetCachedData() => cachedData;


    public IEnumerator LoadDataAsync(int playerId, System.Action<PlayerDataResult> onComplete)
    {
        var url = $"{serverURI}/api/get-userdata.php?id={playerId}";

        var result = new PlayerDataResult();

        UnityWebRequest www = null;
        try
        {
            www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[LOAD] Failed to load data: {www.error}");
                result.status = PlayerDataResult.ResultStatus.Error;
            }
            else
            {
                if (!ValidateJSON(www.downloadHandler.text))
                {
                    Debug.Log("[LOAD] User not found.");
                    result.status = PlayerDataResult.ResultStatus.UserNotFound;
                }
                else
                {
                    result.data = JsonUtility.FromJson<PlayerData>(www.downloadHandler.text);
                    result.status = PlayerDataResult.ResultStatus.Succes;

                    cachedData = result.data;
                }
            }
        }
        finally
        {
            www?.Dispose();
        }

        onComplete(result);
    }

    public IEnumerator GetBestPlayers(System.Action<List<PlayerData>> onComplete)
    {
        var url = $"{serverURI}/api/get-best-players.php";

        List<PlayerData> players = new List<PlayerData>();
        UnityWebRequest www = null;
        try
        {
            www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[LOAD] Failed to load data: {www.error}");
                www?.Dispose();
            }
            else
            {
                players = JsonConvert.DeserializeObject<List<PlayerData>>(www.downloadHandler.text);
            }
        }
        finally
        {
            www?.Dispose();
        }

        onComplete(players);
    }

    private bool ValidateJSON(string s)
    {
        try
        {
            JToken.Parse(s);
            return true;
        }
        catch
        {
            return false;
        }
    }
}