using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour
{
   public AudioScript m_AudioScript;
    bool Battle;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!Battle)
        {
            m_AudioScript.Track(1);
            Battle = true;
        }
        else
        {
            m_AudioScript.Track(0);
            Battle = false;
        }

    }
}
