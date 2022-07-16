using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Game.Data
{
    public class DataManager : MonoBehaviour
    {
        private static string FileName => $"{Application.persistentDataPath}/GameData.dat";

        private SaveData _saveData;

        [SerializeField] internal GameData gameData;

        public int Coins
        {
            get => _saveData.coins;
            set
            {
                _saveData.coins = value;
                Save(_saveData);
            }
        }

        public int CoinsCollected
        {
            get => _saveData.coinsCollected;
            set
            {
                _saveData.coinsCollected = value;
                Save(_saveData);
            }
        }
        
        public int TimeCoins
        {
            get => _saveData.timeCoins;
            set
            {
                _saveData.timeCoins = value;
                Save(_saveData);
            }
        }

        public int EquippedPlane
        {
            get => _saveData.planeIndex;
            set
            {
                _saveData.planeIndex = value;
                Save(_saveData);
            }
        }
        
        public int EquippedShield
        {
            get => _saveData.shieldIndex;
            set
            {
                _saveData.shieldIndex = value;
                Save(_saveData);
            }
        }

        public InputMode InputMode
        {
            get => _saveData.inputMode;
            set
            {
                _saveData.inputMode = value;
                Save(_saveData);
            }
        }
        
        public int EffectsVolume
        {
            get => _saveData.effectsVolume;
            set
            {
                _saveData.effectsVolume = value;
                Save(_saveData);
            }
        }

        public bool HasBoughtPlane(int i)
        {
            return _saveData.planesBought.Contains(i);
        }
        
        public bool HasBoughtShield(int i)
        {
            return _saveData.shieldsBought.Contains(i);
        }

        public bool TryBuyPlane(int index, bool equip = true)
        {
            if (HasBoughtPlane(index))
            {
                if(equip) EquippedPlane = index;
                Save(_saveData);
                return true;
            }
            
            var cost = gameData.planes[index].cost;
            if (cost > Coins) return false;

            _saveData.coins -= cost;
            _saveData.planesBought.Add(index);

            if (equip)
            {
                _saveData.planeIndex = index;
            }
            Save(_saveData);
            return true;
        }
        
        public bool TryBuyShield(int index, bool equip = true)
        {
            if (HasBoughtShield(index))
            {
                if(equip) EquippedShield = index;
                Save(_saveData);
                return true;
            }
            
            var cost = gameData.shields[index].cost;
            if (cost > Coins) return false;

            _saveData.coins -= cost;
            _saveData.shieldsBought.Add(index);

            if (equip)
            {
                _saveData.shieldIndex = index;
            }
            Save(_saveData);
            return true;
        }
        
        private void Awake()
        {
            _saveData = Load();
        }
        
        public static void Save(SaveData data)
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
            
            writer.Write(data.timeCoins);
            writer.Write(data.coinsCollected);
            writer.Write((int)data.inputMode);
            
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                SyncFiles();
            }
        }

        public static SaveData Load()
        {
            if (!File.Exists(FileName)) return new SaveData()
            {
                planesBought = new List<int>() { 0 },
                shieldsBought = new List<int>() { 0 }
            };
            
            using var reader = new BinaryReader(File.Open(FileName, FileMode.Open, FileAccess.Read));
            
            // ReSharper disable once UseObjectOrCollectionInitializer
            var data = new SaveData();
            try
            {
                data.coins = reader.ReadInt32();
                #if UNITY_EDITOR
                data.coins = 10000;
                #endif
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
                data.timeCoins = reader.ReadInt32();
                data.inputMode = (InputMode)reader.ReadInt32();
            }
            catch
            {
                data.planesBought = new List<int>() { 0 };
                data.shieldsBought = new List<int>() { 0 };
                data.inputMode = InputMode.Joystick;
            }

            return data;
        }
        
        [DllImport("__Internal")]
        private static extern void SyncFiles();

        [DllImport("__Internal")]
        private static extern void WindowAlert(string message); 
    }
}