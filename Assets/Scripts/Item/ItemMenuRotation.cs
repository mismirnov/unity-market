using UnityEngine;

namespace Item
{
    public class ItemMenuRotation : MonoBehaviour
    {
        public float SpeedInDegrees = 10;
        
        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.up * SpeedInDegrees * Time.deltaTime, Space.Self);
        }
    }
}
