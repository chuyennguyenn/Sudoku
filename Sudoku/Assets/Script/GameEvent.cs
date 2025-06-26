using UnityEngine;

public class GameEvent: MonoBehaviour
{
    public delegate void UpdateSquareNumber(int number);
    public static event UpdateSquareNumber OnUpdateSquareNumber;

    public static void UpdateSquareNumberEvent(int number)
    {
        if (OnUpdateSquareNumber != null)
        {
            OnUpdateSquareNumber(number);
        }
    }

    public delegate void SquareSelected(int squareIndex);
    public static event SquareSelected OnSquareSelected;

    public static void SquareSelectedEvent(int squareIndex)
    {
        if (OnSquareSelected != null)
        {
            OnSquareSelected(squareIndex);
        }
    }

    public delegate void GetHint();
    public static event GetHint OnGetHint;

    public static void GetHintEvent()
    {
        if (OnGetHint != null)
        {
            OnGetHint();
        }
    }
}
