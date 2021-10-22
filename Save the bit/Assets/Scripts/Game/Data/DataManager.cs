using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Game.Data
{
    public class DataManager : MonoBehaviour
    {
        private static string FileName => $"{Application.persistentDataPath}/GameData.dat";

        private GameData _gameData;

        public int Coins
        {
            get => _gameData.coins;
            set
            {
                _gameData.coins = value;
                Save(_gameData);
            }
        }

        public int CoinsCollected
        {
            get => _gameData.coinsCollected;
            set
            {
                _gameData.coinsCollected = value;
                Save(_gameData);
            }
        }
        
        public int BaseCoins
        {
            get => _gameData.baseCoins;
            set
            {
                _gameData.baseCoins = value;
                Save(_gameData);
            }
        }
        
        private void Start()
        {
            _gameData = Load();
        }
        
        public static void Save(GameData data)
        {
            using var writer = new BinaryWriter(File.Open(FileName, FileMode.Create, FileAccess.Write));
            
            writer.Write(data.coins);
            writer.Write(data.planeIndex);
            writer.Write(data.shieldIndex);

            if (data.planesBought != null)
            {
                writer.Write(data.planesBought.Count);
                foreach (var i in data.planesBought) writer.Write(i);
            } else writer.Write(0);

            if (data.shieldsBought != null)
            {
                writer.Write(data.shieldsBought.Count);
                foreach (var i in data.shieldsBought) writer.Write(i);
            } else writer.Write(0);
            
            writer.Write(data.baseCoins);
            writer.Write(data.coinsCollected);
            
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                SyncFiles();
            }
        }

        public static GameData Load()
        {
            if (!File.Exists(FileName)) return new GameData();
            
            using var reader = new BinaryReader(File.Open(FileName, FileMode.Open, FileAccess.Read));
            
            // ReSharper disable once UseObjectOrCollectionInitializer
            var data = new GameData();
            try
            {
                data.coins = reader.ReadInt32();
                data.planeIndex = reader.ReadInt32();
                data.shieldIndex = reader.ReadInt32();

                var count = reader.ReadInt32();
                data.planesBought = new List<int>(count);
                for (var i = 0; i < count; i++)
                {
                    data.planesBought.Add(reader.ReadInt32());
                }

                count = reader.ReadInt32();
                data.shieldsBought = new List<int>(count);
                for (var i = 0; i < count; i++)
                {
                    data.shieldsBought.Add(reader.ReadInt32());
                }

                data.coinsCollected = reader.ReadInt32();
                data.baseCoins = reader.ReadInt32();
            }
            catch
            {
                data.planesBought = new List<int>();
                data.shieldsBought = new List<int>();
            }

            return data;
        }
        
        [DllImport("__Internal")]
        private static extern void SyncFiles();

        [DllImport("__Internal")]
        private static extern void WindowAlert(string message); 
    }
}