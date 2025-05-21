using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemParameters : MonoBehaviour
{
    [Tooltip("输入物件的类型Tag（如“水池”、“窗户”）")]
    public string typeTag; // 类型Tag，支持中文输入
    [Header("图片参数")]
    public Sprite itemSprite; // 直接拖拽 PNG 到这里

    [Header("数值参数")]
    [Tooltip("隐逸值")]
    public float hiddenValue; // 隐逸
    [Tooltip("清雅值")]
    public float elegantValue; // 清雅
    [Tooltip("灵动值")]
    public float agileValue;   // 灵动
    [Tooltip("禅意值")]
    public float zenValue;     // 禅意

    // 如果需要初始化逻辑，可以在这里添加
    void Start()
    {
        // 初始化代码（可选）
    }

    // 如果需要每帧更新逻辑，可以在这里添加
    void Update()
    {
        // 更新代码（可选）
    }
}