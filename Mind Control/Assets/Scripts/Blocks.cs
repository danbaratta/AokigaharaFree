using UnityEngine;
using System.Collections;

public class Blocks : MonoBehaviour
{
    bool m_Move;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_Move)
        {
            Vector3 temp = Input.mousePosition;
            temp = Camera.main.ScreenToWorldPoint(temp);
            temp.z = transform.position.z;
            transform.position = temp;
        }
    }

    public void MoveBlock()
    {
        m_Move = true;
    }
    public void StopMoveBlock()
    {
        m_Move = false;
    }
}
