using System;
using UnityEngine;

public class ScreenStateManager : MonoBehaviour
{
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private AddBookFirstScreen _addBookFirstScreen;
    [SerializeField] private AddBookSecondScreen _addBookSecondScreen;
    [SerializeField] private OpenBookScreen _openBookScreen;
    [SerializeField] private AddBookListMainScreen _addBookListMainScreen;
    [SerializeField] private AddBookListScreen _addBookListScreen;
    [SerializeField] private SettingsScreen _settingsScreen;

    public event Action MainScreenOpen;
    public event Action AddBookFirstScreenOpen;
    public event Action AddBooksListScreenOpen;
    public event Action AddBookListMainScreenOpen;
    public event Action SettingsScreenOpen;
    public event Action SettingsDisabled;
    public event Action SettingsOpened; 

    private void OnEnable()
    {
        _mainScreen.AddBookClicked += OnAddBookOpen;
        _mainScreen.AddBookListClicked += OnAddBooksListMainScreenOpen;
        _mainScreen.SettingsClicked += OnSettingsScreenOpen;
        
        _addBookFirstScreen.BackButtonClicked += OnMainScreenOpen;
        
        _addBookSecondScreen.ReturnToMainMenu += OnMainScreenOpen;
        
        _openBookScreen.BackButtonClicked += OnMainScreenOpen;
        
        _addBookListMainScreen.AddBookListClicked += OnAddBooksListOpen;
        _addBookListMainScreen.BackButtonClicked += OnMainScreenOpen;
        
        _addBookListScreen.BackButtonClicked += OnAddBooksListMainScreenOpen;

        _settingsScreen.BackButtonClicked += OnMainScreenOpen;
        _settingsScreen.SettingsDisabled += OnSettingsDisabled;
        _settingsScreen.SettingsOpened += OnSettingsOpened;
    }

    private void OnDisable()
    {
        _mainScreen.AddBookClicked -= OnAddBookOpen;
        _mainScreen.AddBookListClicked -= OnAddBooksListMainScreenOpen;
        _mainScreen.SettingsClicked -= OnSettingsScreenOpen;
        
        _addBookFirstScreen.BackButtonClicked -= OnMainScreenOpen;
        
        _addBookSecondScreen.ReturnToMainMenu -= OnMainScreenOpen;
        
        _openBookScreen.BackButtonClicked -= OnMainScreenOpen;
        
        _addBookListMainScreen.AddBookListClicked -= OnAddBooksListOpen;
        _addBookListMainScreen.BackButtonClicked -= OnMainScreenOpen;
        
        _addBookListScreen.BackButtonClicked -= OnAddBooksListMainScreenOpen;
        
        _settingsScreen.BackButtonClicked -= OnMainScreenOpen;
        _settingsScreen.SettingsDisabled -= OnSettingsDisabled;
        _settingsScreen.SettingsOpened -= OnSettingsOpened;
    }

    private void OnAddBookOpen() => AddBookFirstScreenOpen?.Invoke();
    private void OnMainScreenOpen() => MainScreenOpen?.Invoke();
    private void OnAddBooksListOpen() => AddBooksListScreenOpen?.Invoke();
    private void OnAddBooksListMainScreenOpen() => AddBookListMainScreenOpen?.Invoke();
    private void OnSettingsScreenOpen() => SettingsScreenOpen?.Invoke();
    private void OnSettingsDisabled() => SettingsDisabled?.Invoke();
    private void OnSettingsOpened() => SettingsOpened?.Invoke();
}
