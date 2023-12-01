using UnityEngine;

namespace main.view.Canvas
{
    public class WarningCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _timeWarningContainer;
        private Animator _animator;

        public static WarningCanvas Instance { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            Instance = this;
        }

        public void DisplayTimeWarning()
        {
            if (_timeWarningContainer.activeInHierarchy) return;
            _animator.Play("Warning_Time");
        }
    }
}