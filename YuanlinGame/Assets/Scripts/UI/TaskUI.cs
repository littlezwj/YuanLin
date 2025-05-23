using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TaskUI : MonoBehaviour
{
    [Header("任务样式预设")]
    public GameObject ItemtaskPrefab; // 单个任务 UI 的预设

    [Header("值的任务样式预设")]
    public GameObject HiddenValueTaskPrefab; // 隐逸值任务 UI 的预设
    public GameObject ElegantValueTaskPrefab; // 清雅值任务 UI 的预设
    public GameObject AgileValueTaskPrefab; // 灵动值任务 UI 的预设
    public GameObject ZenValueTaskPrefab; // 禅意值任务 UI 的预设

    [Header("任务列表容器")]
    public Transform ItemtaskContainer; // 用于放置任务 UI 的父物体
    public Transform ValuetaskContainer; // 用于放置值的任务 UI 的父物体

    private LevelConditionChecker levelConditionChecker;
    private List<GameObject> taskUIList = new List<GameObject>(); // 存储生成的任务 UI
    private List<GameObject> valueTaskUIList = new List<GameObject>(); // 存储生成的值任务 UI

    private void Start()
    {
        levelConditionChecker = FindObjectOfType<LevelConditionChecker>();
        if (levelConditionChecker == null)
        {
            Debug.LogError("LevelConditionChecker not found in the scene!");
            return;
        }

        GenerateTaskList();
        GenerateValueTaskList(); // 生成值的任务 UI
    }

    private void Update()
    {
        // 实时更新任务完成状态
        UpdateTaskCompletionStatus();
        UpdateValueTaskCompletionStatus(); // 更新值的任务完成状态
    }
    private void GenerateValueTaskList()
    {
        // 清空容器和列表
        foreach (Transform child in ValuetaskContainer)
        {
            Destroy(child.gameObject);
        }
        valueTaskUIList.Clear();

        // 检查四种值是否需要显示
        var valueCondition = levelConditionChecker.valueCondition;
        if (valueCondition.requiredHiddenValue > 0)
        {
            CreateValueTaskUI(HiddenValueTaskPrefab, "隐逸值", valueCondition.requiredHiddenValue);
        }
        if (valueCondition.requiredElegantValue > 0)
        {
            CreateValueTaskUI(ElegantValueTaskPrefab, "清雅值", valueCondition.requiredElegantValue);
        }
        if (valueCondition.requiredAgileValue > 0)
        {
            CreateValueTaskUI(AgileValueTaskPrefab, "灵动值", valueCondition.requiredAgileValue);
        }
        if (valueCondition.requiredZenValue > 0)
        {
            CreateValueTaskUI(ZenValueTaskPrefab, "禅意值", valueCondition.requiredZenValue);
        }
    }

    private void CreateValueTaskUI(GameObject prefab, string description, float requiredValue)
    {
        if (prefab == null || ValuetaskContainer == null)
        {
            Debug.LogError("Value task prefab or container is not assigned!");
            return;
        }

        GameObject valueTaskUI = Instantiate(prefab, ValuetaskContainer);
        TextMeshProUGUI descriptionText = valueTaskUI.GetComponentInChildren<TextMeshProUGUI>();
        if (descriptionText != null)
        {
            descriptionText.text = $"{description} >= {requiredValue}";
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in value task prefab!");
        }

        valueTaskUIList.Add(valueTaskUI);
    }

    private void UpdateValueTaskCompletionStatus()
    {
        var valueCondition = levelConditionChecker.valueCondition;
        for (int i = 0; i < valueTaskUIList.Count; i++)
        {
            GameObject valueTaskUI = valueTaskUIList[i];
            Transform completeImage = valueTaskUI.transform.Find("complete");
            if (completeImage != null)
            {
                // 根据值的完成状态更新 UI
                bool isCompleted = false;
                switch (i)
                {
                    case 0:
                        isCompleted = valueCondition.currentHiddenValue >= valueCondition.requiredHiddenValue;
                        break;
                    case 1:
                        isCompleted = valueCondition.currentElegantValue >= valueCondition.requiredElegantValue;
                        break;
                    case 2:
                        isCompleted = valueCondition.currentAgileValue >= valueCondition.requiredAgileValue;
                        break;
                    case 3:
                        isCompleted = valueCondition.currentZenValue >= valueCondition.requiredZenValue;
                        break;
                }
                completeImage.gameObject.SetActive(isCompleted);
            }
            else
            {
                Debug.LogError("Complete image not found in value task prefab!");
            }
        }
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