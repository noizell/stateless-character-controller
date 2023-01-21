using Stateless;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace STVR.SMH
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainMenuCanvas;

        [SerializeField]
        private GameObject lighting;
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button exitButton;
        [SerializeField]
        private Image fadingImg;


        private StateMachine<GameState, GameStateTrigger> _machine;

        public enum GameState
        {
            Initializing,
            Menu,
            Gameplay
        }

        public enum GameStateTrigger
        {
            OnButtonPress,
            OnInitializing
        }

        private void Start()
        {
            _machine = new StateMachine<GameState, GameStateTrigger>(GameState.Initializing);
            _machine.Configure(GameState.Initializing).Permit(GameStateTrigger.OnInitializing, GameState.Menu);
            _machine.Configure(GameState.Menu).Permit(GameStateTrigger.OnButtonPress, GameState.Gameplay).OnEntry(() => OpenMenu());
            _machine.Configure(GameState.Gameplay).OnEntry(() => StartGame());

            _machine.Fire(GameStateTrigger.OnInitializing);
            startButton.onClick.AddListener(() => OnChangeState());
        }

        private void StartGame()
        {
            fadingImg.gameObject.SetActive(true);
            fadingImg.DOFade(1f, 1f).onComplete = () =>
            {
                SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive).completed += MenuManager_completed;
                lighting.SetActive(false);
            };
        }

        private void MenuManager_completed(AsyncOperation obj)
        {
            Debug.Log("done load");
            mainMenuCanvas.gameObject.SetActive(false);
            obj.allowSceneActivation = true;
            fadingImg.DOFade(0f, 1f).onComplete = () =>
            {
                fadingImg.gameObject.SetActive(false);
                SceneManager.UnloadSceneAsync("Menu");
            };
        }

        private void OpenMenu()
        {
            mainMenuCanvas.SetActive(true);
        }

        private void OnChangeState()
        {
            startButton.transform.DOScale(2f, 0.2f).onComplete = () =>
            {
                startButton.transform.DOScale(1f, .18f);
                _machine.Fire(GameStateTrigger.OnButtonPress);
            };

        }
    }
}