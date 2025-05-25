using UnityEngine;
using HardCodeLab.TutorialMaster;

public class DelayedTutorialStart : MonoBehaviour
{
    public TutorialMasterManager tutorialManager;
    public float delay = 3f;

    private void Start()
    {
        // 初始隐藏物体并禁用
        gameObject.SetActive(false);

        // 3秒后显示并启用
        Invoke("ShowAndEnable", delay);
    }

    private void ShowAndEnable()
    {
        // 显示物体并启用
        gameObject.SetActive(true);

        // 如果 tutorialManager 不为空，启动教程
        if (tutorialManager != null)
        {
            tutorialManager.StartTutorial(tutorialManager.StartingTutorialIndex);
        }
    }
}