/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroloopFadeTmeText : MonoBehaviour {

    public Slider fadeTimeSlider;
    public Text text;

    void Start()
    {
        UpdateFadeTimeText();
    }

    public void UpdateFadeTimeText()
    {
        text.text = fadeTimeSlider.value.ToString("0.00") + (fadeTimeSlider.value == 0 ? " second" : " seconds");
    }

}
