using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField]
    Vector2Int boardsize = new Vector2Int(12, 12);

    [SerializeField]
    GameBoard board = default;

    [SerializeField]
    int destinationNum = 1;
    [SerializeField]
    int emptyNum = 1;
    [SerializeField]
    int toolNum = 1;
    [SerializeField]
    int spawnPointNum = 1;

    int[] res = { 0, 0, 0, 0 };

    GameTile targetTile = null; //�ȴ���������Ʒ��tile
    private void Awake()
    {
        //board.Initialized(boardsize);
        board.CheckTileState(spawnPointNum);
    }

    private void OnValidate()
    {
        if(boardsize.x < 2)
        {
            boardsize.x = 2;
        }
        if(boardsize.y < 2)
        {
            boardsize.y = 2;
        }
    }
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition); //������Ľ�ƽ�淢��һ�����ߵ���Ļλ��
    GameTile HandleTouch()
    {
        GameTile tile = board.GetTile(TouchRay); //��ȡ���ߴ������Ĵ�ש
        if (tile != null)
        {
            tile.selectGrid(); //���Ĳ���
            return tile;
            //tile.Content = tileContentFactory.Get(GameTileContentType.Destination);
        }
        else
        {
            Debug.Log("Warnning: Your Tile is Missing or Disable!Check for it!");
        }
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //��������ζ�Ŷ���һ�����ӵĲ������������Խ���������
        {
            res = updateNum();
            print("empty: " + res[0]);
            print("des: " + res[1]);
            print("tool: " + res[2]);
            print("spp: " + res[3]);
            board.clearSelect(); //���²���
            print("mouse down");
            targetTile = HandleTouch();
        }
        if(targetTile != null)
        {           
            targetTile.PlaceContent(res);
        }
    }

    int[] updateNum()
    {
        int[] countNum = board.countNum();
        int[] result = { emptyNum - countNum[0], destinationNum - countNum[1], toolNum - countNum[2], spawnPointNum - countNum[3] };
        return result;
    }
}
