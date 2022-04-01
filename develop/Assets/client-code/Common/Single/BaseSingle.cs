
public abstract class SingleManager
{
    public bool isInited { get; protected set; }
    public bool isIniting { get; set; }
    public virtual void OnInit() { }
    public virtual void DoUpdate(float deltaTime) { }
    public virtual void OnReConnect() { }
    public virtual void Clear() { }
}

public abstract class BaseSingle<T> : SingleManager where T : class, new()
{
    static T s_Instance = null;

    public static T GetInstance()
    {
        if (s_Instance == null)
        {
            s_Instance = new T();
        }
        return s_Instance;
    }

    public static T instance
    {
        get
        {
            return GetInstance();
        }
    }
}