using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class GameTileContentFactory : ScriptableObject
{
	[SerializeField]
	GameTileContent destinationPrefab = default;

	[SerializeField]
	GameTileContent emptyPrefab = default;	

	[SerializeField]
	GameTileContent toolPrefab = default;
	
	[SerializeField]
	GameTileContent spawnPointPrefab = default;

	public GameTileContent Get(GameTileContentType type, int num)
	{
		switch (type)
		{
			case GameTileContentType.Destination:return Get(destinationPrefab, num);				
			case GameTileContentType.Empty:return Get(emptyPrefab, num);				
			case GameTileContentType.Tool:return Get(toolPrefab, num);			
			case GameTileContentType.SpawnPoint:return Get(spawnPointPrefab, num);
		}
		Debug.Assert(false, "Unsupported type: " + type);
		return null;
	}

	GameTileContent Get(GameTileContent prefab, int num)
	{
		if(num > 0)
        {
			GameTileContent instance = Instantiate(prefab);
			instance.OriginFactory = this;
			MoveToFactoryScene(instance.gameObject); //我真的不知道这个是干啥用的，，
			Debug.Log("Generate Prefab");
			return instance;
        }
        else
        {
			return null;
        }
		
		
	}

	Scene contentScene;
	public void Reclaim(GameTileContent content)
	{
		Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(content.gameObject);
	}	
	
	void MoveToFactoryScene(GameObject o)
	{
		if (!contentScene.isLoaded)
		{
			if (Application.isEditor)
			{
				contentScene = SceneManager.GetSceneByName(name);
				if (!contentScene.isLoaded)
				{
					contentScene = SceneManager.CreateScene(name);
				}
			}
			else
			{
				contentScene = SceneManager.CreateScene(name);
			}
		}
		SceneManager.MoveGameObjectToScene(o, contentScene);
	}
}
