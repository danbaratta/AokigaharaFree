﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadialMenu : MonoBehaviour
{
    public Button[] m_Buttons;

    Button m_PrevBtn;
    Button m_CurBtn;
    public Color m_HighlighColor;
    public Color m_NormalColor;
    int m_CurrentButtonNum;
    public GameObject m_RadialMenuUI;
    // Use this for initialization
    void Start()
    {

        var InputTest = Input.GetJoystickNames();
        if (InputTest != null && InputTest.Length > 1)
        {
            if (InputTest[0].Length != 0)
            {
                for (int i = 0; i < m_Buttons.Length; i++)
                {
                    m_Buttons[i].enabled = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        ControllerUpdate();

        //Mouse


        ///

        if (Input.GetButtonDown("joystick button 0"))
        {
            m_Buttons[m_CurrentButtonNum].SendMessage("PickAbility");
            m_RadialMenuUI.SetActive(false);
            Time.timeScale = 1;
        }

        // Close canvus.
        if (Input.GetButtonDown("joystick button 1"))
        {
            m_RadialMenuUI.SetActive(false);
            Time.timeScale = 1;
        }
    }



    void ControllerUpdate()
    {
        // Controller
        float ControllerX, ControllerY;
        ControllerX = Input.GetAxis("Horizontal");
        ControllerY = Input.GetAxis("Vertical");
        //Update only when there controller input that vaild
        if (ControllerX != 0 || ControllerY != 0)
        {
            float temp;

            temp = Mathf.Atan2(ControllerX, ControllerY);
            temp = Mathf.Rad2Deg * temp;
            temp = temp % 360;
            if (temp < 0)
                temp = 360 + temp;
            //Debug.Log("Angle = " + temp);

            float buffer = 360 / m_Buttons.Length;

            // Debug.Log("CurrentBtnOn = "+(int)((temp / 360) * 6));
            if (m_Buttons[(int)((temp / 360) * m_Buttons.Length)].gameObject.GetComponent<RadialBtn>().Type != Abilities.m_Abilities.MaxSize)

            {
                m_Buttons[(int)((temp / 360) * m_Buttons.Length)].gameObject.GetComponent<RadialBtn>().EnterText();
                m_Buttons[(int)((temp / 360) * m_Buttons.Length)].enabled = true;

                m_CurrentButtonNum = (int)((temp / 360) * m_Buttons.Length);
                ColorBlock myColor;
                if (m_CurBtn != null && m_PrevBtn != m_Buttons[m_CurrentButtonNum])
                {
                    myColor = m_CurBtn.colors;
                    myColor.normalColor = m_NormalColor;
                    m_CurBtn.colors = myColor;
                    m_CurBtn.enabled = false;
                    m_CurBtn.gameObject.GetComponent<RadialBtn>().ExitText();
                    m_PrevBtn = m_CurBtn;

                }
                else if (m_PrevBtn != null && m_CurBtn)
                    m_PrevBtn = m_CurBtn;
                m_CurBtn = m_Buttons[(int)((temp / 360) * m_Buttons.Length)];
                myColor = m_CurBtn.colors;
                myColor.normalColor = m_HighlighColor;
                m_CurBtn.colors = myColor;
            }
        }
    }

    void MouseUpdate()
    {

    }


    void OnMouseOver()
    {

    }
}
