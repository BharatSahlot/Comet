using System;
using UnityEngine;

namespace Tutorial
{
    public abstract class TutorialStep : MonoBehaviour
    {
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
                    if (_currentStep >= steps.Length) return;
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
