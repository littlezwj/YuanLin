using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField]
        public bool isCompleted;           // 是否完成（显示为勾选框）
    }

    [Header("类型 Tag 检测条件")]
    public List<TagCondition> tagConditions = new List<TagCondition>();

    [Header("数值总和检测条件")]
    public ValueCondition valueCondition;

    private GameBoard gameBoard;

    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        if (gameBoard == null)
        {
            Debug.LogError("GameBoard not found in the scene!");
        }
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
    valueCondition.isCompleted = false;

    // 遍历所有格子
    for (int i = 0; i < gameBoard.size.x * gameBoard.size.y; i++)
    {
        GameTile tile = gameBoard.GetTileAt(i % gameBoard.size.x, i / gameBoard.size.x);
        if (tile == null || tile.Content == null) continue;

        // 检测 Tag 条件
        if (tile.Content.Type == GameTileContentType.Tool)
        {
            ItemParameters itemParams = tile.Content.GetComponent<ItemParameters>();
            if (itemParams != null)
            {
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
            }
        }
    }

    // 检查数值条件
    valueCondition.isCompleted =
        valueCondition.currentHiddenValue >= valueCondition.requiredHiddenValue &&
        valueCondition.currentElegantValue >= valueCondition.requiredElegantValue &&
        valueCondition.currentAgileValue >= valueCondition.requiredAgileValue &&
        valueCondition.currentZenValue >= valueCondition.requiredZenValue;

    // 调试输出
    Debug.Log($"数值总和 - 隐逸: {valueCondition.currentHiddenValue}/{valueCondition.requiredHiddenValue}, 清雅: {valueCondition.currentElegantValue}/{valueCondition.requiredElegantValue}, 灵动: {valueCondition.currentAgileValue}/{valueCondition.requiredAgileValue}, 禅意: {valueCondition.currentZenValue}/{valueCondition.requiredZenValue}");
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