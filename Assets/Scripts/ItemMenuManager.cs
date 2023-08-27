using Item;
using UnityEngine;

public class ItemMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    private GameObject _currentModel;
    private ItemSelection _itemSelection;
    private GameObject _modelPosition;

    private void Start()
    {
        _modelPosition = GameObject.FindGameObjectWithTag("Menu-model__position");
        canvas.SetActive(false);
    }

    public void ShowItemMenu(GameObject obj)
    {
        if (canvas == null) return;
        canvas.SetActive(true);
        
        if (_currentModel != null) return;

        _itemSelection = obj.GetComponentInParent<ItemSelection>();
        _currentModel = Instantiate(obj, _modelPosition.transform, false);

        _currentModel.transform.localPosition = Vector3.zero;
        _currentModel.transform.localRotation = Quaternion.identity;

        foreach (Transform child in _currentModel.transform)
        {
            child.position = new Vector3(
                _currentModel.transform.position.x,
                _currentModel.transform.position.y,
                _currentModel.transform.position.z);
        }
        _currentModel.AddComponent<ItemMenuRotation>();
        ChangeLayers(_currentModel, "UI");
    }

    public void HideItemMenu()
    {
        if (_currentModel == null) return;
        
        canvas.SetActive(false);
        _itemSelection.EnableHighlight(false);
        
        Destroy(_currentModel);
        _currentModel = null;
    }
    
    void ChangeLayers(GameObject go, string layer)
    {
        go.layer = LayerMask.NameToLayer(layer);
        foreach (var child in go.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("UI");
        }
    }
}
