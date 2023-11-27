using UnityEngine;
using UnityEngine.SceneManagement;

namespace main.view.Canvas
{
    public class ExitCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _container;

        public void ConfirmExit()
        {
            SceneManager.LoadScene("Menu");
        }

        public void Render(bool enable)
        {
            _container.SetActive(enable);
        }
    }
}