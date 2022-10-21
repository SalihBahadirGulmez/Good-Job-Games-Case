using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public List<GameObject> blueBlocks = new List<GameObject>();
    public List<GameObject> greenBlocks = new List<GameObject>();
    public List<GameObject> pinkBlocks = new List<GameObject>();
    public List<GameObject> purpleBlocks = new List<GameObject>();
    public List<GameObject> redBlocks = new List<GameObject>();
    public List<GameObject> yellowBlocks = new List<GameObject>();
    public List<List<GameObject>> blocks2d = new List<List<GameObject>>();

    public int row = 10;
    public int column = 10;
    public int numberOfColors = 6;
    public int conditionA = 4;
    public int conditionB = 6;
    public int conditionC = 7;

    private int blockNum;
    private bool unique = true;
    public int height = 14;

    public List<List<int>> indexArray;
    public List<List<GameObject>> blockArray = new List<List<GameObject>>();
    public List<List<int>> blockArrayAsNum = new List<List<int>>();

    private void Start()
    {
        blocks2d.Add(blueBlocks);
        blocks2d.Add(greenBlocks);
        blocks2d.Add(pinkBlocks);
        blocks2d.Add(purpleBlocks);
        blocks2d.Add(redBlocks);
        blocks2d.Add(yellowBlocks);

        Camera.main.transform.position = new Vector3((column / 2f) - 0.5f, 0, -10);
               
        CreateBlocks();      
        indexArray = GroupFinder();
        Shuffle(indexArray);
        Conditions();
    }

    public void CreateBlocks()
    {
        for (int i = 0; i < row; i++)
        {
            blockArrayAsNum.Add(new List<int>());
            blockArray.Add(new List<GameObject>());
            for (int j = 0; j < column; j++)
            {
                int tempRandomNumber = Random.Range(0, numberOfColors);
                blockArrayAsNum[i].Add(tempRandomNumber);
                GameObject block = Instantiate(blocks2d[tempRandomNumber][0], new Vector2(j, i + height), Quaternion.identity);
                block.name = i + "" + j;
                block.GetComponent<SpriteRenderer>().sortingOrder = i;
                blockArray[i].Add(block);                             
            }
        }
    }

    public List<List<int>> GroupFinder()
    {
        List<List<int>> _indexArray = new List<List<int>>();

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                blockNum = blockArrayAsNum[i][j];
                List<int> subIndexArray = new List<int>();
                                       
                if (IsUnique(_indexArray, i, j))
                {
                    subIndexArray.Add(blockNum);
                    subIndexArray.Add(i);
                    subIndexArray.Add(j);

                    SearchNeighbors(subIndexArray, _indexArray, i, j);

                    _indexArray.Add(subIndexArray);
                }                        
            }
        }   
        return _indexArray;
    }

    public List<int> SearchNeighbors(List<int> _subIndexArray, List<List<int>> _indexArray, int _row, int _column)
    {
        for (int k = -1; k < 2; k++)
        {
            for (int l = -1; l < 2; l++)
            {
                if (k != -l && k != l 
                    && _row + k >= 0 && _row + k < row 
                    && _column + l >= 0 && _column + l < column 
                    && blockArrayAsNum[_row + k][_column + l] == blockNum)
                {
                    unique = IsUniqueV2(_subIndexArray, _indexArray, _row, _column, k, l);  
                    
                    if (unique)
                    {
                        _subIndexArray.Add(_row + k);
                        _subIndexArray.Add(_column + l);
                        SearchNeighbors(_subIndexArray, _indexArray, _row + k, _column + l);
                    }
                }
            }
        }        
        return _subIndexArray;
    }

    public bool IsUniqueV2(List<int> _subIndexArray, List<List<int>> _indexArray, int _row, int _column, int neighbourR, int neighbourC)
    {          
        for (int m = 1; m < _subIndexArray.Count; m += 2)
        {
            if (_subIndexArray[m] == _row + neighbourR && _subIndexArray[m + 1] == _column + neighbourC)
            {
                return false;                    
            }
        }               
        return true;
    }

    public bool IsUnique(List<List<int>> _indexArray, int _row, int _column)
    {
        for (int i = 0; i < _indexArray.Count; i++)
        {
            for (int j = 1; j < _indexArray[i].Count; j += 2)
            {
                if (_indexArray[i][j] == _row  && _indexArray[i][j + 1] == _column )
                {
                    return false;
                }
            }
        }
        return true;
    }
        
    public void Conditions()
    {
        for (int i = 0; i < indexArray.Count; i++)
        {
            if (indexArray[i].Count > conditionC * 2)
            {
                for (int j = 1; j < indexArray[i].Count; j += 2)
                {
                    int tempNum1 = indexArray[i][j];
                    int tempNum2 = indexArray[i][j + 1];
                    Vector2 tempPos = blockArray[tempNum1][tempNum2].transform.position;
                    Destroy(blockArray[tempNum1][tempNum2]);
                    GameObject block = Instantiate(blocks2d[indexArray[i][0]][3], tempPos, Quaternion.identity);
                    block.name = tempNum1 + "" + tempNum2;
                    block.GetComponent<SpriteRenderer>().sortingOrder = tempNum1;
                    blockArray[tempNum1][tempNum2] = block;
                }
            }
            else if (indexArray[i].Count > conditionB * 2)
            {
                for (int j = 1; j < indexArray[i].Count; j += 2)
                {
                    int tempNum1 = indexArray[i][j];
                    int tempNum2 = indexArray[i][j + 1];
                    Vector2 tempPos = blockArray[tempNum1][tempNum2].transform.position;                    
                    Destroy(blockArray[tempNum1][tempNum2]);
                    GameObject block = Instantiate(blocks2d[indexArray[i][0]][2], tempPos, Quaternion.identity);
                    block.name = tempNum1 + "" + tempNum2;
                    block.GetComponent<SpriteRenderer>().sortingOrder = tempNum1;
                    blockArray[tempNum1][tempNum2] = block;
                }
            }
            else if (indexArray[i].Count > conditionA * 2)
            {
                for (int j = 1; j < indexArray[i].Count; j += 2)
                {
                    int tempNum1 = indexArray[i][j];
                    int tempNum2 = indexArray[i][j + 1];
                    Vector2 tempPos = blockArray[tempNum1][tempNum2].transform.position;
                    Destroy(blockArray[tempNum1][tempNum2]);
                    GameObject block = Instantiate(blocks2d[indexArray[i][0]][1], tempPos, Quaternion.identity);
                    block.name = tempNum1 + "" + tempNum2;
                    block.GetComponent<SpriteRenderer>().sortingOrder = tempNum1;
                    blockArray[tempNum1][tempNum2] = block;
                }
            }
            else
            {
                for (int j = 1; j < indexArray[i].Count; j += 2)
                {
                    int tempNum1 = indexArray[i][j];
                    int tempNum2 = indexArray[i][j + 1];
                    Vector2 tempPos = blockArray[tempNum1][tempNum2].transform.position;
                    Destroy(blockArray[tempNum1][tempNum2]);
                    GameObject block = Instantiate(blocks2d[indexArray[i][0]][0], tempPos, Quaternion.identity);
                    block.name = tempNum1 + "" + tempNum2;
                    block.GetComponent<SpriteRenderer>().sortingOrder = tempNum1;
                    blockArray[tempNum1][tempNum2] = block;
                }
            }

        }
    }
    
    public void Shuffle(List<List<int>> _indexArray)
    {     
        for (int i = 0; i < _indexArray.Count; i++)
        {
            if (indexArray[i].Count > 3)
            {
                return;
            }
        }
        RemoveBlocks(blockArray);
        CreateBlocksV2();
        indexArray = GroupFinder();
        Shuffle(indexArray);
    }

    public void RemoveBlocks(List<List<GameObject>> _blockArray)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Destroy(_blockArray[i][j]);
            }
        }
    }

    public void CreateBlocksV2()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                int tempRandomNumber = Random.Range(0, numberOfColors);
                blockArrayAsNum[i][j] = tempRandomNumber;
                GameObject block = Instantiate(blocks2d[tempRandomNumber][0], new Vector2(j, i + height), Quaternion.identity);
                block.name = i + "" + j;
                block.GetComponent<SpriteRenderer>().sortingOrder = i;
                blockArray[i][j] = block;
            }
        }
    }
}
