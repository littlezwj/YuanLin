using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelResultManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject resultPanel;
    public TMP_Text levelNameText;
    public TMP_Text totalScoreText;
    public TMP_Text costText;
    public TMP_Text rewardText;
    public RectTransform extraObjectsParent;
    public GameObject extraObjectPrefab;
    public List<ExtraObjects> extraObjects;
    public TMP_Text objectiveResultText;

    public Button finishBtn;

    [System.Serializable]
    public class ExtraObjects
    {
        public string label;
        public Sprite img;
    }

    private void Awake()
    {
        finishBtn.onClick.AddListener(UpdateResultUI);
        resultPanel.SetActive(false);
    }

    private void Update()
    {
        bool levelComplete = LevelConditionChecker.Instance.valueCondition.isCompleted;
        finishBtn.interactable = levelComplete;
    }

    public void UpdateResultUI()
    {
        resultPanel.SetActive(true);
        // 获取单例（确保它已经在场景中初始化）
        LevelConditionChecker checker = LevelConditionChecker.Instance;
        if (checker == null)
        {
            Debug.LogError("LevelConditionChecker.Instance not found!");
            return;
        }

        // 更新 UI 内容（这些字段你可以根据实际项目自由替换）
        if (levelNameText != null)
            levelNameText.text = "关卡名: " + checker.levelName;
        if (totalScoreText != null)
            totalScoreText.text = "得分: " + checker.valueCondition.currentCost + checker.valueCondition.reward;
        if (costText != null)
            costText.text = "" + checker.valueCondition.currentCost;
        if (rewardText != null)
            rewardText.text = "" + checker.valueCondition.reward;
        GenerateExtraObjectsList();
        //if (objectiveResultText != null)
        //    objectiveResultText.text = checker.IsObjectiveMet ? "目标达成 ✅" : "目标未完成 ❌";
    }

    public void GenerateExtraObjectsList()
    {
        if (extraObjectsParent == null || extraObjectPrefab == null)
            return;

        // 清除旧的子对象
        foreach (Transform child in extraObjectsParent.transform)
        {
            Destroy(child.gameObject);
        }

        // 遍历数据列表并生成对象
        foreach (var data in extraObjects)
        {
            GameObject obj = Instantiate(extraObjectPrefab, extraObjectsParent.transform);

            // 尝试获取 Image 或 SpriteRenderer
            Image image = obj.transform.GetChild(1).GetComponent<Image>();
            if (image != null)
            {
                image.sprite = data.img;
            }

            // 设置 TextMeshPro 文字
            TextMeshPro text = obj.GetComponentInChildren<TextMeshPro>();
            if (text != null)
            {
                text.text = data.label;
            }
        }
    }
}

