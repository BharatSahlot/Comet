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

        
        internal PlayerController PlayerController;
        internal Shield Shield;

        private void Awake()
        {
            PlayerController = Instantiate(playerPrefab);
            PlayerController.InputManager = inputManager;
            
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
            PlayerController.MissileHit += (controller, _) => deadMenu.Display();
        }
    }
}