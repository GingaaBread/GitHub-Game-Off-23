using System.Collections;
using main.service.Turn_System;
using UnityEngine;
using Zenject;

namespace main.view.Buttons
{
    public class EndTurnButton : MonoBehaviour
    {
        private Animator _animator;
        private TurnService turnService;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            turnService.OnNewTurnStart.AddListener(Unlock);
        }

        private void OnDisable()
        {
            turnService.OnNewTurnStart.RemoveListener(Unlock);
        }

        [Inject]
        public void Construct(TurnService turnService)
        {
            this.turnService = turnService;
        }

        public void OnClick()
        {
            Lock();
            turnService.EndTurn();
        }

        private void Lock()
        {
            _animator.Play("EndOfTurnLock");
        }

        private void Unlock()
        {
            StartCoroutine(DelayUnlock());
        }

        private IEnumerator DelayUnlock()
        {
            yield return new WaitForSeconds(1f);
            _animator.Play("EndOfTurnUnlock");
        }
    }
}