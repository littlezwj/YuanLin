using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    //-------------状态-------------
    public GameTileState state = GameTileState.Available; //是否是敌人生成点和是否可用应该不冲突

    [SerializeField]
    bool isSpawnPoint = false;

    public bool IsSpawnPoint => isSpawnPoint;

    //-------------网格-------------
    [SerializeField]
    Transform grid = default;

    public Material[] materials;
    
    public void SelectGrid()
    {
        if (state == GameTileState.Available)
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
        else
        {
            DisableGrid();
        }
    }

    public void RestoreGrid()
    {
        this.state = GameTileState.Available;
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

    public void DisableGrid()
    {
        MeshRenderer gridRenderer = grid.gameObject.GetComponent<MeshRenderer>();
        if (materials[2] != null)
        {
            gridRenderer.material = materials[2];
        }
        else
        {
            Debug.Log("Warning: Miss Material,Please check for the mistake");
        }
    }

    public void OccupiedGrid()
    {
        this.state = GameTileState.Occupied;
        MeshRenderer gridRenderer = grid.gameObject.GetComponent<MeshRenderer>();
        if (materials[3] != null)
        {
            gridRenderer.material = materials[3];
        }
        else
        {
            Debug.Log("Warning: Miss Material,Please check for the mistake");
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
