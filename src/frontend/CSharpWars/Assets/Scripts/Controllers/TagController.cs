using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public abstract class TagController : MonoBehaviour
    {
        private readonly float _offset;

        protected TagController(float offset)
        {
            _offset = offset;
        }

        // Camera related movements should execute after all other updates have been processed!
        private void LateUpdate()
        {
            // Always let the name tags look directly at the camera.
            var mainCameraRotation = Camera.main.transform.rotation;
            transform.LookAt(transform.position + mainCameraRotation * Vector3.forward, mainCameraRotation * Vector3.up);
            transform.localPosition = new Vector3(0, 0, 0);
            transform.position = new Vector3(transform.position.x, _offset, transform.position.z);
        }

        public abstract void UpdateTag(Bot bot);

        public void Destroy()
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }
}