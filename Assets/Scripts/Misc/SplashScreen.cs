using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SplashScreen : MonoBehaviour
{

    public Text text;
    public AnimationCurve fadeFunction;
    public string nextLevel;
    public float fadeInTime, fadeOutTime, nextSceneTime;
    float timer = 0;

    // Update is called once per frame
    void Update()
    {
        float opacity = 0;
        if (timer < fadeInTime)
        {
            opacity = timer / fadeInTime;
        }
        else if (fadeInTime <= timer && timer <= fadeOutTime)
        {
            opacity = 1;
        }
        else if (fadeOutTime <= timer && timer <= nextSceneTime)
        {
            opacity = 1 - (timer - fadeOutTime) / (nextSceneTime - fadeOutTime);
        }
        else
        {
            SceneManager.LoadScene(nextLevel);
        }
        timer += Time.deltaTime;
        text.color = new Color(text.color.r, text.color.g, text.color.b, fadeFunction.Evaluate(opacity));
    }
}
