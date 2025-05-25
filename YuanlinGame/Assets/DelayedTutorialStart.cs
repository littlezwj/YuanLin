using UnityEngine;
using HardCodeLab.TutorialMaster;
using System.Collections;
using System.Collections.Generic;

public class DelayedTutorialStart : MonoBehaviour
{
    private TutorialMasterManager tutorialManager;
    public float delay = 3f;

    private void Awake()
    {
        tutorialManager = transform.GetComponent<TutorialMasterManager>();
        // 初始隐藏物体并禁用
        if (tutorialManager != null)
        {
            tutorialManager.enabled = false;
        }
        //StartCoroutine(TriggerEventsCoroutine());
        // 3秒后显示并启用
        Invoke("ShowAndEnable", delay);
    }

    private void ShowAndEnable()
    {
        // 显示物体并启用
        //gameObject.SetActive(true);

        // 如果 tutorialManager 不为空，启动教程
        if (tutorialManager != null)
        {
            tutorialManager.enabled = true;
            tutorialManager.StartTutorial(tutorialManager.StartingTutorialIndex);
        }
    }
}