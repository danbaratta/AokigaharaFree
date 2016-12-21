using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // Test
        //Save(0, 25, 0, (char)0);
        //load(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static PlayerData load(int slot)
    {

        PlayerData m_PlayerSaveInfo = null;
        if (File.Exists(Application.persistentDataPath + "/GameSlot" + slot + ".gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/GameSlot" + slot + ".gd", FileMode.Open);
            m_PlayerSaveInfo = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
        return m_PlayerSaveInfo;

    }

    public static void Save(int Slot, int KillCount, int DeathCount, char AbilbityUnlocked)
    {
        PlayerData temp = new PlayerData();
        temp.m_KillCount = KillCount;
        temp.m_DeathCount = DeathCount;
        temp.m_AbilityUnlocked = AbilbityUnlocked;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/GameSlot" + Slot + ".gd");
        Debug.Log(Application.persistentDataPath);
        bf.Serialize(file, temp);
        file.Close();
    }

}

[System.Serializable]
public class PlayerData
{
    public int m_KillCount = 0;
    public int m_DeathCount = 0;
    public char m_AbilityUnlocked = (char)3;
    public char m_LevelUnlocked = (char)1;
}