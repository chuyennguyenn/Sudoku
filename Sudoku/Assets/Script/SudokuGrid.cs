using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class SudokuGrid : MonoBehaviour
{
    private int columns = 9; 
    private int rows = 9; 
    public float squareOffset = 0.0f; // gap between squares
    public GameObject squarePrefab; 
    public Vector2 start_pos = new Vector2(0.0f, 0.0f);
    public float squareSize = 1.0f; // size of each square

    public List<GameObject> squares = new List<GameObject>(); 

    public int[,] sudokuBoard;
    public int[,] displayeBoard; //display to player

    public SquareGrid squareGrid; 

    public int cellsToRemove = 40;

    private void Start()
    {
        if (squarePrefab.GetComponent<SquareGrid>() == null)
        {
            Debug.LogError("Square Prefab is not assigned!");
        }

        CreateGrid();
        GenerateSudokuBoard();
        MakePuzzle();
        AssignBoardToGrid();
    }
    private void Update()
    {

    }

    public void CreateGrid()
    {
        SpawnGrid();
        SetSquarePosition();
    }

    public void SpawnGrid()
    {
        int squareIndex = 0;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                var squareObj = Instantiate(squarePrefab) as GameObject;
                squareObj.transform.parent = this.transform;
                squareObj.transform.localScale = new Vector3(squareSize, squareSize, squareSize);

                var squareGrid = squareObj.GetComponent<SquareGrid>();
                squareGrid.row = i;
                squareGrid.col = j;
                squareGrid.parentGrid = this;
                squareGrid.setSquareIndex(squareIndex);

                squares.Add(squareObj);
                squareIndex++;
            }
        }
    }

    private void SetSquarePosition()
    {
        var squareGrid = squares[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        offset.x = squareGrid.rect.width * squareGrid.transform.localScale.x + squareOffset;
        offset.y = squareGrid.rect.height * squareGrid.transform.localScale.y + squareOffset;

        int columnIndex = 0;
        int rowIndex = 0;

        foreach (GameObject grind_square in squares)
        {
            if (columnIndex + 1 > columns)
            {
                columnIndex = 0;
                rowIndex++;
            }

            var pos_x_offset = offset.x * columnIndex;
            var pos_y_offset = offset.y * rowIndex;
            grind_square.GetComponent<RectTransform>().anchoredPosition = new Vector2(start_pos.x + pos_x_offset, start_pos.y - pos_y_offset);
            columnIndex++;
        }
    }

    /*
    public void SetGridNumber()
    {
        foreach (var grind_square in squares)
        {
            grind_square.GetComponent<SquareGrid>().SetNumber(UnityEngine.Random.Range(0, 10));
        }
    }
    */

    private void GenerateSudokuBoard()
    {
        sudokuBoard = new int[rows, columns];
        displayeBoard = new int[rows, columns]; 
        FillSudokuBoard(sudokuBoard, displayeBoard, 0, 0);
    }

    private bool FillSudokuBoard(int[,] board, int[,] display, int row, int col)
    {
        if (row == rows)
            return true;
        if (col == columns)
            return FillSudokuBoard(board, display, row + 1, 0);

        List<int> numbers = new List<int>();
        for (int n = 1; n <= 9; n++) numbers.Add(n);
        Shuffle(numbers);

        foreach (int num in numbers)
        {
            if (IsSafe(board, row, col, num))
            {
                board[row, col] = num;
                display[row, col] = num; 
                if (FillSudokuBoard(board, display, row, col + 1))
                    return true;
                board[row, col] = 0;
                display[row, col] = 0; 
            }
        }
        return false;
    }

    private bool IsSafe(int[,] board, int row, int col, int num)
    {

        for (int i = 0; i < 9; i++)
        {
            if (board[row, i] == num || board[i, col] == num)
                return false;
        }

        int startRow = row - row % 3, startCol = col - col % 3;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[startRow + i, startCol + j] == num)
                    return false;
        return true;
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void MakePuzzle() //remove randomly
    {
        int removed = 0;
        System.Random rand = new System.Random();
        while (removed < cellsToRemove)
        {
            int i = rand.Next(0, rows);
            int j = rand.Next(0, columns);
            if (displayeBoard[i, j] != 0)
            {
                displayeBoard[i, j] = 0;
                int index = i * columns + j;
                squares[index].GetComponent<SquareGrid>().visible = false;
                squares[index].GetComponent<SquareGrid>().isPuzzle = true;
                removed++;
            }
        }
    }

    private void AssignBoardToGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int index = i * columns + j;
                if (index < squares.Count)
                {
                    squares[index].GetComponent<SquareGrid>().SetNumber(displayeBoard[i, j]);
                    setColorWhite(index);
                }
            }
        }
    }

    public void RevealNumber(int row, int col)
    {
        if (displayeBoard[row, col] == 0)
        {
            displayeBoard[row, col] = sudokuBoard[row, col];
            int index = row * columns + col;
            var square = squares[index].GetComponent<SquareGrid>();
            square.SetNumber(sudokuBoard[row, col]);
            square.SetVisible(true);
        }
    }

    public bool IsValidSudoku()
    {
        int count = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (displayeBoard[i, j] != sudokuBoard[i, j])
                {
                    count++;
                    int index = i * columns + j;
                    var square = squares[index].GetComponent<SquareGrid>();
                    if (square.isPuzzle)
                    {
                        ColorBlock cb = square.colors;
                        cb.normalColor = Color.red;
                        square.colors = cb;
                        StartCoroutine(WaitFor(2f, index));
                    }
                }
                else
                {
                    int index = i * columns + j;
                    var square = squares[index].GetComponent<SquareGrid>();
                    if (square.isPuzzle)
                    {
                        ColorBlock cb = square.colors;
                        cb.normalColor = Color.green;
                        square.colors = cb;
                        StartCoroutine(WaitFor(2f, index));
                    }
                }
            }
        }
        if (count > 0)
        {
            return false;
        }
        return true;
    }

    public void setColorWhite(int index)
    {
        var square = squares[index].GetComponent<SquareGrid>();
        ColorBlock cb = square.colors;
        cb.normalColor = Color.white;
        square.colors = cb;
    }

    public void GiveHint(int rowIndex, int colIndex)
    {
        int index = rowIndex * columns + colIndex;
        var square = squares[index].GetComponent<SquareGrid>();
        square.SetNumber(sudokuBoard[rowIndex, colIndex]);
        ColorBlock cb = square.colors;
        cb.normalColor = Color.blue;
        square.colors = cb;
    }

    private IEnumerator WaitFor(float seconds, int index)
    {
        yield return new WaitForSeconds(seconds);
        setColorWhite(index);
    }

    
}
