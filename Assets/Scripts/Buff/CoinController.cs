using System;

namespace DefaultNamespace.Buff
{
    public class CoinController
    {
        private int _totalCoins;
        
        public event Action<int> OnCoinsChanged;
        
        public int TotalCoins => _totalCoins;

        public void AddCoin()
        {
            _totalCoins++;
            OnCoinsChanged?.Invoke(_totalCoins);
        }
        
        public void ResetCoins()
        {
            _totalCoins = 0;
            OnCoinsChanged?.Invoke(_totalCoins);
        }
    }
}