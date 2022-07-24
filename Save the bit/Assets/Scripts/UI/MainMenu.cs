using Game.Data;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private DataManager dataManager;
        [SerializeField] private TextMeshProUGUI coinText;

        private void Start()
        {
            Debug.Log(dataManager.EffectsVolume);
            AudioListener.volume = dataManager.EffectsVolume;
            coinText.SetText($"{dataManager.Coins}");
        }
    }
}