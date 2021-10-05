﻿using System;
using System.Collections;
using Game.Player;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class CoinManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private Coin coinPrefab;
        [SerializeField] private float spawnDelay;
        [SerializeField] private float spawnDistance;

        internal PlayerController PlayerController;

        private int _coinsCollected;
        private Pool<Coin> _pool;

        private void Start()
        {
            _pool = new Pool<Coin>(coinPrefab, 5);
            PlayerController.CoinHit += () =>
            {
                _coinsCollected++;
                coinText.SetText($"{_coinsCollected}");
            };
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                var position = PlayerController.transform.position.xy() + Random.insideUnitCircle.normalized * spawnDistance;
                var coin = _pool.Borrow(false);
                coin.transform.position = position;
                coin.Pool = _pool;
                coin.gameObject.SetActive(true);
                coin.Player = PlayerController.gameObject;
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}