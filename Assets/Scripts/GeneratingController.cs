using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratingController : MonoBehaviour
{
    public GameObject cellPrefabBlack;
    public GameObject cellPrefabWhite;
    public GameObject parentObject;

    private int iterationTotal;
    private int cellInRowTotal;
    private int type;
    private int[] ruleset;

    private int[,] states;
    private void OnEnable()
    {
        //Set parameters of generating image
        iterationTotal = PlayerPrefs.GetInt("iteration");
        cellInRowTotal = PlayerPrefs.GetInt("cell");
        type = PlayerPrefs.GetInt("type");

        //Change to binary- initate ruleset
        ruleset = ChangeToBinary();

        //Set cell size by dividing
        SetCellSizeAndCanvas();

        //Initate state array of ints with default 0
        //1 means high state (green), 0 means low signal (white)
        states = new int[iterationTotal, cellInRowTotal];

        //Set first living cell
        InitiateLife();

        //Set proper values of state array
        FillStateArray();

        //Initiate cells with colors
        InitiateCells();
    }

    private void FillStateArray()
    {
        for (int i = 0; i < iterationTotal-1; i++)
        {
            for (int j = 0; j < cellInRowTotal; j++)
            {
                //When it's on border (for example on right upper corner) it's needed change method
                //Next node is first one of this iteration
                //Analogical, when is on start, it's needed to get last cell as previous node
                if (IsStart(j))
                {
                    //First cell
                    states[i + 1, j] = BornChild(states[i, cellInRowTotal - 1], states[i, j], states[i, j+1]);
                }
                else if (IsEnd(j))
                {
                    //Last cell
                    states[i + 1, j] = BornChild(states[i, j - 1], states[i, j], states[i, 0]);
                }
                else
                {
                    //Normal filling
                    //Set 1 value on the middle of three, one line lower
                    states[i+1, j] = BornChild(states[i,j-1], states[i,j], states[i,j+1]);
                }
            }
        }
    }

    private int BornChild(int previous, int me, int next)
    {
        if (previous == 1 && me == 1 && next == 1) // 1 1 1
        {
            return ruleset[0];
        }
        if (previous == 1 && me == 1 && next == 0) // 1 1 0
        {
            return ruleset[1];
        }
        if (previous == 1 && me == 0 && next == 1) // 1 0 1
        {
            return ruleset[2];
        }
        if (previous == 1 && me == 0 && next == 0) // 1 0 0
        {
            return ruleset[3];
        }
        if (previous == 0 && me == 1 && next == 1) // 0 1 1
        {
            return ruleset[4];
        }
        if (previous == 0 && me == 1 && next == 0) // 0 1 0
        {
            return ruleset[5];
        }
        if (previous == 0 && me == 0 && next == 1) // 0 0 1
        {
            return ruleset[6];
        }
        if (previous == 0 && me == 0 && next == 0) // 0 0 0
        {
            return ruleset[7];
        }
        Debug.LogError("Child born in wrong way!");
        return 9;

    }

    private bool IsStart(int i)
    {
        return i == 0;
    }

    private bool IsEnd(int i)
    {
        return i == cellInRowTotal-1;
    }

    private void InitiateCells()
    {
        for (int i = 0; i < iterationTotal; i++)
        {
            for (int j = 0; j < cellInRowTotal; j++)
            {
                if (states[i, j] == 1)
                {
                    Instantiate(cellPrefabBlack, parentObject.transform);
                }
                else
                {
                    Instantiate(cellPrefabWhite, parentObject.transform);
                }
            }
        }
    }

    private void InitiateLife()
    {
        int firstIndex = cellInRowTotal / 2;
        states[0, firstIndex - 1] = 1;
    }

    private int[] ChangeToBinary()
    {
        switch (type)
        {
            case 30:
                {
                    return new int[] { 0, 0, 0, 1, 1, 1, 1, 0 };
                }
            case 60:
                {
                    return new int[] { 0, 0, 1, 1, 1, 1, 0, 0 };
                }
            case 90:
                {
                    return new int[] { 0, 1, 0, 1, 1, 0, 1, 0 };
                }
            case 120:
                {
                    return new int[] { 0, 1, 1, 1, 1, 0, 0, 0 };
                }
            case 225:
                {
                    return new int[] { 1, 1, 1, 0, 0, 0, 0, 1 };
                }
            case 150:
                {
                    return new int[] { 1, 0, 0, 1, 0, 1, 1, 0 };
                }
            case 122:
                {
                    return new int[] { 0, 1, 1, 1, 1, 0, 1, 0 };
                }
            case 182:
                {
                    return new int[] { 1, 0, 1, 1, 0, 1, 1, 0 };
                }
            default:
                {
                    Debug.LogError("Error! Default switch case!");
                    return new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };     
                }
        }
    }

    private void SetCellSizeAndCanvas()
    {
        float width= parentObject.GetComponent<RectTransform>().sizeDelta.x;
        float height= parentObject.GetComponent<RectTransform>().sizeDelta.y;
        float cellX = width / cellInRowTotal;
        float cellY = height / iterationTotal;
        float squareCellSide;
        int multiplier;

        //Cells need to be a squares with lower values (to fill canvas)
        if (cellX < cellY)
        {
            squareCellSide = cellX;
            multiplier = cellInRowTotal;
        }
        else
        {
            squareCellSide = cellY;
            multiplier = iterationTotal;
        }
        parentObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(squareCellSide, squareCellSide);

        //Recalculate width and height of canvas
        parentObject.GetComponent<RectTransform>().sizeDelta = new Vector2((cellInRowTotal * squareCellSide), (iterationTotal * squareCellSide));
    }
}
