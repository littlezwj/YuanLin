using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameBoard))]
public class GameBoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameBoard board = (GameBoard)target;

        if (GUILayout.Button("生成 Tile 网格"))
        {
            int rows = board.size.x;
            int cols = board.size.y;

            board.transform.GetChild(0).localScale = new Vector3(rows, cols, 1);

            // 清除旧的 Tile（保留 index 0）
            for (int i = board.transform.childCount - 1; i >= 1; i--)
            {
                DestroyImmediate(board.transform.GetChild(i).gameObject);
            }

            for (int y = 0; y < cols; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    GameTile tile = (GameTile)PrefabUtility.InstantiatePrefab(board.tilePrefab, board.transform);
                    tile.transform.localPosition = new Vector3(x - rows * 0.5f + 0.5f, 0f, y - cols * 0.5f + 0.5f);
                }
            }

            Debug.Log("生成完成！");
        }
    }
}
