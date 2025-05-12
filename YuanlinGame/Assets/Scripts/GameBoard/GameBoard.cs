using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    Transform ground = default;

    public Vector2Int size;

    public GameTile tilePrefab = default;

    GameTile[] tiles; //用array而非list来存储，因为tile一经生成就不会再被销毁或清除
    public float xOffset;
    public float zOffset;

    public void Initialized()
    {
        //this.size = size;
        tiles = new GameTile[size.x * size.y];
        ground.localScale = new Vector3(size.x, size.y, 1f);
        xOffset = 0;// size.x / 2;
        zOffset = 0;// size.y / 2;

        if (size.x % 2 == 0) xOffset -= 0.5f;
        if (size.y % 2 == 0) zOffset -= 0.5f;

        /*
        for (int index = 0, i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++,index++)
            {             
                GameTile tile = Instantiate(tilePrefab);
                tile.transform.SetParent(transform, false); //防止tile的transform值随着父级改变
                tile.transform.localPosition = new Vector3(i - size.x * 0.5f + 0.5f,transform.position.z + 0.001f, j - size.y * 0.5f + 0.5f);
                tiles[index] = tile;
            }
        }
        */

        for (int index = 0;index < size.x * size.y; index++){
            GameTile tile = transform.GetChild(index + 1).GetComponent<GameTile>();
            tiles[index] = tile;
        }

    }

    //检查瓷砖可用性
    public void CheckTileState(int num)
    {
        for (int t = 0; t < size.x * size.y; t++)
        {
            if (tiles[t] != null && tiles[t].State == GameTileState.Disable)
            {
                tiles[t].disableGrid();
            }
            if (tiles[t] != null && tiles[t].IsSpawnPoint == true)
            {
                tiles[t].SetAsSpawnPoint(num);
            }
        }
    }

    //检查瓷砖的各类内容物数量
    public int[] countNum()
    {
        int[] counts = { 0, 0, 0, 0 };
        for (int t = 0; t < size.x * size.y; t++)
        {
            if (tiles[t] != null && tiles[t].Content != null)
            {
                switch (tiles[t].Content.Type)
                {
                    case GameTileContentType.Empty: 
                        counts[0] += 1;
                        break;
                    case GameTileContentType.Destination:
                        counts[1] += 1;
                        break;
                    case GameTileContentType.Tool:
                        counts[2] += 1;
                        break;
                    case GameTileContentType.SpawnPoint:
                        counts[3] += 1;
                        break;
                }
            }
        }
        print("empty: " + counts[0]);
        print("des: " + counts[1]);
        print("tool: " + counts[2]);
        print("spp: " + counts[3]);
        return counts;
    }

    public GameTile GetTileAt(int x, int z)
    {
        int indexX = x;
        int indexZ = z;

        if (indexX >= 0 && indexX < size.x && indexZ >= 0 && indexZ < size.y)
        {
            return tiles[indexX * size.x + indexZ];
        }

        return null;
    }

    //触碰瓷砖
    public GameTile GetTile(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            int x = (int)(hit.point.z + size.y * 0.5f); //获取触碰点对应的瓷砖的编号
            int y = (int)(hit.point.x + size.x * 0.5f);            
            //判断是否出界
            if(x >= 0 && x < size.x && y >= 0 && y < size.y)
            {
                print(x + y * size.x);
                return tiles[x + y * size.x];
            }
            
        }
        return null;
    }

    //清除所有不为空的瓷砖的选择
    public void clearSelect()
    {
        for (int t = 0;t<size.x * size.y; t++)
        {
            if(tiles[t] != null)
            {
                tiles[t].restoreGrid();
            }
        }
    }

    private void Awake()
    {
        Initialized();
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
