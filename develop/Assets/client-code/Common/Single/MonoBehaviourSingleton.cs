using UnityEngine;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private Transform m_Tran;
    protected Transform trans
    {
        get
        {
            if (m_Tran == null)
            {
                m_Tran = transform;
            }
            return m_Tran;
        }
    }

    private static T s_instance;
    public static T instance
    {
        get
        {
            return GetInstance();
        }
    }

    public static T GetInstance()
    {
        if (s_instance == null)
        {
            GameObject go = new GameObject(typeof(T).Name);
            GameObject.DontDestroyOnLoad(go);
            Transform tran = go.transform;
            tran.localPosition = Vector3.zero;
            tran.localRotation = Quaternion.identity;
            tran.localScale = Vector3.one;
            s_instance = go.AddComponent<T>();
        }
        return s_instance;
    }

    private void Awake()
    {
        OnInit();
    }

    private void Update()
    {
        OnUpdate(Time.deltaTime);
    }

    private void OnApplicationQuit()
    {
    }

    protected virtual void OnInit() { }
    public virtual void OnUpdate(float deltaTime) { }
    public virtual void OnReConnect() { }
    public virtual void Clear() { }
}