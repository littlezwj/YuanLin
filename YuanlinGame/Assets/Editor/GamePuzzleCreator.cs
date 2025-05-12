using UnityEditor;
using UnityEngine;

public class GamePuzzleCreator
{
    [MenuItem("GameObject/3D Object/Game Puzzle", false, 10)]
    public static void CreateGamePuzzle()
    {
        GameObject puzzleObject = new GameObject("GamePuzzle");
        puzzleObject.AddComponent<GamePuzzle>();

        // ���Ĭ��Mesh��Ϊ���ӻ��ο�����ѡ��
        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        visual.transform.SetParent(puzzleObject.transform, false);
        visual.transform.localScale = new Vector3(1f, 0.2f, 1f);
        Object.DestroyImmediate(visual.GetComponent<Collider>()); // ��ȥ��Ĭ����ײ��

        // ����Ĭ��λ���ڳ�������
        puzzleObject.transform.position = Vector3.zero;

        // ����Ϊ��ǰѡ������
        Selection.activeGameObject = puzzleObject;
    }
}

