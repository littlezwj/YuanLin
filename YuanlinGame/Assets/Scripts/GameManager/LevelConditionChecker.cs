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
    // 重置计数（仅一次）
    foreach (var condition in tagConditions)
    {
        condition.currentCount = 0;
        condition.isCompleted = false;
    }

    // 遍历所有格子
    for (int i = 0; i < gameBoard.size.x * gameBoard.size.y; i++)
    {
        GameTile tile = gameBoard.GetTileAt(i % gameBoard.size.x, i / gameBoard.size.x);
        if (tile == null || tile.Content == null)
        {
            continue;
        }

        Debug.Log($"Checking Tile at ({i % gameBoard.size.x}, {i / gameBoard.size.x}): Type={tile.Content.Type}");
        if (tile.Content.Type == GameTileContentType.Tool)
        {
            GamePuzzle puzzle = tile.Content.GetComponent<GamePuzzle>();
            if (puzzle == null)
            {
                Debug.LogWarning("Tile has no GamePuzzle component!");
                continue;
            }

            ItemParameters itemParams = puzzle.GetComponent<ItemParameters>();
            if (itemParams == null)
            {
                Debug.LogWarning("GamePuzzle has no ItemParameters component!");
                continue;
            }

            Debug.Log($"Found ItemParams: {itemParams.typeTag}");
            foreach (var condition in tagConditions)
            {
                if (itemParams.typeTag == condition.typeTag)
                {
                    condition.currentCount++;
                    Debug.Log($"Matched Tag: {condition.typeTag}, New Count: {condition.currentCount}");
                    if (condition.currentCount >= condition.requiredCount)
                    {
                        condition.isCompleted = true;
                    }
                }
            }
        }
    }
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