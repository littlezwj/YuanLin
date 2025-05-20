using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    GameTileContentType type = default;

    //����ԭʼ�������ù���Ӧ��ֻ����һ�Σ�����һ���������������ͻع���
    public GameTileContentType Type { get; set; } // 将 set 设为公开

    GameTileContentFactory originFactory; //ΪGameTileContent����һ�����������������ͼӹ�����״�ȵ�
    public GameTileContentFactory OriginFactory
    {
        get => originFactory; //��ֵָ��originFactory
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }

    public void Recycle()
    {
        originFactory.Reclaim(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
