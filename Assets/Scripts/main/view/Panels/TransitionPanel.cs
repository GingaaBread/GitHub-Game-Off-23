using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace main.view.Panels
{
    public class TransitionPanel : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _prefix;
        [SerializeField] [Scene] private string _sceneToLoadAfterOut;

        private bool _exitApplicationOnFinish;

        public void TransitionIn()
        {
            _animator.Play(_prefix + "_In");
        }

        public void TransitionOut()
        {
            _animator.Play(_prefix + "_Out");
        }

        public void TransitionOutThenExit()
        {
            _exitApplicationOnFinish = true;
            TransitionOut();
        }

        public void FinishTransition()
        {
            if (_exitApplicationOnFinish) Application.Quit();
            else if (_sceneToLoadAfterOut != null) SceneManager.LoadScene(_sceneToLoadAfterOut);
        }
    }
}