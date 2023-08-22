using UnityEngine;

namespace Item
{
    public class ItemMenuRotation : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, 10 * Time.deltaTime, 0);
        }
    }
}
