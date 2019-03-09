using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class TagController : MonoBehaviour
    {
        public GameObject BotGameObject { get; set; }

        private readonly Single _offset;

        private float variableOffset = 0;

        public void SetVariableOffset(float offset)
        {
            variableOffset = offset;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public TagController(Single offset)
        {
            _offset = offset;
        }

        private void LateUpdate()
        {
            // Position the name tag centered above the bot.
            //var botPosition = BotGameObject.transform.position;
            // Always let the name tags look directly at the camera.
            var mainCameraRotation = Camera.main.transform.rotation;
            transform.LookAt(transform.position + mainCameraRotation * Vector3.forward, mainCameraRotation * Vector3.up);
            transform.localPosition = new Vector3(0, 0, 0);
            transform.position = new Vector3(transform.position.x, _offset + variableOffset, transform.position.z);
        }
    }
}