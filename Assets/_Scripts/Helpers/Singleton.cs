using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _ref;

    public static bool isValid => _ref != null;


    public bool isDontDestroy;

    public static T Instance
    {
        get
        {
            if (!isValid)
                _ref = FindAnyObjectByType<T>();
            return _ref;
        }
    }

    protected void Awake()
    {
        if (isValid && _ref != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _ref = (T)(MonoBehaviour)this;
            if (isDontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
