using UnityEditor;
using UnityEngine;

public class GamePuzzleCreator
{
    [MenuItem("GameObject/3D Object/Game Puzzle", false, 10)]
    public static void CreateGamePuzzle()
    {
        GameObject puzzleObject = new GameObject("GamePuzzle");
        puzzleObject.AddComponent<GamePuzzle>();

        // 添加默认Mesh作为可视化参考（可选）
        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        visual.transform.SetParent(puzzleObject.transform, false);
        visual.transform.localScale = new Vector3(1f, 0.2f, 1f);
        Object.DestroyImmediate(visual.GetComponent<Collider>()); // 可去掉默认碰撞体

        // 设置默认位置在场景中央
        puzzleObject.transform.position = Vector3.zero;

        // 设置为当前选中物体
        Selection.activeGameObject = puzzleObject;
    }
}

