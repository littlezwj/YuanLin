using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TaskUI : MonoBehaviour
{
    [Header("任务样式预设")]
    public GameObject ItemtaskPrefab; // 单个任务 UI 的预设

    [Header("任务列表容器")]
    public Transform ItemtaskContainer; // 用于放置任务 UI 的父物体

    private LevelConditionChecker levelConditionChecker;
    private List<GameObject> taskUIList = new List<GameObject>(); // 存储生成的任务 UI

    private void Start()
    {
        levelConditionChecker = FindObjectOfType<LevelConditionChecker>();
        if (levelConditionChecker == null)
        {
            Debug.LogError("LevelConditionChecker not found in the scene!");
            return;
        }

        GenerateTaskList();
    }

    private void Update()
    {
        // 实时更新任务完成状态
        UpdateTaskCompletionStatus();
    }

    private void GenerateTaskList()
    {
        // 清空容器和列表
        foreach (Transform child in ItemtaskContainer)
        {
            Destroy(child.gameObject);
        }
        taskUIList.Clear();

        // 生成 Tag 任务
        for (int i = 0; i < levelConditionChecker.tagConditions.Count; i++)
        {
            var tagCondition = levelConditionChecker.tagConditions[i];
            GameObject taskUI = CreateTaskUI(tagCondition.description, tagCondition.rewardAmount, i);
            taskUIList.Add(taskUI);
        }
    }

    private GameObject CreateTaskUI(string description, int rewardAmount, int conditionIndex)
    {
        if (ItemtaskPrefab == null || ItemtaskContainer == null)
        {
            Debug.LogError("Task prefab or container is not assigned!");
            return null;
        }

        GameObject taskUI = Instantiate(ItemtaskPrefab, ItemtaskContainer);
        TextMeshProUGUI descriptionText = taskUI.GetComponentInChildren<TextMeshProUGUI>();
        if (descriptionText != null)
        {
            descriptionText.text = description;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in task prefab!");
        }

        // 绑定任务索引到 UI 对象
        taskUI.name = $"Task_{conditionIndex}";
        return taskUI;
    }

    private void UpdateTaskCompletionStatus()
    {
        for (int i = 0; i < levelConditionChecker.tagConditions.Count; i++)
        {
            if (i >= taskUIList.Count) break; // 防止越界

            var tagCondition = levelConditionChecker.tagConditions[i];
            GameObject taskUI = taskUIList[i];

            // 查找 complete 图片并更新显示状态
            Transform completeImage = taskUI.transform.Find("complete"); // 根据实际路径调整
            if (completeImage != null)
            {
                completeImage.gameObject.SetActive(tagCondition.isCompleted);
            }
            else
            {
                Debug.LogError("Complete image not found in task prefab!");
            }
        }
    }
}