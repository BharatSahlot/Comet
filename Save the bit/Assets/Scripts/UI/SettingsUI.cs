using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Button keyboardBtn;
        [SerializeField] private Button joystickBtn;

        [SerializeField] private Sprite keyboardOnSprite;
        [SerializeField] private Sprite keyboardOffSprite;
        
        [SerializeField] private Sprite joystickOnSprite;
        [SerializeField] private Sprite joystickOffSprite;

        [SerializeField] private DataManager dataManager;

        private void Awake()
        {
            if (dataManager.InputMode == InputMode.Joystick)
            {
                keyboardBtn.image.sprite = keyboardOffSprite;
                joystickBtn.image.sprite = joystickOnSprite;
            }
            else
            {
                keyboardBtn.image.sprite = keyboardOnSprite;
                joystickBtn.image.sprite = joystickOffSprite;
            }
            
            keyboardBtn.onClick.AddListener(() =>
            {
                keyboardBtn.image.sprite = keyboardOnSprite;
                joystickBtn.image.sprite = joystickOffSprite;

                dataManager.InputMode = InputMode.Keyboard;
            });
            
            joystickBtn.onClick.AddListener(() =>
            {
                keyboardBtn.image.sprite = keyboardOffSprite;
                joystickBtn.image.sprite = joystickOnSprite;

                dataManager.InputMode = InputMode.Joystick;
            });

            volumeSlider.value = dataManager.EffectsVolume;
            volumeSlider.onValueChanged.AddListener((volume) =>
            {
                dataManager.EffectsVolume = Mathf.RoundToInt(volume * 100);
                AudioListener.volume = volume;
            });
        }
    }
}