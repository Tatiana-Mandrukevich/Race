using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private CoinStatisticsUI _coinStatisticsUI;
    [SerializeField] private LossScreen _lossScreen;
    [SerializeField] private YandexGameService YandexGameService;

    private void Start()
    {
        _coinStatisticsUI.gameObject.SetActive(true);
        _lossScreen.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SpeedManager.OnLost += OnSpeedManagerIsLost;
        LossScreen.OnSecondLifeChoice += OnSecondLifeChoice;
        YandexGameService.OnRewardAdv += OnRewardAdv;
    }

    private void OnDisable()
    {
        SpeedManager.OnLost -= OnSpeedManagerIsLost;
        LossScreen.OnSecondLifeChoice -= OnSecondLifeChoice;
        YandexGameService.OnRewardAdv -= OnRewardAdv;
    }

    private void OnRewardAdv(string rewardID)
    {
        if (rewardID == "SecondLife")
        {
            OnSecondLifeChoice(true);
        }
    }

    private void OnSpeedManagerIsLost(bool isLost)
    {
        if (isLost)
        {
            _coinStatisticsUI.gameObject.SetActive(false);
            _lossScreen.gameObject.SetActive(true);
        }
    }
    
    private void OnSecondLifeChoice(bool isSecondLifeChoice)
    {
        if (isSecondLifeChoice)
        {
            _lossScreen.gameObject.SetActive(false);
            _coinStatisticsUI.gameObject.SetActive(true);
            
            Car car = FindObjectOfType<Car>();
            if (car != null && car._chunkManager != null)
            {
                car._chunkManager.SpeedManager.ResetIsLost();
            }
        }
    }
}