using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Buff;
using DG.Tweening;
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
    [SerializeField] private Button _secondLifeButton;
    
    public static event Action<bool> OnSecondLifeChoice;
    
    [Inject] private CoinController _coinController;

    private void Start()
    {
        ShowActualTotalCoins();
    }
    
    private void OnEnable()
    {
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        _retryButton.onClick.AddListener(OnRetryButtonClick);
        _secondLifeButton.onClick.AddListener(OnSecondLifeButtonClick);
    }
    
    private void OnDisable()
    {
        _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClick);
        _retryButton.onClick.RemoveListener(OnRetryButtonClick);
        _secondLifeButton.onClick.RemoveListener(OnSecondLifeButtonClick);
    }

    private void ShowActualTotalCoins()
    {
        int coins = _coinController.TotalCoins;
        _coinsNumber.text = coins.ToString();
    }

    private void OnMainMenuButtonClick()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(0);
        _coinController.ResetCoins();
    }
    
    private void OnRetryButtonClick()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(1);
        _coinController.ResetCoins();
    }
    
    private void OnSecondLifeButtonClick()
    {
        OnSecondLifeChoice?.Invoke(true);
    }
}
