using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    PlayerData m_Player;
    // Use this for initialization

    void onAwake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadPlayerInfo(int slot)
    {
        m_Player = SaveLoad.load(slot);
        if (m_Player == null)
        {
            //Starting point for save data;
            int temp = 0;
            temp = (1 << 0) | (1 << 1);
            SaveLoad.Save(slot, 0, 0, (char)temp);
            LoadPlayerInfo(slot);
        }
    }
}
