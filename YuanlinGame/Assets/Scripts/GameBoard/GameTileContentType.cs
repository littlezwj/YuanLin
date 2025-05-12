using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileContentType
{
    Empty, //空
    Destination, //塔
    Tool, //道具
    SpawnPoint //敌人生成点 在编辑模式下就规定好，在游戏开始后再开始生成
}
