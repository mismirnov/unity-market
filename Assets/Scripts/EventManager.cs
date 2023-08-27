using UnityEngine;

public class EventManager : MonoBehaviour
{
    private ItemMenuManager _menuManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _menuManager = GetComponent<ItemMenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _menuManager.HideItemMenu();
        }
    }
}
