using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObejct : MonoBehaviour, IInteractable
{
    public ItemData data;
    public bool IsEquipped;
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.des}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Inst.Player.data = data;
        CharacterManager.Inst.Player.addItem?.Invoke();
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
