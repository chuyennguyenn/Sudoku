using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NumberButton : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public int value;

    public SudokuGrid parentGrid;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvent.UpdateSquareNumberEvent(value);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        
    }
}
