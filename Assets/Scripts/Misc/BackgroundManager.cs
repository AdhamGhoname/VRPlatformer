using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Texture2D[] backgroundTextures;
    public GameObject background;

    private int _currentBackground = 0;

    private void Awake()
    {
        var objects = GameObject.FindGameObjectsWithTag("BackgroundManager");
        int numberOfLevels = GameObject.FindGameObjectsWithTag("Level").Length;
        if (objects.Length > numberOfLevels)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        //background.GetComponent<Renderer>().material.mainTexture = backgroundTextures[_currentBackground];
        Random.InitState((int)Time.time);
    }

    public void SetRandomBackground()
    {
        int newBackground = Random.Range(0, backgroundTextures.Length);
        while (newBackground == _currentBackground)
        {
            newBackground = Random.Range(0, backgroundTextures.Length);
        }
        _currentBackground = newBackground;
        background.GetComponent<Renderer>().material.mainTexture = backgroundTextures[_currentBackground];
    }
}
