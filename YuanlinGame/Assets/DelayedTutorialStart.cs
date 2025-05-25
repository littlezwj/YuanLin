using UnityEngine;
using HardCodeLab.TutorialMaster;

public class DelayedTutorialStart : MonoBehaviour
{
    public TutorialMasterManager tutorialManager;
    public float delay = 3f;

    private void Start()
    {
        // ��ʼ�������岢����
        gameObject.SetActive(false);

        // 3�����ʾ������
        Invoke("ShowAndEnable", delay);
    }

    private void ShowAndEnable()
    {
        // ��ʾ���岢����
        gameObject.SetActive(true);

        // ��� tutorialManager ��Ϊ�գ������̳�
        if (tutorialManager != null)
        {
            tutorialManager.StartTutorial(tutorialManager.StartingTutorialIndex);
        }
    }
}