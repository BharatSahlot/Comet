using System;
using Game.Data;
using TMPro;
using Unity.VisualScripting;
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
        [SerializeField] private TextMeshProUGUI coinText;

        [Space]
        [SerializeField] private Button replayButton;
        [SerializeField] private Button rewardButton;
        
        private void Awake()
        {
            replayButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
            });
        }

        public void Display(int timeCoins, int coinsCollected, int totalCoins)
        {
            int total = timeCoins + coinsCollected;
            
            baseCoinText.SetText($"+{timeCoins}");
            bonusCoinText.SetText($"+{coinsCollected}");
            totalCoinText.SetText($"{total}");
            
            coinText.SetText($"{totalCoins}");
            
            rewardButton.gameObject.SetActive(true);
            
            gameCanvas.SetActive(false);
            gameObject.SetActive(true);
        }

        public void UpdateTotalCoins(int totalCoins)
        {
            coinText.SetText($"{totalCoins}");
        }
    }
}