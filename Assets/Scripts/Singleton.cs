using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    public static T Instance { get; private set; }
    [SerializeField] private bool dontDestroyOnLoad = false;
    
    protected virtual void Awake() {
        if (Instance == null)
            Instance = (T)this;
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        if(dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}