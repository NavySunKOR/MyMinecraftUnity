using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorWithPerlin : MonoBehaviour
{
    public enum RenderType
    { 
        Cubes,
        Color
    }

    [Header("해상도 조정 - width, height 설정")]
    public int width;
    public int height;
    [Header("스케일 조정 - 스케일이 높을 수록 지형들의 크기가 커진다.(지형 밀도가 작아짐)")]
    public int scaleFactor; 
    public MeshRenderer meshRenderer;
    public Transform meshTr;
    public GameObject meshCube;
    public RenderType renderType;



    private float[,] maps; 

    private void Start()
    {
        maps = CreateGenMap(width, height, scaleFactor);
        if(renderType == RenderType.Color)
        {
            RenderMapWithColor();
        }
        else
        {
            RenderMapWithMesh();
        }
    }


 

    private void OnGUI()
    {
        if(GUILayout.Button("Apply"))
        {
            RecreateMap();
        }
    }

    float[,] CreateGenMap(int width,int height,int scaleFactor)
    {
        float[,] map = new float[width,height];

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                map[x, y] = Mathf.PerlinNoise((float)x / scaleFactor,(float)y/scaleFactor);
                //Debug.Log("XCoord: " + x + " / YCoord :" + y + " ::::: " + map[x, y]);
            }
        }
        return map;
    }

    private void RecreateMap()
    {
        maps = CreateGenMap(width, height, scaleFactor);
        if (renderType == RenderType.Color)
            RenderMapWithColor();
        else
            RenderMapWithMesh();
    }


    private void RenderMapWithColor()
    {
        Color[] colors = new Color[width * height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                colors[x + y * width] = Color.Lerp(Color.white, Color.black, maps[x, y]);
                //Debug.Log("x : " + x + " / y : " + y + " : " + colors[x + y]);
            }
        }

        Texture2D texture2D = new Texture2D(width, height);
        texture2D.SetPixels(colors);
        texture2D.Apply();

        meshRenderer.sharedMaterial.SetTexture("_MainTex", texture2D);
    }
    private void RenderMapWithMesh()
    {
        foreach(Transform tr in meshTr)
        {
            Destroy(tr.gameObject);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int perlinCount = (int)(maps[x, y] * 10);
                GameObject gb = Instantiate(meshCube, new Vector3(x - (width / 2) , perlinCount, y - (height / 2)),Quaternion.identity);
                gb.transform.parent = meshTr;
            }
        }

    }
}
