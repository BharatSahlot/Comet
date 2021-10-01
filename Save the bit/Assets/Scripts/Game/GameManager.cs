using Cinemachine;
using Game.Missiles;
using Game.Player;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private Shield shieldPrefab;

        [SerializeField] internal ModificationController modificationController;
        
        [SerializeField] internal MissileSpawner missileSpawner;
        [SerializeField] internal CosmicRaySpawner cosmicRaySpawner;

        [SerializeField] internal CinemachineVirtualCamera cinemachine;
        
        internal PlayerController PlayerController;
        internal Shield Shield;

        private void Awake()
        {
            PlayerController = Instantiate(playerPrefab);
            Shield = Instantiate(shieldPrefab);

            var shieldGo = Shield.gameObject;
            var playerGo = PlayerController.gameObject;
            
            PlayerController.Shield = shieldGo;
            Shield.Player = playerGo;
            
            modificationController.PlayerController = PlayerController;
            modificationController.Shield = shieldGo;

            missileSpawner.Player = playerGo;
            cosmicRaySpawner.Player = PlayerController;

            var playerTransform = PlayerController.transform;
            cinemachine.Follow = playerTransform;
            cinemachine.LookAt = playerTransform;
        }
    }
}