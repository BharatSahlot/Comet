﻿using System;
using System.Globalization;
using Cinemachine;
using Game.Data;
using Game.Enemy;
using Game.Player;
using UI;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private DataManager dataManager;
        [SerializeField] private GameData gameData;

        [SerializeField] internal ModificationController modificationController;
        [SerializeField] internal InputManager inputManager;

        [SerializeField] internal EnemySpawner enemySpawner;
        
        // [SerializeField] internal MissileSpawner[] missileSpawners;
        [SerializeField] internal CosmicRaySpawner cosmicRaySpawner;
        [SerializeField] internal CoinManager coinManager;

        [SerializeField] internal CinemachineVirtualCamera cinemachine;

        [SerializeField] internal SlowMotionEffect slowMotionEffect;

        [SerializeField] internal ResponsiveGameUIManager uiManager;
        [SerializeField] internal Explosion deadExplosion;

        internal PlayerController PlayerController;
        internal Player.Shield Shield;

        private float _playStartTime;

        private bool _playing = true;
        private int _currentPlayCoins = 0;

        private void Awake()
        {
            dataManager.gameData = gameData;
            var playerPrefab = gameData.planes[dataManager.EquippedPlane].planePrefab;
            var shieldPrefab = gameData.shields[dataManager.EquippedShield].shieldPrefab;
            
            PlayerController = Instantiate(playerPrefab);
            PlayerController.InputManager = inputManager;
            PlayerController.DeadExplosion = deadExplosion;
            
            Shield = Instantiate(shieldPrefab);

            var shieldGo = Shield.gameObject;
            var playerGo = PlayerController.gameObject;
            
            Shield.Player = playerGo;
            
            modificationController.PlayerController = PlayerController;
            modificationController.Shield = shieldGo;

            enemySpawner.Player = playerGo;
            
            cosmicRaySpawner.Player = PlayerController;
            cosmicRaySpawner.EnemySpawner = enemySpawner;
            
            coinManager.PlayerController = PlayerController;
            coinManager.DataManager = dataManager;

            var playerTransform = PlayerController.transform;
            cinemachine.Follow = playerTransform;
            cinemachine.LookAt = playerTransform;

            slowMotionEffect.PlayerController = PlayerController;

            // if there are not zero that means the game crashed midway, find better way to display this
            dataManager.CoinsCollected = 0;
            dataManager.TimeCoins = 0;
            
            PlayerController.MissileHit += (controller, _) =>
            {
                _playing = false;
                Destroy(PlayerController.gameObject);
                Destroy(Shield.gameObject);

                var ui = GameObject.FindObjectsOfType<Canvas>();
                foreach (var canvas in ui)
                {
                    canvas.gameObject.SetActive(false);
                }

                dataManager.TimeCoins = Mathf.FloorToInt(Time.time - _playStartTime);
                _currentPlayCoins = dataManager.CoinsCollected + dataManager.TimeCoins;
                // dataManager.Coins += dataManager.CoinsCollected + dataManager.TimeCoins;
                dataManager.Coins += _currentPlayCoins;
                uiManager.DeadMenu.Display(dataManager.TimeCoins, dataManager.CoinsCollected, dataManager.Coins);
                
                dataManager.CoinsCollected = 0;
                dataManager.TimeCoins = 0;
                PlayerController = null;
                Shield = null;
                CrazySDK.Instance.GameplayStop();
            };
        }

        private void Start()
        {
            inputManager.SetInputMode(dataManager.InputMode);
            _playing = true;
            _playStartTime = Time.time;
        }

        private void Update()
        {
            if (!_playing) return;
            
            var elapsed = Time.time - _playStartTime;
            uiManager.GameUI.SetTime(TimeSpan.FromSeconds(elapsed).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture));
        }

        public void StartMissileSpawner()
        {
            enemySpawner.enabled = true;
        }

        public void StartRaySpawner()
        {
            cosmicRaySpawner.StartSpawner();
        }

        [Preserve]
        public void PlayRewardedAd()
        {
            CrazyAds.Instance.beginAdBreakRewarded(OnAdSuccess, OnAdFail);
        }

        private void OnAdSuccess()
        {
            Debug.Log("Ad Playing");
            dataManager.Coins += _currentPlayCoins;
            _currentPlayCoins = 0;
            uiManager.DeadMenu.UpdateTotalCoins(dataManager.Coins);
            uiManager.DeadMenu.rewardButton.gameObject.SetActive(false);
        }

        private void OnAdFail()
        {
            uiManager.DeadMenu.rewardedAdFailPopup.SetActive(true);
        }

        private void OnDestroy()
        {
            dataManager = null;
            gameData = null;
            modificationController = null;
            inputManager = null;
            // missileSpawners = null;
            enemySpawner = null;
            cosmicRaySpawner = null;
            coinManager = null;
            cinemachine = null;
            slowMotionEffect = null;
            uiManager = null;
            PlayerController = null;
            Shield = null;
        }
    }
}