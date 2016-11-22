/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroloopTestSwitches : MonoBehaviour {

    public bool useFade;
    public float fadeTime;

    public Toggle fadeToggle;
    public void UseFade()
    {
        useFade = fadeToggle.isOn;
    }

    public Slider fadeTimeSlider;
    public void UpdateFadeTime()
    {
        fadeTime = fadeTimeSlider.value;
    }

}
