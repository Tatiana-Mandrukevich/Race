using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Buff;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class LossScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsNumber;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _retryButton;
    
    [Inject] private CoinController _coinController;

    private void Start()
    {
        ShowActualTotalCoins();
    }
    
    private void OnEnable()
    {
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        _retryButton.onClick.AddListener(OnRetryButtonClick);
    }
    
    private void OnDisable()
    {
        _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClick);
        _retryButton.onClick.RemoveListener(OnRetryButtonClick);
    }

    private void ShowActualTotalCoins()
    {
        int coins = _coinController.TotalCoins;
        _coinsNumber.text = coins.ToString();
    }

    private void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("MainScreen");
        _coinController.ResetCoins();
    }
    
    private void OnRetryButtonClick()
    {
        SceneManager.LoadScene("GameScene");
        _coinController.ResetCoins();
    }
}
