using UnityEngine;

public class ItemSelection : MonoBehaviour
{
    private MeshRenderer _mesh;
    private Material _originalMaterial;

    [SerializeField] private Material hightlightedMaterial;
    
    void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
        _originalMaterial = _mesh.material;
        EnableHighlight(false);
    }

    public void EnableHighlight(bool flag)
    {
        if (hightlightedMaterial == null) return;

        _mesh.material = flag 
            ? hightlightedMaterial 
            : _originalMaterial;
    }

    private void OnMouseEnter()
    {
        EnableHighlight(true);
    }

    private void OnMouseExit()
    {
        EnableHighlight(false);
    }
}
