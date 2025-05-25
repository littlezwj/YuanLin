using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    Transform ground = default;

    public Vector2Int size;
    public ShapeCard shapeCardRef; // 当前场景中与该拼图绑定的卡牌

    public GameTile tilePrefab = default;

    GameTile[] tiles; //��array����list���洢����Ϊtileһ�����ɾͲ����ٱ����ٻ����
    public float xOffset;
    public float zOffset;

    public List<GamePuzzle> gamePuzzles = new List<GamePuzzle>();
    public void Initialized()
    {
        gamePuzzles.Clear();
        //this.size = size;
        tiles = new GameTile[size.x * size.y];
        ground.localScale = new Vector3(size.x + 3, size.y + 3, 1f);
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
                tile.transform.SetParent(transform, false); //��ֹtile��transformֵ���Ÿ����ı�
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

    public GameTile GetTileAt(int x, int z)
    {
        int indexX = x;
        int indexZ = z;

        if (indexX >= 0 && indexX < size.x && indexZ >= 0 && indexZ < size.y)
        {
            return tiles[indexZ * size.x + indexX];
        }

        return null;
    }

    //������ש
    public GameTile GetTile(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            int x = (int)(hit.point.z + size.y * 0.5f); //��ȡ�������Ӧ�Ĵ�ש�ı��
            int y = (int)(hit.point.x + size.x * 0.5f);            
            //�ж��Ƿ����
            if(x >= 0 && x < size.x && y >= 0 && y < size.y)
            {
                print(x + y * size.x);
                return tiles[x + y * size.x];
            }
            
        }
        return null;
    }

    //������в�Ϊ�յĴ�ש��ѡ��
    public void clearSelect()
    {
        for (int t = 0;t<size.x * size.y; t++)
        {
            if(tiles[t] != null)
            {
                tiles[t].RestoreGrid();
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
    public void OnPuzzleFailedToPlace(GamePuzzle puzzle)
    {
        // 回收实例
        if (shapeCardRef != null)
        {
            shapeCardRef.RecoverOneUse(); // 通知卡牌+1
        }

        Destroy(puzzle.gameObject); // 删除当前场上的拼图
    }
    // 传入想放置物体的世界位置 和 物体的初始位置
    // 返回bool是否允许放置，如果不允许，调用方自己复位物体位置
    /*
    public bool TryPlaceObjectAt(Vector3 placePosition, Transform objectTransform, Vector3 originalPosition)
    {
        Ray ray = new Ray(placePosition + Vector3.up * 10f, Vector3.down);
        GameTile tile = GetTile(ray);

        if (tile == null)
        {
            return false; // 不在任何格子，放置失败
        }

        if (tile.Content != null && tile.Content.Type != GameTileContentType.Empty)
        {
            return false; // 格子被占用，放置失败
        }

        // 放置成功，设置物体位置和格子内容
        objectTransform.position = tile.transform.position + Vector3.up * 0.1f;

        // 确保物体有 GamePuzzle 组件
        GamePuzzle puzzle = objectTransform.GetComponent<GamePuzzle>();
        if (puzzle != null)
        {
            tile.Content = puzzle; // 将 GamePuzzle 设置为格子的 Content
            tile.Content.Type = GameTileContentType.Tool; // 设置类型为 Tool
        }

        // 触发条件检测
        FindObjectOfType<LevelConditionChecker>()?.UpdateConditions();
        return true;
    }
    */
}
