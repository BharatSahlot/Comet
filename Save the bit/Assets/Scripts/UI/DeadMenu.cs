using System;
using Game.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class DeadMenu : MonoBehaviour
    {
        [SerializeField] private GameObject gameCanvas;
        
        [Space]
        [SerializeField] private TextMeshProUGUI baseCoinText;
        [SerializeField] private TextMeshProUGUI bonusCoinText;
        [SerializeField] private TextMeshProUGUI totalCoinText;

        [Space]
        [SerializeField] private Button replayButton;
        
        internal DataManager DataManager;

        private void Awake()
        {
            replayButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
            });
        }

        public void Display()
        {
            baseCoinText.SetText("+0");
            bonusCoinText.SetText($"+{DataManager.Coins}");
            totalCoinText.SetText($"{DataManager.Coins}");
            
            gameObject.SetActive(true);
        }
    }
}