using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShapeCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject shapePrefab;
    public int maxUses = 2;
    private int currentUses;

    private GameObject draggingInstance;
    private Canvas canvas;
    private GameBoard gameBoard;

    private bool canDrag = true;

    // 新增 UI 元素（通过 Inspector 赋值）
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI usesText;

    public Sprite cardIcon;
    public string cardName;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        gameBoard = FindObjectOfType<GameBoard>();
        currentUses = maxUses;

        // 告诉 GameBoard：我是那个 ShapeCard！
        if (gameBoard != null)
        {
            gameBoard.shapeCardRef = this;
        }

        UpdateUI(); // 显示容量等
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag || currentUses <= 0) return;
        if (GamePlaceController.Instance)
            GamePlaceController.Instance.canDrag = false;
        draggingInstance = Instantiate(shapePrefab);

        var puzzleInstance = draggingInstance.GetComponent<GamePuzzle>();
        if (puzzleInstance != null)
        {
            puzzleInstance.SetGameBoard(gameBoard);
            puzzleInstance.Select();
        }
    }

    
    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag || draggingInstance == null) return;

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //if (!draggingInstance.activeSelf)
            //    draggingInstance.SetActive(true);
            var puzzleInstance = draggingInstance.GetComponent<GamePuzzle>();
            if (puzzleInstance != null)
            {
                puzzleInstance.SnapToGrid(hit.point);
            }
        }
        //else
        //{
        //    draggingInstance.SetActive(false);
        //}
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag || draggingInstance == null) return;
        if (GamePlaceController.Instance)
            GamePlaceController.Instance.canDrag = true;
        var puzzleInstance = draggingInstance.GetComponent<GamePuzzle>();
        if (puzzleInstance == null) return;
        bool placed = puzzleInstance.TryPlaceAt();

        if (placed)
        {
            currentUses--;
            UpdateUI();

            if (currentUses <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(draggingInstance);         
        }
        draggingInstance = null;
    }

    private void UpdateUI()
    {
        if (iconImage != null) iconImage.sprite = cardIcon;
        if (nameText != null) nameText.text = cardName;
        if (usesText != null) usesText.text = "x" + currentUses.ToString();
    }

    public void RecoverOneUse()
    {
        if (currentUses <= 0)
        {
            gameObject.SetActive(true);
        }

        currentUses++;
        UpdateUI();
    }
}
