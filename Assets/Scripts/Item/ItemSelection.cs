using UnityEngine;

namespace Item
{
    public class ItemSelection : MonoBehaviour
    {
        private GameObject _model;
        private MeshRenderer _mesh;
        private Material _originalMaterial;

        [SerializeField] private Material hightlightedMaterial;
        private ItemMenuManager _itemMenuManager;
    
        void Start()
        {
            _mesh = GetComponent<MeshRenderer>();
            _originalMaterial = _mesh.material;
            _itemMenuManager = GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<ItemMenuManager>();
        }

        public void InitModel(GameObject go)
        {
            _model = go;
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

        private void OnMouseDown()
        {
            if (_model == null) return;
        
            _itemMenuManager.ShowItemMenu(_model);
        }
    }
}
