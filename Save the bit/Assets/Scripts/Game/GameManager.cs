using System;
using Cinemachine;
using Game.Data;
using Game.Missiles;
using Game.Player;
using UI;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private DataManager dataManager;
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private Shield shieldPrefab;

        [SerializeField] internal ModificationController modificationController;
        [SerializeField] internal InputManager inputManager;
        
        [SerializeField] internal MissileSpawner missileSpawner;
        [SerializeField] internal CosmicRaySpawner cosmicRaySpawner;
        [SerializeField] internal CoinManager coinManager;

        [SerializeField] internal CinemachineVirtualCamera cinemachine;

        [SerializeField] internal SlowMotionEffect slowMotionEffect;

        [SerializeField] internal DeadMenu deadMenu;
        [SerializeField] internal Explosion deadExplosion;

        internal PlayerController PlayerController;
        internal Shield Shield;

        private float _playStartTime;

        private void Awake()
        {
            PlayerController = Instantiate(playerPrefab);
            PlayerController.InputManager = inputManager;
            PlayerController.DeadExplosion = deadExplosion;
            
            Shield = Instantiate(shieldPrefab);

            var shieldGo = Shield.gameObject;
            var playerGo = PlayerController.gameObject;
            
            Shield.Player = playerGo;
            
            modificationController.PlayerController = PlayerController;
            modificationController.Shield = shieldGo;

            missileSpawner.Player = playerGo;
            cosmicRaySpawner.Player = PlayerController;
            
            coinManager.PlayerController = PlayerController;
            coinManager.DataManager = dataManager;

            var playerTransform = PlayerController.transform;
            cinemachine.Follow = playerTransform;
            cinemachine.LookAt = playerTransform;

            slowMotionEffect.PlayerController = PlayerController;

            deadMenu.DataManager = dataManager;

            // if there are not zero that means the game crashed midway, find better way to display this
            dataManager.CoinsCollected = 0;
            dataManager.BaseCoins = 0;
            
            PlayerController.MissileHit += (controller, _) =>
            {
                Destroy(PlayerController.gameObject);
                Destroy(Shield.gameObject);

                dataManager.BaseCoins = Mathf.FloorToInt(Time.time - _playStartTime);
                dataManager.Coins += dataManager.CoinsCollected + dataManager.BaseCoins;
                deadMenu.Display();
                
                dataManager.CoinsCollected = 0;
                dataManager.BaseCoins = 0;
                PlayerController = null;
                Shield = null;
            };
        }

        private void Start()
        {
            _playStartTime = Time.time;
        }

        private void OnDestroy()
        {
            dataManager = null;
            playerPrefab = null;
            shieldPrefab = null;
            modificationController = null;
            inputManager = null;
            missileSpawner = null;
            cosmicRaySpawner = null;
            coinManager = null;
            cinemachine = null;
            slowMotionEffect = null;
            deadMenu = null;
            PlayerController = null;
            Shield = null;
        }
    }
}