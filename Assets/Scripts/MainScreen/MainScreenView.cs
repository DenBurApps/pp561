using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreenView : MonoBehaviour
{
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _addBookButton;
    [SerializeField] private Button _addBookListButton;
    [SerializeField] private TMP_InputField _searchInput;
    [SerializeField] private GameObject _emptyPlane;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action SettingsButtonClicked;
    public event Action AddBookButtonClicked;
    public event Action AddBookListButtonClicked;
    public event Action<string> SearchTextInputed;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _addBookButton.onClick.AddListener(OnAddBookButtonClicked);
        _addBookListButton.onClick.AddListener(OnAddBookListButtonClicked);
        _searchInput.onValueChanged.AddListener(OnSearchTextInputed);
    }

    private void OnDisable()
    {
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
        _addBookButton.onClick.RemoveListener(OnAddBookButtonClicked);
        _addBookListButton.onClick.RemoveListener(OnAddBookListButtonClicked);
        _searchInput.onValueChanged.RemoveListener(OnSearchTextInputed);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void SetTransperent()
    {
        _screenVisabilityHandler.SetTransperent();
    }

    public void ToggleEmptyPlane(bool status)
    {
        _emptyPlane.gameObject.SetActive(status);
    }

    private void OnSettingsButtonClicked() => SettingsButtonClicked?.Invoke();
    private void OnAddBookButtonClicked() => AddBookButtonClicked?.Invoke();
    private void OnAddBookListButtonClicked() => AddBookListButtonClicked?.Invoke();
    private void OnSearchTextInputed(string searchText) => SearchTextInputed?.Invoke(searchText);
}
