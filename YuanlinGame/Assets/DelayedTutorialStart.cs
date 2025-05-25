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
        // ��ʼ�������岢����
        if (tutorialManager != null)
        {
            tutorialManager.enabled = false;
        }
        //StartCoroutine(TriggerEventsCoroutine());
        // 3�����ʾ������
        Invoke("ShowAndEnable", delay);
    }

    private void ShowAndEnable()
    {
        // ��ʾ���岢����
        //gameObject.SetActive(true);

        // ��� tutorialManager ��Ϊ�գ������̳�
        if (tutorialManager != null)
        {
            tutorialManager.enabled = true;
            tutorialManager.StartTutorial(tutorialManager.StartingTutorialIndex);
        }
    }
}