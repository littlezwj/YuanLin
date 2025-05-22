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
    public int currentUses;

    private GameObject draggingInstance;
    private Canvas canvas;
    private GameBoard gameBoard;

    private bool canDrag = true;

    // ???? UI ??????? Inspector ?????
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI usesText;
    public TextMeshProUGUI hiddenValueText;
    public TextMeshProUGUI elegantValueText;
    public TextMeshProUGUI agileValueText;
    public TextMeshProUGUI zenValueText;

    public TextMeshProUGUI costText; 

    


    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        gameBoard = FindObjectOfType<GameBoard>();
        currentUses = maxUses;

        // ???? GameBoard????????? ShapeCard??
        if (gameBoard != null)
        {
            gameBoard.shapeCardRef = this;
        }

        UpdateUI(); // ?????????
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
            puzzleInstance.m_card = this;
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

    public void UpdateUI()
    {
        if (usesText != null) usesText.text = currentUses.ToString();

        // 更新 ItemParameters 的值和图片
        if (shapePrefab != null)
        {
            var itemParams = shapePrefab.GetComponent<ItemParameters>();
            if (itemParams != null)
            {
                if (nameText != null) nameText.text = itemParams.typeTag;
                if (hiddenValueText != null) hiddenValueText.text = ((int)itemParams.hiddenValue).ToString();
                if (elegantValueText != null) elegantValueText.text = ((int)itemParams.elegantValue).ToString();
                if (agileValueText != null) agileValueText.text = ((int)itemParams.agileValue).ToString();
                if (zenValueText != null) zenValueText.text = ((int)itemParams.zenValue).ToString();
                if (costText != null) costText.text = itemParams.cost.ToString(); // 更新成本价格文本
                
                // 同步 ItemParameters 中的 image 到 iconImage
                if (iconImage != null && itemParams.itemSprite != null)
                {
                    iconImage.sprite = itemParams.itemSprite;
                }
            }
        }
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
