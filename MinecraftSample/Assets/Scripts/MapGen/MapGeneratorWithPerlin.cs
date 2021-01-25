using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedTerrain
{
    public List<GameObject> meshes;

    public GeneratedTerrain()
    {
        meshes = new List<GameObject>();
    }
}

public class MapGeneratorWithPerlin : MonoBehaviour
{
    public enum RenderType
    { 
        Cubes,
        Color
    }

    [Header("가시거리")]
    public int visibleDistance;
    [Header("해상도 조정 - square 설정(정사각형의 한면)")]
    public int square;
    [Header("스케일 조정 - 스케일이 높을 수록 지형들의 크기가 커진다.(지형 밀도가 작아짐)")]
    public int scaleFactor; 
    public MeshRenderer meshRenderer;
    public Transform meshTr;
    public GameObject meshCube;
    public RenderType renderType;

    private Dictionary<Vector2, GeneratedTerrain> terrainDic;



    private float[,] maps;
    private Transform playerTr;
    private Vector2 playerPos = new Vector2();

    private void Start()
    {
        playerTr = Camera.main.transform;
        terrainDic = new Dictionary<Vector2, GeneratedTerrain>();
        maps = CreateGenMap(square, square, scaleFactor);
        if(renderType == RenderType.Color)
        {
            RenderMapWithColor();
        }
        else
        {
            RenderMapWithMesh();
        }
    }

    private void Update()
    {
        playerPos.x = playerTr.position.x;
        playerPos.y = playerTr.position.z;

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
        maps = CreateGenMap(square, square, scaleFactor);
        if (renderType == RenderType.Color)
            RenderMapWithColor();
        else
            RenderMapWithMesh();
    }


    private void RenderMapWithColor()
    {
        Color[] colors = new Color[square * square];
        for (int x = 0; x < square; x++)
        {
            for (int y = 0; y < square; y++)
            {
                colors[x + y * square] = Color.Lerp(Color.white, Color.black, maps[x, y]);
                //Debug.Log("x : " + x + " / y : " + y + " : " + colors[x + y]);
            }
        }

        Texture2D texture2D = new Texture2D(square, square);
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
        GeneratedTerrain generatedTerrain = new GeneratedTerrain();
        for (int x = 0; x < square; x++)
        {
            for (int y = 0; y < square; y++)
            {
                int perlinCount = (int)(maps[x, y] * 10);
                GameObject gb = Instantiate(meshCube, new Vector3(x - (square / 2) , perlinCount, y - (square / 2)),Quaternion.identity);
                gb.transform.parent = meshTr;
                generatedTerrain.meshes.Add(gb);
            }
        }

        terrainDic.Add(new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z), generatedTerrain);
    }
}
