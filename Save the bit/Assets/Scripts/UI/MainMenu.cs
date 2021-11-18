using System;
using Game.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private DataManager dataManager;
        [SerializeField] private TextMeshProUGUI coinText;

        private void Start()
        {
            coinText.SetText($"{dataManager.Coins}");
        }
    }
}