using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelCreator : EditorWindow
{
    int levelLength = 0;
    int groundLayers = 1;
    int whiteBrickLayers = 2;
    int redBrickLayers = 2;
    int spikesLayers = 1;

    Vector2 tileDimensions = Vector2.one;

    GameObject groundTile;
    GameObject whiteBrickTile;
    GameObject redBrickTile;
    GameObject spikesTile;


    private float _optionWidth = 200;

    [MenuItem("Window / LevelCreator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LevelCreator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Tile set", EditorStyles.boldLabel);
        GUILayout.Space(5);

        #region Tiles Prefabs
        GUILayout.BeginHorizontal();
        GUILayout.Label("Ground Tile Prefab");
        groundTile = (GameObject)EditorGUILayout.ObjectField(groundTile, typeof(GameObject), false, GUILayout.MaxWidth(_optionWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("White Brick Tile Prefab");
        whiteBrickTile = (GameObject)EditorGUILayout.ObjectField(whiteBrickTile, typeof(GameObject), false, GUILayout.MaxWidth(_optionWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Red Brick Tile Prefab");
        redBrickTile = (GameObject)EditorGUILayout.ObjectField(redBrickTile, typeof(GameObject), false, GUILayout.MaxWidth(_optionWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spikes Tile Prefab");
        spikesTile = (GameObject)EditorGUILayout.ObjectField(spikesTile, typeof(GameObject), false, GUILayout.MaxWidth(_optionWidth));
        GUILayout.EndHorizontal();
        #endregion

        GUILayout.Space(25);

        GUILayout.Label("Level options", EditorStyles.boldLabel);
        GUILayout.Space(5);

        #region Level Options
        GUILayout.BeginHorizontal();
        tileDimensions = EditorGUILayout.Vector2Field("Tile Dimensions", tileDimensions);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Level Length");
        levelLength = EditorGUILayout.IntField(levelLength, GUILayout.MaxWidth(_optionWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Ground Layers");
        groundLayers = EditorGUILayout.IntField(groundLayers, GUILayout.MaxWidth(_optionWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("White Brick Layers");
        whiteBrickLayers = EditorGUILayout.IntField(whiteBrickLayers, GUILayout.MaxWidth(_optionWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Red Brick Layers");
        redBrickLayers = EditorGUILayout.IntField(redBrickLayers, GUILayout.MaxWidth(_optionWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spikes Layers");
        spikesLayers = EditorGUILayout.IntField(spikesLayers, GUILayout.MaxWidth(_optionWidth));
        GUILayout.EndHorizontal();
        #endregion

        GUILayout.Space(15);
        if (GUILayout.Button("Create Level"))
        {
            CreateLevel();
        }
    }

    private void CreateLevel()
    {
        Transform cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Vector3 origin = cam.position;
        float angle = 2 * Mathf.PI / levelLength;
        float radius = tileDimensions.x / (2 * Mathf.Tan(angle / 2));

        GameObject roundLevel = new GameObject("RoundLevel");


        #region Ground Layers

        GameObject groundTiles = new GameObject("GroundTiles");
        groundTiles.transform.parent = roundLevel.transform;

        for (int layer = 0; layer < groundLayers; layer++)
        {
            for (int i = 0; i < levelLength; i++)
            {
                float x = radius * Mathf.Cos(angle * i);
                float z = radius * Mathf.Sin(angle * i);
                Vector3 position = origin + new Vector3(x, layer * tileDimensions.y, z);
                GameObject tile = Instantiate(groundTile, groundTiles.transform);
                tile.transform.position = position;
                tile.transform.LookAt(cam);
                tile.transform.Rotate(new Vector3(-tile.transform.rotation.eulerAngles.x, 0, 0));
            }
        }
        #endregion

        #region White Brick Layers

        GameObject whiteBrickTiles = new GameObject("WhiteBrickTiles");
        whiteBrickTiles.transform.parent = roundLevel.transform;

        for (int layer = groundLayers; layer < groundLayers + whiteBrickLayers; layer++)
        {
            for (int i = 0; i < levelLength; i++)
            {
                float x = radius * Mathf.Cos(angle * i);
                float z = radius * Mathf.Sin(angle * i);
                Vector3 position = origin + new Vector3(x, layer * tileDimensions.y, z);
                GameObject tile = Instantiate(whiteBrickTile, whiteBrickTiles.transform);
                tile.transform.position = position;
                tile.transform.LookAt(cam);
                tile.transform.Rotate(new Vector3(-tile.transform.rotation.eulerAngles.x, 0, 0));
            }
        }
        #endregion

        #region Red Brick Layers

        GameObject redBrickTiles = new GameObject("RedBrickTiles");
        redBrickTiles.transform.parent = roundLevel.transform;

        for (int layer = groundLayers + whiteBrickLayers; layer < groundLayers + whiteBrickLayers + redBrickLayers; layer++)
        {
            for (int i = 0; i < levelLength; i++)
            {
                float x = radius * Mathf.Cos(angle * i);
                float z = radius * Mathf.Sin(angle * i);
                Vector3 position = origin + new Vector3(x, layer * tileDimensions.y, z);
                GameObject tile = Instantiate(redBrickTile, redBrickTiles.transform);
                tile.transform.position = position;
                tile.transform.LookAt(cam);
                tile.transform.Rotate(new Vector3(-tile.transform.rotation.eulerAngles.x, 0, 0));
            }
        }
        #endregion

        #region Spikes Layers

        GameObject spikesTiles = new GameObject("SpikesTiles");
        spikesTiles.transform.parent = roundLevel.transform;

        for (int layer = groundLayers + whiteBrickLayers + redBrickLayers; layer < groundLayers + whiteBrickLayers + redBrickLayers + spikesLayers; layer++)
        {
            for (int i = 0; i < levelLength; i++)
            {
                float x = radius * Mathf.Cos(angle * i);
                float z = radius * Mathf.Sin(angle * i);
                Vector3 position = origin + new Vector3(x, layer * tileDimensions.y, z);
                GameObject tile = Instantiate(spikesTile, spikesTiles.transform);
                tile.transform.position = position;
                tile.transform.LookAt(cam);
                tile.transform.Rotate(new Vector3(-tile.transform.rotation.eulerAngles.x, 0, 0));
            }
        }
        #endregion

        Debug.Log($"Level created successfully!\nLevel radius = {radius}");
    }
}
