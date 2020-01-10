using Models;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        public GameObject Panel { get; private set; }
        private Text _selectionText;
        private GameObject _image;
        private Image _selectionImage;

        // Start is called before the first frame update
        void Start()
        {
            Panel = GameObject.FindWithTag("Selection Panel");
            _image = GameObject.FindWithTag("Selection Image");
            _selectionText = GameObject.FindWithTag("Selection Text").GetComponent<Text>();
            
            _selectionImage = _image.GetComponent<Image>();
            Panel.SetActive(false);
        }

        public void ChangeMaterialSelection(Tile tile, Material material)
        {
            _selectionText.text = tile.XPos + "|" + tile.YPos + "\n" + tile.Type;
            _selectionImage.material = material;
        }
        
    }
}
