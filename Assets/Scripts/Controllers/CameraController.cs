using System;
using Models;
using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float translationMultiplier = 20f;
        [SerializeField] private float zoomMultiplier = 100f;

        private World _world;
        private Camera _cam;
        
        private void Start()
        {
            _cam = Camera.main;
            _world = WorldManager.Instance.World;
        }

        Vector3 GetInputTranslationDirection()
        {
            Vector3 direction = new Vector3();
            
            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector3.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                direction += Vector3.down;
            }
            if (Input.GetKey(KeyCode.A))
            {
                direction += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction += Vector3.right;
            }
            
            return direction;
        }

        float GetZoom()
        {
            float zoom = 0f;

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                zoom += zoomMultiplier;
            } 
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                zoom -= zoomMultiplier;
            }

            return zoom;
        }

        // Update is called once per frame
        void Update()
        {
            var translation = GetInputTranslationDirection() * (Time.deltaTime * translationMultiplier);
            transform.Translate(translation);

            var position = transform.position;
            
            position = new Vector3(
                Mathf.Clamp(position.x, 0, _world.Width),
                Mathf.Clamp(position.y, 0, _world.Height),
                position.z
            );
            
            transform.position = position;

            _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize += GetZoom() * Time.deltaTime, 3f, 30f);
        }
    }
}
