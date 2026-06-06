using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private CoinStatisticsUI _coinStatisticsUI;
    [SerializeField] private LossScreen _lossScreen;

    private void Start()
    {
        _coinStatisticsUI.gameObject.SetActive(true);
        _lossScreen.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SpeedManager.OnLost += OnSpeedManagerIsLost;
    }

    private void OnDisable()
    {
        SpeedManager.OnLost -= OnSpeedManagerIsLost;
    }

    private void OnSpeedManagerIsLost(bool isLost)
    {
        if (isLost)
        {
            _coinStatisticsUI.gameObject.SetActive(false);
            _lossScreen.gameObject.SetActive(true);
        }
    }
}