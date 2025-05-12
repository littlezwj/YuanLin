using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    GameTileContentType type = default;

    //跟踪原始工厂，该工厂应该只设置一次，并以一个方法将自身发送回工厂
    public GameTileContentType Type => type;

    GameTileContentFactory originFactory; //为GameTileContent创建一个工厂，根据其类型加工其形状等等
    public GameTileContentFactory OriginFactory
    {
        get => originFactory; //将值指向originFactory
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
