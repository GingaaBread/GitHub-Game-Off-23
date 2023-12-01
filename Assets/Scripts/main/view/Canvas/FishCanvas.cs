using System;
using System.Collections;
using System.Collections.Generic;
using main.service.Fish_Management;
using main.service.Turn_System;
using main.view.Panels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace main.view.Canvas
{
    public class FishCanvas : MonoBehaviour
    {
        [SerializeField] private SlashPanel _slashPrefab;
        [SerializeField] private Image _rawFishImage;
        [SerializeField] private RemainingScalesView _remainingScalesView;
        [SerializeField] private RemainingFishView _remainingFishView;
        [SerializeField] private Animator _newFishAnimator;
        private readonly Queue<EndOfTurnSegment> _endOfTurnQueue = new();
        private float _currentAlphaDamage;
        private EffectAssemblyService _effectAssemblyService;
        private FishService _fishService;

        private TurnService _turnService;
        private int damage;

        private void OnEnable()
        {
            _fishService.OnFishHasReceivedDamage.AddListener(EnqueueFishScale);
            _fishService.OnFishHasBeenScaled.AddListener(EnqueueFishKill);
            _effectAssemblyService.OnEffectsWereExecuted.AddListener(HandleNextSegment);

            UpdateScalesView();
        }

        private void OnDisable()
        {
            _fishService.OnFishHasReceivedDamage.RemoveListener(EnqueueFishScale);
            _fishService.OnFishHasBeenScaled.RemoveListener(EnqueueFishKill);
            _effectAssemblyService.OnEffectsWereExecuted.RemoveListener(HandleNextSegment);
        }

        [Inject]
        public void Construct(FishService fishService, EffectAssemblyService effectAssemblyService,
            TurnService turnService)
        {
            _fishService = fishService;
            _effectAssemblyService = effectAssemblyService;
            _turnService = turnService;
        }

        private void EnqueueFishKill()
        {
            _endOfTurnQueue.Enqueue(new FishKillSegment());
        }

        private void EnqueueFishScale(int amountThatHasBeenScaled)
        {
            _endOfTurnQueue.Enqueue(new ScaleDamageSegment
            {
                DamageAmount = amountThatHasBeenScaled
            });
        }

        private void HandleNextSegment()
        {
            if (_endOfTurnQueue.Count is 0)
            {
                _turnService.ProceedWithEndOfTurn();
            }
            else
            {
                var nextSegment = _endOfTurnQueue.Dequeue();
                switch (nextSegment)
                {
                    case ScaleDamageSegment scaleDamageSegment:
                        damage = scaleDamageSegment.DamageAmount;
                        _rawFishImage.color = new Color(1f, 1f, 1f,
                            TranslateDamageToRawFishAlphaColour(damage));
                        UpdateScalesView();
                        StartCoroutine(CreateSlash(TranslateDamageToSlashAmount(damage)));
                        break;
                    case FishKillSegment:
                        StartCoroutine(HandleFishKill());
                        break;
                    default:
                        throw new NotImplementedException("Segment is not implemented");
                }
            }
        }

        private IEnumerator HandleFishKill()
        {
            _currentAlphaDamage = 0f;
            _newFishAnimator.Play("New_Fish");
            UpdateScalesView();
            _remainingFishView.IncrementAndRender();

            yield return new WaitForSeconds(2.3f);
            HandleNextSegment();
        }

        private void UpdateScalesView()
        {
            _remainingScalesView.Render(_fishService.GetBaseScalesOfCurrentFish() - (int)_currentAlphaDamage);
        }

        private float TranslateDamageToRawFishAlphaColour(int damage)
        {
            _currentAlphaDamage += damage;
            return _currentAlphaDamage / _fishService.GetBaseScalesOfCurrentFish();
        }

        private static int TranslateDamageToSlashAmount(int damage)
        {
            return damage switch
            {
                >= 100 => 5,
                >= 50 => 4,
                >= 20 => 3,
                >= 10 => 2,
                _ => 1
            };
        }

        private IEnumerator CreateSlash(int amountOfSlashes)
        {
            while (amountOfSlashes > 0)
            {
                Instantiate(_slashPrefab, transform).Render();
                amountOfSlashes--;
                yield return new WaitForSeconds(0.2f);
            }

            UpdateScalesView();
            HandleNextSegment();
        }

        private abstract class EndOfTurnSegment
        {
        }

        private class ScaleDamageSegment : EndOfTurnSegment
        {
            public int DamageAmount { set; get; }
        }

        private class FishKillSegment : EndOfTurnSegment
        {
        }
    }
}