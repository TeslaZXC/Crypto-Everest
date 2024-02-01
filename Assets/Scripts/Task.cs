using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TaskForm
{
    public string nameTask;
    public float requiredNumber;
    public float necessaryNumber;
    public float addRequiredNumber;
    public TaskType taskType;
}

public enum TaskType
{
    jumpPlatform,
    brokenPlatform,
    flyJetPack,
    jumpBooster
}

public class Task : MonoBehaviour
{
    [SerializeField] private Button openTask;
    [SerializeField] private Button closeTask;

    [SerializeField] private List<TaskForm> tasks = new List<TaskForm>();
    [SerializeField] private GameObject taskItem;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Transform taskChild;
    private Menu menu;

    private void Start()
    {
        if (SaveSystem.singleton == null)
        {
            openTask.gameObject.SetActive(false);
            return;
        }

        menu = GetComponentInParent<Menu>();
        openTask.onClick.AddListener(() => OpenTask(true));
        closeTask.onClick.AddListener(() => OpenTask(false));
        notificationPanel.SetActive(false);
        InitTask();
    }

    public void InitTask(){
        RemoveChildren();

        foreach (TaskForm task in tasks)
        {
            PlayerData data = SaveSystem.singleton.GetCachedData();
            switch (task.taskType){
                case TaskType.jumpPlatform:
                    if(data.necessaryPlatformJumps > 0)
                    {
                        task.necessaryNumber = data.necessaryPlatformJumps;
                    }
                    else
                    {
                        task.requiredNumber = data.platformJumps;
                    }
                    break;
                case TaskType.brokenPlatform:
                    if (data.necessaryPlatformBrokens > 0)
                    {
                        task.necessaryNumber = data.necessaryPlatformBrokens;
                    }
                    else
                    {
                        task.requiredNumber = data.platformBrokens;
                    }
                    break;
                case TaskType.flyJetPack:
                    if (data.necessaryFlyJetpack > 0)
                    {
                        task.necessaryNumber = data.necessaryFlyJetpack;
                    }
                    else
                    {
                        task.requiredNumber = data.flyJetpack;
                    }
                    break;
                case TaskType.jumpBooster:
                    if (data.necessaryJumpBoster > 0)
                    {
                        task.necessaryNumber = data.necessaryJumpBoster;
                    }
                    else
                    {
                        task.requiredNumber = data.jumpBoster;
                    }
                    break;
            }

            GameObject taskObject = Instantiate(taskItem, content);
            taskObject.transform.Find("TaskText").GetComponent<TMP_Text>().text = task.nameTask;
            taskObject.transform.Find("CountText").GetComponent<TMP_Text>().text = task.requiredNumber + "/" + task.necessaryNumber;

            if(task.requiredNumber >= task.necessaryNumber){
                taskObject.transform.Find("ImageBar").gameObject.SetActive(false);
                taskObject.transform.Find("ImageBackGround").gameObject.SetActive(false);
                taskObject.transform.Find("ClaimButton").GetComponent<Button>().onClick.AddListener(()=>ClaimTask(task));
                NotificationShow(true);
            }
            else{
                taskObject.transform.Find("ImageBar").GetComponent<Image>().fillAmount = task.requiredNumber / task.necessaryNumber;
                taskObject.transform.Find("ClaimButton").gameObject.SetActive(false);
                NotificationShow(false);
            }
        }
    }

    private void RemoveChildren()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    public void NotificationShow(bool option){
        notificationPanel.SetActive(option);
    }

    public void ClaimTask(TaskForm task){
        PlayerData data = SaveSystem.singleton.GetCachedData();

        task.necessaryNumber += task.addRequiredNumber;

        if (SaveSystem.singleton != null)
        {
            if (task.taskType == TaskType.jumpPlatform)
            {
                data.necessaryPlatformJumps = (int)task.necessaryNumber;
                data.jumpBoster = 0;
            }
            else if (task.taskType == TaskType.brokenPlatform)
            {
                data.necessaryPlatformBrokens = (int)task.necessaryNumber;
                data.platformBrokens = 0;
            }
            else if (task.taskType == TaskType.flyJetPack)
            {
                data.necessaryFlyJetpack = (int)task.necessaryNumber;
                data.flyJetpack = 0;
            }
            else if (task.taskType == TaskType.jumpBooster)
            {
                data.jumpBoster = (int)task.necessaryNumber;
                data.jumpBoster = 0;
            }

            task.requiredNumber = 0;

            StartCoroutine(SaveSystem.singleton.SaveDataAsync(data));
        }

        data.health+=10;

        StartCoroutine(SaveSystem.singleton.SaveDataAsync(data));
        menu.UpdateUI();

        InitTask();
        print("claim task");
    }

    public void OpenTask(bool option)
    {
        Audio.singleton.PlayCachedAudio("Click", 0.5f);
        taskChild.gameObject.SetActive(option);
    }
}