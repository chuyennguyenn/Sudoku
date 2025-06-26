using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices.WindowsRuntime;

public class SquareGrid : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public GameObject number_text; 
    private int number = 0; 

    public bool visible = true;
    public bool isPuzzle = false; 
    
    public int row;
    public int col;
    public SudokuGrid parentGrid;

    private bool selected = false;
    private int squareIndex = 0; 

    public bool IsSelected()
    {
        return selected;
    }

    public void setSquareIndex(int index)
    {
        squareIndex = index;    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        /*
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!visible && parentGrid != null)
            {
                parentGrid.RevealNumber(row, col);
            }
            else if (visible)
            {
                Debug.Log("Square clicked: " + number);
            }
            else Debug.Log("Hidden");
        }
        */

        selected = true;
        Debug.Log("Square clicked: " + number + " at index: " + squareIndex);
        GameEvent.SquareSelectedEvent(squareIndex);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        
    }

    private void OnEnable()
    {
        GameEvent.OnUpdateSquareNumber += OnSetNumber;
        GameEvent.OnSquareSelected += SquareSelected;
        GameEvent.OnGetHint += GetHint;
    }

    private void OnDisable()
    {
        GameEvent.OnSquareSelected -= OnSetNumber;
        GameEvent.OnSquareSelected -= SquareSelected;
        GameEvent.OnGetHint -= GetHint;
    }

    public void DisplayText()
    {
        if (number <= 0)
        {
            number_text.GetComponent<TextMeshProUGUI>().text = " ";
        }
        else
        {
            number_text.GetComponent<TextMeshProUGUI>().text = number.ToString();
        }
    }

    public void SetNumber(int num)
    {
        number = num;
        parentGrid.GetComponent<SudokuGrid>().displayeBoard[row, col] = num;
        ColorBlock colorBlock = this.colors;
        if (colorBlock.normalColor != Color.white)
        {
            parentGrid.GetComponent<SudokuGrid>().setColorWhite(squareIndex);
        }
        DisplayText();
    }

    public void SetVisible(bool isVisible)
    {
        visible = isVisible;
        DisplayText();
    }

    public void OnSetNumber(int num)
    {
        if (selected && isPuzzle)
        {
            SetNumber(num);
        }
    }
    public void SquareSelected(int squareInd)
    {
        if (squareIndex != squareInd)
        {
            selected = false;
        }
    }

    private void GetHint()
    {
        if (selected && isPuzzle)
        {
            parentGrid.GetComponent<SudokuGrid>().GiveHint(row, col);
        }
    }
}
