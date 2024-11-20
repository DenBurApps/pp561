using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class AddBooksMainScreenView : MonoBehaviour
{
    [SerializeField] private Button _addBookListButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private GameObject _emptyBookListPlane;

    public event Action AddBookListClicked;
    public event Action BackButtonClicked;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _addBookListButton.onClick.AddListener(OnAddBookClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _addBookListButton.onClick.RemoveListener(OnAddBookClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void ToggleEmptyBookListPlane(bool status)
    {
        _emptyBookListPlane.SetActive(status);
    }

    private void OnAddBookClicked() => AddBookListClicked?.Invoke();
    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
}
