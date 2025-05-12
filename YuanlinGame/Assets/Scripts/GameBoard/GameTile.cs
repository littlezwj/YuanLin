using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    //-------------状态-------------
    [SerializeField]
    GameTileState state = GameTileState.Available; //是否是敌人生成点和是否可用应该不冲突
    bool available = true;

    [SerializeField]
    bool isSpawnPoint = false;

    public bool IsSpawnPoint => isSpawnPoint;

    public GameTileState State => state;

    //-------------网格-------------
    [SerializeField]
    Transform grid = default;

    public Material[] materials;
    
    public void selectGrid()
    {
        if (available)
        {
            MeshRenderer gridRenderer = grid.gameObject.GetComponent<MeshRenderer>();
            if(materials[1] != null)
            {
                gridRenderer.material = materials[1];
            }
            else
            {   
                Debug.Log("Warning: Miss Material,Please check for the mistake");
            }
        }     
    }

    public void restoreGrid()
    {
        if (available) 
        { 
            MeshRenderer gridRenderer = grid.gameObject.GetComponent<MeshRenderer>();
            if (materials[0] != null)
            {
                gridRenderer.material = materials[0];
            }
            else
            {
                Debug.Log("Warning: Miss Material,Please check for the mistake");
            }
        }
        
    }

    public void disableGrid()
    {
        MeshRenderer gridRenderer = grid.gameObject.GetComponent<MeshRenderer>();
        if (materials[2] != null)
        {
            gridRenderer.material = materials[2];
            available = false;
        }
        else
        {
            Debug.Log("Warning: Miss Material,Please check for the mistake");
        }
    }

    //-------------内容物-------------
    GameTileContent content; //内容物也是瓷砖的属性之一

    [SerializeField]
    GameTileContentFactory tileContentFactory = default;

    public GameTileContent Content
    {
        get => content; //content本来是私有变量 这样写可以用GameTile.Content的方式访问到它
        set
        {
            if (content != null)
            {
                content.Recycle();
            }
            Debug.Assert(value != null, "Null assigned to content!");
            content = value;
            content.transform.localPosition = transform.localPosition;
        }
    }

    public void SetAsSpawnPoint(int num)
    {
        Content = tileContentFactory.Get(GameTileContentType.SpawnPoint, num);
    }

    public void PlaceContent(int[] nums)
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Content = tileContentFactory.Get(GameTileContentType.Destination, nums[1]);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            Content = tileContentFactory.Get(GameTileContentType.Tool, nums[2]);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            Content = tileContentFactory.Get(GameTileContentType.Empty, nums[0]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
