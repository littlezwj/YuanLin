using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaceController : MonoBehaviour
{
    public static GamePlaceController Instance { get; private set; }
    public bool canDrag = false;
    private GamePuzzle selectedPuzzle = null;
    private Vector3 origin_puzzle_pos = Vector3.zero;
    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 防止重复单例
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 如果你希望它在切场景时保持存在
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canDrag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.GetComponent<GamePuzzle>())
                        selectedPuzzle = hit.collider.GetComponent<GamePuzzle>();                   
                }
                if (selectedPuzzle != null)
                {
                    selectedPuzzle.Select();
                    origin_puzzle_pos = selectedPuzzle.transform.position;
                    selectedPuzzle.RestoreOccupiedTile();
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (selectedPuzzle != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

                    if (groundPlane.Raycast(ray, out float enter))
                    {
                        selectedPuzzle.SnapToGrid(ray.GetPoint(enter));
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (selectedPuzzle != null)
                {
                    bool placed = selectedPuzzle.TryPlaceAt();
                    if (!placed)
                    {
                        selectedPuzzle.transform.position = origin_puzzle_pos;
                        selectedPuzzle.MarkTilesAsUsed();
                        selectedPuzzle.Deselect();
                    }
                }
                selectedPuzzle = null;
            }
        }
    }
}
