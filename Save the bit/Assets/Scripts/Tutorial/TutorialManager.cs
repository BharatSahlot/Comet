using System;
using Game;
using UnityEngine;

namespace Tutorial
{
    public abstract class TutorialStep : MonoBehaviour
    {
        [SerializeField] protected GameManager gameManager;
        
        public Action OnEnd { get; set; }
        public abstract void Begin();
    }
    
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private TutorialStep[] steps;

        private int _currentStep = 0;

        private void Awake()
        {
            foreach (var step in steps)
            {
                step.OnEnd += () =>
                {
                    if (_currentStep + 1 >= steps.Length) return;
                    steps[_currentStep + 1].Begin();
                    _currentStep++;
                };
            }
        }

        private void Start()
        {
            steps[0].Begin();
        }
    }
}
