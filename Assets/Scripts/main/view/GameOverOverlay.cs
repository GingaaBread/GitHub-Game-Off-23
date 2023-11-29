using main.service.Turn_System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using FMODUnity;


namespace main.view
{
    public class GameOverOverlay : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private GameObject _gameOverWinText;
        [SerializeField] private GameObject _gameOverLossText;
        [SerializeField] private StudioEventEmitter _gameMusic;
        [SerializeField] private StudioEventEmitter _gameWinMusic;
        [SerializeField] private StudioEventEmitter _gameLossMusic;


        private GameService gameService;
        

        private void Awake()
        {
            _container.SetActive(false);
        }

        private void Start()
        {
            gameService.OnGameOver.AddListener(() => Render(false));
            // TODO: OnGameWon -> Render(true)
        }

        [Inject]
        public void Construct(GameService gameService)
        {
            this.gameService = gameService;
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Main");
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        private void Render(bool gameIsWon)
        {
            _gameOverWinText.SetActive(gameIsWon);
            _gameOverLossText.SetActive(!gameIsWon);
            _container.SetActive(true);
            _gameMusic.Stop();
            if (gameIsWon) _gameWinMusic.Play();
            else _gameLossMusic.Play();
        }

     
    }
}