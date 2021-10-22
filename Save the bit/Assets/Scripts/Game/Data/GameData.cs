using System.Collections.Generic;

namespace Game.Data
{
    [System.Serializable]
    public struct GameData
    {
        public int coins;
        
        public int planeIndex;
        public int shieldIndex;

        public List<int> planesBought;
        public List<int> shieldsBought;

        // last / current play data
        public int coinsCollected;
        public int baseCoins;
    }
}