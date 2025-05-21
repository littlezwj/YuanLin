using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;//��ȡ���������Camera���
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
        }
    }
}
