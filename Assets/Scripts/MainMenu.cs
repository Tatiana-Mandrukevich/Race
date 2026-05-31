using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClick);
    }
    
    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClick);
    }

    private void OnPlayButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }
}