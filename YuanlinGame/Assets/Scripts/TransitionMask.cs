using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionMask : MonoBehaviour
{
    public bool auto_start = true;
    [SerializeField]
    float interval = 1;
    Material m_mat;
    // 参数
    float timer = 0;
    // Start is called before the first frame update
    void Awake()
    {
        m_mat = GetComponent<Image>().material;
        SetMatParams(1);
        if (auto_start) 
        { 
            timer = -1f;
            StartCoroutine(RemoveMask());
        }
        else 
        { 
            SetMatParams(0);
        }
    }

    private void SetMatParams(float value)
    {
        m_mat.SetFloat("_TransitionPara", value);
    }

    public void AddMaskOn()
    {
        Debug.Log("进入遮罩");
        StartCoroutine(AddMask());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 0--1
    /// </summary>
    /// <returns></returns>
    private IEnumerator AddMask()
    {
        while (timer <= interval)
        {
            timer += Time.deltaTime;
            SetMatParams(- timer / interval);
            yield return null;
        }
        SetMatParams(-1);
        timer = 0;
    }
    /// <summary>
    /// 1-0
    /// </summary>
    /// <returns></returns>
    private IEnumerator RemoveMask()
    {
        while(timer <= interval)
        {
            timer += Time.deltaTime;
            SetMatParams(1 - Mathf.Max(0, timer / interval));
            yield return null;
        }
        SetMatParams(0);
        timer = 0;
    }
}
