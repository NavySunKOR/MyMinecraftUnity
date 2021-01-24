using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecraftMapGen : MonoBehaviour
{
    public GameObject renderBlock;
    public int width;
    public int height;
    public int randSeed;

    private int[,] map;

    private void Start()
    {
        map = new int[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                int genHeight = Random.Range(4, 8);
                map[x, y] = genHeight;
            }
        }

        for(int x = 0; x < map.GetLength(0); x++)
        {
            for(int y = 0; y < map.GetLength(1); y++)
            {
                for(int height = 0; height < map[x,y]; height++)
                {
                    Instantiate(renderBlock, new Vector3(x* renderBlock.transform.localScale.x, height * renderBlock.transform.localScale.y, y * renderBlock.transform.localScale.z), Quaternion.identity);
                }
            }
        }
    }

}
