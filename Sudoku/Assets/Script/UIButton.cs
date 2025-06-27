
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    public GameObject sudokoBoard;
    public void loadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void CheckValid()
    {
        bool valid = sudokoBoard.GetComponent<SudokuGrid>().IsValidSudoku();
        if (valid)
        {
            SceneManager.LoadScene("Win");
        }
        else
        {
            Debug.Log("Sudoku is not valid!");
        }
    }
}
