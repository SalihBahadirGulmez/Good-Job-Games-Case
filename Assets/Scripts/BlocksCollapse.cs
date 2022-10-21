using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksCollapse : MonoBehaviour
{
    Blocks blocksSc;
    private bool reBuild = false;
    void Start()
    {
        blocksSc = GameObject.Find("Blocks").GetComponent<Blocks>();
    }

    void OnMouseDown()
    {
        DestroyObject(gameObject);
        if (reBuild)
        {
            blocksSc.indexArray = blocksSc.GroupFinder();
            blocksSc.Shuffle(blocksSc.indexArray);
            blocksSc.Conditions();
        }
    }

    public void DestroyObject(GameObject gameObject)
    {
        reBuild = false;
        for (int i = 0; i < blocksSc.indexArray.Count; i++)
        {
            if (blocksSc.indexArray[i].Count > 3)
            {
                for (int j = 1; j < blocksSc.indexArray[i].Count; j += 2)
                {
                    if (blocksSc.indexArray[i][j] + "" + blocksSc.indexArray[i][j + 1] == gameObject.name)
                    {
                        for (int k = 1; k < blocksSc.indexArray[i].Count; k += 2)
                        {                          
                            int tempNum1 = blocksSc.indexArray[i][k];
                            int tempNum2 = blocksSc.indexArray[i][k + 1];
                            Destroy(blocksSc.blockArray[tempNum1][tempNum2]);
                            blocksSc.blockArrayAsNum[tempNum1][tempNum2] = 10;
                        }
                        reBuild = true;
                        break;
                    }
                }
            }
           
        }
        if (reBuild)
        {
            ReBuild();
        }
    }

    public void ReBuild()
    {
        for (int j = 0; j < blocksSc.blockArrayAsNum[0].Count; j++)
        {
            int numOfNewObj = 0;

            for (int i = 0; i < blocksSc.blockArrayAsNum.Count; i++)
            {
                if (blocksSc.blockArrayAsNum[i][j] == 10)
                {
                    numOfNewObj++;
                    for (int k = i + 1; k < blocksSc.blockArrayAsNum.Count; k++)
                    {
                        if (blocksSc.blockArrayAsNum[k][j] != 10)
                        {
                            blocksSc.blockArrayAsNum[i][j] = blocksSc.blockArrayAsNum[k][j];
                            blocksSc.blockArrayAsNum[k][j] = 10;
                            numOfNewObj--;
                            blocksSc.blockArray[i][j] = blocksSc.blockArray[k][j];
                            blocksSc.blockArray[i][j].GetComponent<SpriteRenderer>().sortingOrder = i;
                            blocksSc.blockArray[i][j].name = i + "" + j;
                            break;
                        }
                    }                  
                }             
            }
            for (int i = blocksSc.row - numOfNewObj; i < blocksSc.row; i++)
            {                   
                int tempRandomNumber = Random.Range(0, blocksSc.numberOfColors);
                blocksSc.blockArrayAsNum[i][j] = tempRandomNumber;
                Vector2 tempPosition = new Vector2(j, 2 + blocksSc.height);
                if (i - 1 >= 0 && blocksSc.blockArray[i - 1][j].transform.position.y > blocksSc.height)
                {
                    tempPosition.y = blocksSc.blockArray[i - 1][j].transform.position.y + 2;
                }
                GameObject block = Instantiate(blocksSc.blocks2d[tempRandomNumber][0], tempPosition, Quaternion.identity);
                block.name = i + "" + j;
                block.GetComponent<SpriteRenderer>().sortingOrder = i;
                blocksSc.blockArray[i][j] = block;
            }
        }
    }
}
