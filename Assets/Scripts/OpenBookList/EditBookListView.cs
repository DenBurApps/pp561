using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class EditBookListView : MonoBehaviour
{
    [SerializeField] private Color _unselectedButtonColor;
    [SerializeField] private Color _unselectedTextColor;
    [SerializeField] private Color _selectedButtonColor;
    [SerializeField] private Color _selectedTextColor;

    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private TMP_Text _saveButtonText;
    [SerializeField] private TMP_InputField _bookListTitle;
    [SerializeField] private TMP_InputField _bookListDescription;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action BackButtonClicked;
    public event Action SaveButtonClicked;
    public event Action DeleteButtonClicked;
    public event Action<string> BookTitleInputed;
    public event Action<string> BookDescriptionInputed; 
        
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _saveButton.onClick.AddListener(OnSaveButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _bookListTitle.onValueChanged.AddListener(OnBookTitleInputed);
        _bookListDescription.onValueChanged.AddListener(OnBookDescriptionInputed);
        _deleteButton.onClick.AddListener(OnDeleteButtonClicked);
    }

    private void OnDisable()
    {
        _saveButton.onClick.RemoveListener(OnSaveButtonClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _bookListTitle.onValueChanged.RemoveListener(OnBookTitleInputed);
        _bookListDescription.onValueChanged.RemoveListener(OnBookDescriptionInputed);
        _deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }
    
    public void SetSaveButtonInteractable(bool status)
    {
        _saveButton.enabled = status;

        if (_saveButton.enabled)
        {
            _saveButton.image.color = _selectedButtonColor;
            _saveButtonText.color = _selectedTextColor;
        }
        else
        {
            _saveButton.image.color = _unselectedButtonColor;
            _saveButtonText.color = _unselectedTextColor;
        }
    }

    public void SetTitleText(string text)
    {
        _bookListTitle.text = text;
    }

    public void SetDescriptionText(string text)
    {
        _bookListDescription.text = text;
    }

    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
    private void OnSaveButtonClicked() => SaveButtonClicked?.Invoke();
    private void OnBookTitleInputed(string title) => BookTitleInputed?.Invoke(title);
    private void OnBookDescriptionInputed(string description) => BookDescriptionInputed?.Invoke(description);
    private void OnDeleteButtonClicked() => DeleteButtonClicked?.Invoke();
}
