using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T m_instance = null;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = (T)FindObjectOfType(typeof(T));

                if (m_instance == null)
                {
                    GameObject singletonHolder = new();
                    m_instance = singletonHolder.AddComponent<T>();

                    singletonHolder.name = "_" + typeof(T).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonHolder);
                }
            }

            return m_instance;
        }
    }

    public static bool IsExisting
    {
        get
        {
            return (T)FindObjectOfType(typeof(T)) != null;
        }
    }

    void OnApplicationQuit()
    {
        m_instance = null;
    }

    void OnDestroy()
    {
        m_instance = null;
    }
}
