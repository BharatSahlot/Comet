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
            int baseCoins = Mathf.FloorToInt(DataManager.BaseCoins);
            int coinsCollected = DataManager.CoinsCollected;
            int total = baseCoins + coinsCollected;
            
            baseCoinText.SetText($"+{baseCoins}");
            bonusCoinText.SetText($"+{coinsCollected}");
            totalCoinText.SetText($"{total}");
            
            gameCanvas.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}