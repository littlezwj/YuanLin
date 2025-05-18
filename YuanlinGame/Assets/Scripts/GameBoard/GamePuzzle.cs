using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GamePuzzle : MonoBehaviour
{
    [SerializeField]
    Vector2Int[] shapeOffsets; // 形状偏移格子，比如L形2x2等

    // [SerializeField]
    // GameBoard gameBoard;

    private List<GameTile> occupiedTiles = new List<GameTile>();
    private List<GameTile> previewTiles = new List<GameTile>();

    private bool isDragging = false;
    private Camera mainCam;

    // --- 新增，管理当前选中的实例 ---
    private static GamePuzzle currentSelected = null;
    private bool isSelected = false;
    [SerializeField]
    public GameBoard gameBoard;

    public void SetGameBoard(GameBoard board)
    {
        gameBoard = board;
    }


    private void Awake()
    {
        mainCam = Camera.main;

        // 确保shapeOffsets独立，不引用外部传入的数组（若是外部SetShape调用）
        if(shapeOffsets != null)
            SetShape(shapeOffsets);
    }

    void Update()
    {
        if (!isSelected) return; // 只有选中物体才响应输入

        HandleRotation();
        HandleDragging();
    }

    void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -90f);
            UpdateShapeRotation(-90);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(Vector3.up, 90f);
            UpdateShapeRotation(90);
        }
    }

    void HandleDragging()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 snappedPos = SnapToGrid(hit.point);
                transform.position = new Vector3(snappedPos.x, transform.position.y, snappedPos.z);
                HighlightPreviewTiles(snappedPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            ClearPreviewTiles();
            TryPlaceAt(transform.position);
        }
    }

    // 吸附到格子逻辑
    Vector3 SnapToGrid(Vector3 hitPoint)
    {
        Vector3 boardOrigin = gameBoard.transform.position;
        int x = Mathf.RoundToInt(hitPoint.x - boardOrigin.x);
        int z = Mathf.RoundToInt(hitPoint.z - boardOrigin.z);
        return new Vector3(boardOrigin.x + x - gameBoard.xOffset, boardOrigin.y, boardOrigin.z + z - gameBoard.zOffset);
    }

    void HighlightPreviewTiles(Vector3 snappedPos)
    {
        ClearPreviewTiles();

        Vector3 boardOrigin = gameBoard.transform.position;
        int halfX = gameBoard.size.x / 2;
        int halfZ = gameBoard.size.y / 2;

        int cx = Mathf.RoundToInt(snappedPos.x - boardOrigin.x + halfX - 0.5f);
        int cz = Mathf.RoundToInt(snappedPos.z - boardOrigin.z + halfZ - 0.5f);

        foreach (var offset in shapeOffsets)
        {
            int x = cx + offset.x;
            int z = cz + offset.y;
            GameTile tile = gameBoard.GetTileAt(x, z);
            if (tile != null)
            {
                tile.selectGrid(); // 高亮
                previewTiles.Add(tile);
            }
        }
    }

    void ClearPreviewTiles()
    {
        foreach (var tile in previewTiles)
        {
            if (tile != null)
                tile.restoreGrid(); // 取消高亮
        }
        previewTiles.Clear();
    }


    public bool TryPlaceAt(Vector3 worldPosition)
    {
        Vector3 boardOrigin = gameBoard.transform.position;
        int halfX = gameBoard.size.x / 2;
        int halfZ = gameBoard.size.y / 2;

        // 根据中心点和偏移计算目标格子
        int cx = Mathf.RoundToInt(worldPosition.x - boardOrigin.x);
        int cz = Mathf.RoundToInt(worldPosition.z - boardOrigin.z);

        List<GameTile> targetTiles = new List<GameTile>();
        foreach (var offset in shapeOffsets)
        {
            int x = cx + offset.x;
            int z = cz + offset.y;
            GameTile tile = gameBoard.GetTileAt(x, z);
            if (tile == null || tile.State == GameTileState.Disable)
            {
                return false;
            }
            targetTiles.Add(tile);
        }

        // 吸附到目标位置
        float worldX = boardOrigin.x + cx - gameBoard.xOffset;
        float worldZ = boardOrigin.z + cz - gameBoard.zOffset;
        transform.position = new Vector3(worldX, transform.position.y, worldZ);

        occupiedTiles = targetTiles;
        MarkTilesAsUsed();
        return true;
    }

    private void MarkTilesAsUsed()
    {
        foreach (GameTile tile in occupiedTiles)
        {
            tile.disableGrid();
        }
    }

    void UpdateShapeRotation(float angle)
    {
        for (int i = 0; i < shapeOffsets.Length; i++)
        {
            Vector2Int old = shapeOffsets[i];
            shapeOffsets[i] = RotateOffset(old, angle);
        }
    }

    Vector2Int RotateOffset(Vector2Int offset, float angle)
    {
        if (angle == -90)
            return new Vector2Int(-offset.y, offset.x);
        else if (angle == 90)
            return new Vector2Int(offset.y, -offset.x);
        return offset;
    }

    // 深拷贝shapeOffsets，避免多个物体共享引用
    public void SetShape(Vector2Int[] newShape)
    {
        shapeOffsets = new Vector2Int[newShape.Length];
        for (int i = 0; i < newShape.Length; i++)
        {
            shapeOffsets[i] = newShape[i];
        }
    }

    // -------- 新增鼠标点击选中逻辑 --------
    private void OnMouseDown()
    {
        // 取消之前选中物体
        if (currentSelected != null && currentSelected != this)
        {
            currentSelected.Deselect();
        }

        Select();
    }

    public void Select()
{
    if (currentSelected != null && currentSelected != this)
    {
        currentSelected.Deselect();
    }
    isSelected = true;
    currentSelected = this;
    // 选中高亮逻辑
}


    void Deselect()
    {
        isSelected = false;
        ClearPreviewTiles();
        // 取消选中高亮
    }


    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Color fillColor = new Color(1f, 1f, 0f, 0.25f); // 半透明黄色
        Color borderColor = Color.yellow;

        Vector3 centerPos = transform.position;

        for (int i = 0; i < shapeOffsets.Length; i++)
        {
            Vector2Int offset = shapeOffsets[i];
            Vector3 tilePos = centerPos + new Vector3(offset.x, 0f, offset.y);

            // ---- 填充颜色 ----
            Gizmos.color = fillColor;
            Gizmos.DrawCube(tilePos + Vector3.up * 0.01f, new Vector3(1f, 0.02f, 1f));

            // ---- 画边框 ----
            Gizmos.color = borderColor;
            Vector3 p1 = tilePos + new Vector3(-0.5f, 0.01f, -0.5f);
            Vector3 p2 = tilePos + new Vector3(0.5f, 0.01f, -0.5f);
            Vector3 p3 = tilePos + new Vector3(0.5f, 0.01f, 0.5f);
            Vector3 p4 = tilePos + new Vector3(-0.5f, 0.01f, 0.5f);
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);

            // ---- 文字标签 ----
            Handles.color = Color.white;
            Handles.Label(tilePos + Vector3.up * 0.2f, $"#{i}: ({offset.x},{offset.y})");
        }
#endif
    }

} 