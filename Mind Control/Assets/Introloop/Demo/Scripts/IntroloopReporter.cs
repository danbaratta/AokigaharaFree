/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class IntroloopReporter : MonoBehaviour {

    public Text text1;
    public Text text2;
    private StringBuilder sb;
    private string[] debug1,debug2;

	// Use this for initialization
	void Start () {
        sb = new StringBuilder();
	}
	
	// Update is called once per frame
	void Update () {
        debug1 = IntroloopPlayer.Instance.GetDebugInformation1();
        debug2 = IntroloopPlayer.Instance.GetDebugInformation2();
        sb.Length = 0;
        sb.Append("DSP TIME : " + AudioSettings.dspTime + "\n");
        sb.Append("TOTAL PLAY TIME : " + IntroloopPlayer.Instance.GetTime() + "\n");
        sb.Append("<color=\"yellow\">= Track 1 =\n</color>");
        foreach(string s in debug1)
        {
            sb.Append(s);
            sb.Append("\n");
        }

        text1.text = sb.ToString();
        sb.Length = 0;
            sb.Append("\n");
            sb.Append("\n");
        sb.Append("<color=\"yellow\">= Track 2 =\n</color>");
        foreach(string s in debug2)
        {
            sb.Append(s);
            sb.Append("\n");
        }
        text2.text = sb.ToString();

	}
}
