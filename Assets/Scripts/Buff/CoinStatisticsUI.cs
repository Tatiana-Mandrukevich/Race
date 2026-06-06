using System;
using DefaultNamespace.Buff;
using TMPro;
using UnityEngine;
using Zenject;

public class CoinStatisticsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsNumber;
    
    [Inject] private CoinController _coinController;

    private void OnEnable()
    {
        _coinController.OnCoinsChanged += ShowActualTotalCoins;
        ShowActualTotalCoins(_coinController.TotalCoins);
    }

    private void OnDisable()
    {
        _coinController.OnCoinsChanged -= ShowActualTotalCoins;
    }

    private void ShowActualTotalCoins(int coins)
    {
        _coinsNumber.text = coins.ToString();
    }
}