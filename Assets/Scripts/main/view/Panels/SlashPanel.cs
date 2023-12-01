using UnityEngine;
using UnityEngine.UI;

namespace main.view.Panels
{
    public class SlashPanel : MonoBehaviour
    {
        [SerializeField] private Sprite[] _textures;

        private Image _image;
        private RectTransform _transform;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _transform = GetComponent<RectTransform>();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void Render()
        {
            _image.sprite = _textures[Random.Range(0, _textures.Length)];

            var scale = Random.Range(0.5f, 1f);
            _transform.localScale = new Vector3(scale, scale);
            var rotation = _transform.rotation;
            rotation = new Quaternion(rotation.x, rotation.y,
                Random.Range(-90, 90), rotation.w);
            _transform.rotation = rotation;
        }
    }
}