using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolDataType
    {
        public PoolManager.EnemiesType Type;
        public GameObject m_GameObject;
        public int Count = 0;
        List<GameObject> m_Alive = new List<GameObject>();
        List<GameObject> m_Dead = new List<GameObject>();

        void Start()
        {
            //Create(Count);
        }

      public  void Create(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject NewItem = Instantiate(m_GameObject);
                NewItem.SetActive(false);
                m_Dead.Add(NewItem);
                Count++;
                NewItem.SendMessage("Spanwed");
            }
        }

        public GameObject GetObject()
        {
            GameObject temp = null;
            if (m_Dead.Count != 0)
            {
                //Get Temp
                temp = m_Dead[0];
                //Turn on
                temp.SetActive(true);
                // Remove it from dead list
                m_Dead.Remove(temp);
                //Add to alive
                m_Alive.Add(temp);
            }
            else
            {
                //Always create Extra for now
                Create(2);
                temp = GetObject();
            }
            //Send off
            return temp;
        }

        public void Dead(GameObject DeadObject)
        {
            DeadObject.SetActive(false);
            m_Alive.Remove(DeadObject);
            m_Dead.Add(DeadObject);
        }
    }


    public enum EnemiesType
    {
        Yurei_Level1,
        Yurei_Level2,
        Yurei_Level3,
        Yurei_Level3B,
        Oni_Level1,
        Oni_Level2,
        Oni_Level3,
        PlayerBullets,
        EnemyBullets,
    }
    // public List<PoolDataType> Enemies = new List<PoolDataType>();
    public PoolDataType[] m_AllObjects;
    //public List<List<GameObject>> Pool;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < m_AllObjects.Length; i++)
        {
            m_AllObjects[i].Create(m_AllObjects[i].Count);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject FindClass(EnemiesType type)
    {
        GameObject temp = null;
        for (int i = 0; i < m_AllObjects.Length; i++)
        {
            if (m_AllObjects[i].Type == type)
            {
                temp = m_AllObjects[i].GetObject();
                break;
            }
        }
        temp.SendMessage("Reset",SendMessageOptions.DontRequireReceiver);
        return temp;
    }

    public void Remove(GameObject TheGameObject,PoolManager.EnemiesType TheType)
    {
        for (int i = 0; i < m_AllObjects.Length; i++)
        {
            if (m_AllObjects[i].Type == TheType)
            {
                m_AllObjects[i].Dead(TheGameObject);
                break;
            }
        }
    }
}

