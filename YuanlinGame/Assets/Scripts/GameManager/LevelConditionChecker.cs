using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelConditionChecker : MonoBehaviour
{
    [System.Serializable]
    public class TagCondition
    {
        public string typeTag; // 需要检测的 Tag（如“水池”、“窗户”）
        public int requiredCount; // 需要的数量
        [HideInInspector]
        public int currentCount; // 当前数量
        [SerializeField]
        public bool isCompleted; // 是否完成（显示为勾选框）
    }

    [System.Serializable]
public class ValueCondition
{
    public float requiredHiddenValue; // 需要的隐逸值总和
    public float requiredElegantValue; // 需要的清雅值总和
    public float requiredAgileValue;   // 需要的灵动值总和
    public float requiredZenValue;     // 需要的禅意值总和
    [HideInInspector]
    public float currentHiddenValue;   // 当前隐逸值总和
    [HideInInspector]
    public float currentElegantValue;  // 当前清雅值总和
    [HideInInspector]
    public float currentAgileValue;    // 当前灵动值总和
    [HideInInspector]
    public float currentZenValue;      // 当前禅意值总和
    [HideInInspector]
    public int currentCost;            // 当前 cost 总和
    [SerializeField]
    public bool isCompleted;           // 是否完成（显示为勾选框）
}

    [Header("类型 Tag 检测条件")]
    public List<TagCondition> tagConditions = new List<TagCondition>();

    [Header("数值总和检测条件")]
    public ValueCondition valueCondition;

    private GameBoard gameBoard;

    [Header("UI References")]
    public TMPro.TextMeshProUGUI hiddenValueText;
    public TMPro.TextMeshProUGUI elegantValueText;
    public TMPro.TextMeshProUGUI agileValueText;
    public TMPro.TextMeshProUGUI zenValueText;
    public TMPro.TextMeshProUGUI costText; // 新增 cost 总和文本
    public Image hiddenValueFill;
    public Image elegantValueFill;
    public Image agileValueFill;
    public Image zenValueFill;




    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        if (gameBoard == null)
        {
            Debug.LogError("GameBoard not found in the scene!");
        }

        // Initialize UI to 0 on game load
        InitializeUI();
    }
    private void InitializeUI()
    {
        // Set all text to "0"
        hiddenValueText.text = "0";
        elegantValueText.text = "0";
        agileValueText.text = "0";
        zenValueText.text = "0";
        if (costText != null) costText.text = "0"; // 初始化 cost 文本

        // Set all fill images to 0
        hiddenValueFill.fillAmount = 0;
        elegantValueFill.fillAmount = 0;
        agileValueFill.fillAmount = 0;
        zenValueFill.fillAmount = 0;
    }
    private void UpdateUI()
    {
        // Update Text (only current value)
        hiddenValueText.text = $"{valueCondition.currentHiddenValue}";
        elegantValueText.text = $"{valueCondition.currentElegantValue}";
        agileValueText.text = $"{valueCondition.currentAgileValue}";
        zenValueText.text = $"{valueCondition.currentZenValue}";
        if (costText != null) costText.text = $"{valueCondition.currentCost}"; // 更新 cost 文本

        // Update Fill Images (normalized to 0-1 range, assuming 8 is the max)
        hiddenValueFill.fillAmount = Mathf.Clamp01(valueCondition.currentHiddenValue / 8f);
        elegantValueFill.fillAmount = Mathf.Clamp01(valueCondition.currentElegantValue / 8f);
        agileValueFill.fillAmount = Mathf.Clamp01(valueCondition.currentAgileValue / 8f);
        zenValueFill.fillAmount = Mathf.Clamp01(valueCondition.currentZenValue / 8f);
    }

    // 当物体被放置或移除时调用此方法
    public void UpdateConditions()
{
    // 重置 Tag 条件
    foreach (var condition in tagConditions)
    {
        condition.currentCount = 0;
        condition.isCompleted = false;
    }

    // 重置数值条件
    valueCondition.currentHiddenValue = 0;
    valueCondition.currentElegantValue = 0;
    valueCondition.currentAgileValue = 0;
    valueCondition.currentZenValue = 0;
    valueCondition.currentCost = 0; // 重置 cost 总和
    valueCondition.isCompleted = false;

    if (gameBoard.gamePuzzles.Count > 0)
    {
        foreach (GamePuzzle puzzle in gameBoard.gamePuzzles)
        {
            if (!puzzle.GetComponent<ItemParameters>())
                continue;
            ItemParameters itemParams = puzzle.GetComponent<ItemParameters>();
            // 更新 Tag 条件
            foreach (var condition in tagConditions)
            {
                if (itemParams.typeTag == condition.typeTag)
                {
                    condition.currentCount++;
                    condition.isCompleted = condition.currentCount >= condition.requiredCount;
                }
            }

            // 累加数值
            valueCondition.currentHiddenValue += itemParams.hiddenValue;
            valueCondition.currentElegantValue += itemParams.elegantValue;
            valueCondition.currentAgileValue += itemParams.agileValue;
            valueCondition.currentZenValue += itemParams.zenValue;
            valueCondition.currentCost += itemParams.cost; // 累加 cost
        }
    }

    // 检查数值条件
    valueCondition.isCompleted =
        valueCondition.currentHiddenValue >= valueCondition.requiredHiddenValue &&
        valueCondition.currentElegantValue >= valueCondition.requiredElegantValue &&
        valueCondition.currentAgileValue >= valueCondition.requiredAgileValue &&
        valueCondition.currentZenValue >= valueCondition.requiredZenValue;

    // 更新UI
    UpdateUI();
    // 调试输出
    Debug.Log($"数值总和 - 隐逸: {valueCondition.currentHiddenValue}/{valueCondition.requiredHiddenValue}, 清雅: {valueCondition.currentElegantValue}/{valueCondition.requiredElegantValue}, 灵动: {valueCondition.currentAgileValue}/{valueCondition.requiredAgileValue}, 禅意: {valueCondition.currentZenValue}/{valueCondition.requiredZenValue}, 成本: {valueCondition.currentCost}");
}

    // 检查所有条件是否完成
    public bool AreAllConditionsMet()
    {
        foreach (var condition in tagConditions)
        {
            if (!condition.isCompleted)
            {
                return false;
            }
        }

        return valueCondition.isCompleted;
    }
}