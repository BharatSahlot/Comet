using System;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class MovementTutorial : TutorialStep
    {
        [System.Serializable]
        private class UI
        {
            public GameObject root;
            public TextMeshProUGUI text;
            public Image screen;
            public Image arrow;
        }
        
        [SerializeField] private UI portraitUI;
        [SerializeField] private UI landscapeUI;
        [SerializeField] private float fadeDur = 1;

        [Space]
        [SerializeField] private InputManager inputManager;

        private static bool IsPortrait => Screen.height >= Screen.width;
        
        private bool _isActive;
        private bool _isEnding;
        private float _timeSinceEndStart;

        public override void Begin()
        {
            _isActive = true;
            Time.timeScale = 0.1f; // slow the time while in tutorial

            if (IsPortrait)
            {
                portraitUI.root.SetActive(true);
            }
            else
            {
                landscapeUI.root.SetActive(true);
            }
        }

        private void Update()
        {
            if (!_isActive) return;

            if (_isEnding)
            {
                _timeSinceEndStart += Time.unscaledDeltaTime;
                if (_timeSinceEndStart >= fadeDur)
                {
                    End();
                    return;
                }

                float alpha = 1 - (_timeSinceEndStart / fadeDur);

                UI ui = IsPortrait ? portraitUI : landscapeUI;
                Color color = new Color(1, 1, 1, alpha);
                ui.arrow.color *= color;
                ui.screen.color *= color;
                ui.text.color *= color;
            }
            else
            {
                // end and move to next tutorial stage as soon as player uses the joystick
                if (inputManager.GetMoveDirection() != Vector2.zero) _isEnding = true;
            }
        }

        private void End()
        {
            _isActive = _isEnding = false;
            (IsPortrait ? portraitUI : landscapeUI).root.SetActive(false);
            OnEnd.Invoke();
        }
    }
}
