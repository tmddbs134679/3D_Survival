using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _inst;

    public static CharacterManager Inst
    {
        get
        {
            if(_inst == null)
            {
                _inst = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _inst;
        }
    }
   
    public Player _player;

    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private void Awake()
    {
        if(_inst == null )
        {
            _inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(_inst != this)
            {
                Destroy(gameObject);
            }
        }

    }
}
