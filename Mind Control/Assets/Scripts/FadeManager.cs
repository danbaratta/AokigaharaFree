using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class FadeManager : MonoBehaviour
    {
        public static FadeManager Instance { set; get; }
        public Image FadeImage;
        private float _duration;
        private float _transition;
        private bool _showing;
        private bool _inTransition;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(FadeImage);
        }

        public void Fade(bool showing, float duration)
        {
            _showing = showing;
            _inTransition = true;
            _duration = duration;
            _transition = _showing ? 0 : 1;
        }

        //void OnEnable()
        //{
        //    SceneManager.sceneLoaded += OnLevelFinishedLoading;
        //}

        //void OnDisable()
        //{
        //    SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        //}

        //void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        //{
        //    Debug.Log("Level Loaded");
        //    Debug.Log(scene.name);
        //    Debug.Log(mode);
        //}

        private void Update()
        {
            //  Test F1 - Fade out || F2 - Fade in
            if (Input.GetKeyDown(KeyCode.F1)) Fade(true, 1.25f); 
            if (Input.GetKeyDown(KeyCode.F2)) Fade(false, 3f);
            //  Test End

            if (!_inTransition) return;

            _transition += (_showing) ? Time.deltaTime * (1 / _duration) : -Time.deltaTime * (1 / _duration);
            FadeImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, _transition);
            if (FadeImage.color.a < 0.001f)
                FadeImage.enabled = false;

            if (_transition > 1 || _transition < 0)
                _inTransition = false;
        }
    }
}

