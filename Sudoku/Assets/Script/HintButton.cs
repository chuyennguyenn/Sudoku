using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class HintButton : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvent.GetHintEvent();
    }

    public void OnSubmit(BaseEventData eventData)
    {

    }

}
