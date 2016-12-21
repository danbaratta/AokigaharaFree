using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    float Timer = 5f;
    public bool m_TeleportOn = false;
    public bool m_SpecialBox = false;
    GameObject Morgan;
    // World Limits
    public float Top, Bottom, Left, Right, SpecialLeft, SpecialRight;

    //mouse
    bool m_MouseBool;
    Vector2 m_mouseLocation;
    public float MouseSpeed = 5;
    public float ControllerSpeed = 5;

    // Use this for initialization
    void Start()
    {

        //Buffer Add little padding
        Bottom += 1;
        Left += 1;
        Right -= 1;
        SpecialRight -= 1;
        SpecialLeft += 1;

        Morgan = GameObject.Find("Morgan");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TeleportOn)
        {

            if (!m_MouseBool)
            {
                Cursor.lockState = CursorLockMode.Locked;
                m_mouseLocation = Input.mousePosition;
                m_MouseBool = true;
            }
            if (!gameObject.transform.GetChild(0).gameObject.activeSelf)
                gameObject.transform.GetChild(0).gameObject.SetActive(true);

            Timer -= Time.deltaTime;
            if (Timer > 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("joystick button 0"))
                {
                    Vector3 temp = m_mouseLocation;
                    temp = Camera.main.ScreenToWorldPoint(temp);

                    temp.z = -10;
                    Ray m_Ray = new Ray(temp, Vector3.forward);
                    temp.z = -1;
                    if (temp.y < Bottom)
                        temp.y = Bottom;
                    if (temp.y > Top)
                        temp.y = Top;

                    if (!m_SpecialBox)
                    {
                        if (temp.x < Left)
                            temp.x = Left;
                        if (temp.x > Right)
                            temp.x = Right;
                    }
                    else
                    {
                        if (temp.x < SpecialLeft)
                            temp.x = SpecialLeft;
                        if (temp.x > SpecialRight)
                            temp.x = SpecialRight;
                    }


                    Morgan.transform.position = temp;
                    m_TeleportOn = false;
                    Morgan.SendMessage("TeleportOff");
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    m_MouseBool = false;
                }
                else
                {
                    m_mouseLocation.x += Input.GetAxis("Mouse X") * MouseSpeed;
                    m_mouseLocation.y += Input.GetAxis("Mouse Y") * MouseSpeed;

                    m_mouseLocation.x += Input.GetAxis("Horizontal") * ControllerSpeed;
                    m_mouseLocation.y += Input.GetAxis("Vertical") * ControllerSpeed;
                    Vector3 temp;
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(m_mouseLocation.x, m_mouseLocation.y));

                    temp.z = -10;
                    Ray m_Ray = new Ray(temp, Vector3.forward);
                    temp.z = -1;
                    gameObject.transform.GetChild(0).gameObject.transform.position = temp;
                }

            }
            else
            {
                Vector3 temp = m_mouseLocation;
                temp = Camera.main.ScreenToWorldPoint(temp);

                temp.z = -10;
                Ray m_Ray = new Ray(temp, Vector3.forward);
                temp.z = -1;
                if (temp.y < Bottom)
                    temp.y = Bottom;
                if (temp.y > Top)
                    temp.y = Top;

                if (!m_SpecialBox)
                {
                    if (temp.x < Left)
                        temp.x = Left;
                    if (temp.x > Right)
                        temp.x = Right;
                }
                else
                {
                    if (temp.x < SpecialLeft)
                        temp.x = SpecialLeft;
                    if (temp.x > SpecialRight)
                        temp.x = SpecialRight;
                }
                Morgan.transform.position = temp;
                m_TeleportOn = false;
                Morgan.SendMessage("TeleportOff");
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                m_MouseBool = false;

            }
        }
    }
    public void SetTimer(float t)
    {
        Timer = t;
    }
}
