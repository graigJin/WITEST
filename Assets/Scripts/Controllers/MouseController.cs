using Models;
using UnityEngine;

namespace Controllers
{
    public class MouseController : MonoBehaviour
    {
        private Camera _cam;
        
        void Start()
        {
            _cam = Camera.main;
        }

        void Update()
        {
            HandleMovement();
            HandleLeftClick();
        }

        void HandleMovement()
        {

        }
        
        void HandleLeftClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 currentMousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);

                int x = Mathf.FloorToInt(currentMousePosition.x);
                int y = Mathf.FloorToInt(currentMousePosition.y);

                WorldManager.Instance.OnTileSelected(x,y);
            }   
        }
    }
}
