using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class AddBookFirstScreenView : MonoBehaviour
{
    [SerializeField] private Color _unselectedButtonColor;
    [SerializeField] private Color _unselectedTextColor;
    [SerializeField] private Color _selectedButtonColor;
    [SerializeField] private Color _selectedTextColor;

    [SerializeField] private Button _backButton;
    [SerializeField] private Button _addPhotoButton;
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private TMP_InputField _bookTitleInputField;
    [SerializeField] private TMP_InputField _authorInputField;
    [SerializeField] private TMP_InputField _yearOfPublicationInputField;
    [SerializeField] private TMP_InputField _descriptionInputField;
    [SerializeField] private Button _nextButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action AddPhotoClicked;
    public event Action NextButtonClicked;
    public event Action BackButtonClicked;
    public event Action<string> BookTitleInputed;
    public event Action<string> AuthorInputed;
    public event Action<string> YearOfPublicationInputed;
    public event Action<string> DescriptionInputed;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _addPhotoButton.onClick.AddListener(OnPhotoClicked);
        _bookTitleInputField.onValueChanged.AddListener(OnBookTitleInputed);
        _authorInputField.onValueChanged.AddListener(OnAuthorInputed);
        _yearOfPublicationInputField.onValueChanged.AddListener(OnYearOfPublicationInputed);
        _descriptionInputField.onValueChanged.AddListener(OnDescriptionInputed);
        _nextButton.onClick.AddListener(OnNextButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnDisable()
    {
        _addPhotoButton.onClick.RemoveListener(OnPhotoClicked);
        _bookTitleInputField.onValueChanged.RemoveListener(OnBookTitleInputed);
        _authorInputField.onValueChanged.RemoveListener(OnAuthorInputed);
        _yearOfPublicationInputField.onValueChanged.RemoveListener(OnYearOfPublicationInputed);
        _descriptionInputField.onValueChanged.RemoveListener(OnDescriptionInputed);
        _nextButton.onClick.RemoveListener(OnNextButtonClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void SetNextButtonInteractable(bool status)
    {
        _nextButton.enabled = status;

        if (_nextButton.enabled)
        {
            _nextButton.image.color = _selectedButtonColor;
            _buttonText.color = _selectedTextColor;
        }
        else
        {
            _nextButton.image.color = _unselectedButtonColor;
            _buttonText.color = _unselectedTextColor;
        }
    }

    public void ToggleAddPhotoButton(bool status)
    {
        _addPhotoButton.interactable = status;
    }

    public void SetTitleText(string text)
    {
        _bookTitleInputField.text = text;
    }

    public void SetYearText(string text)
    {
        _yearOfPublicationInputField.text = text;
    }

    public void SetAuthorText(string text)
    {
        _authorInputField.text = text;
    }

    public void SetDescriptionText(string text)
    {
        _descriptionInputField.text = text;
    }
    
    private void OnPhotoClicked() => AddPhotoClicked?.Invoke();
    private void OnBookTitleInputed(string input) => BookTitleInputed?.Invoke(input);
    private void OnAuthorInputed(string input) => AuthorInputed?.Invoke(input);
    private void OnYearOfPublicationInputed(string input) => YearOfPublicationInputed?.Invoke(input);
    private void OnDescriptionInputed(string input) => DescriptionInputed?.Invoke(input);
    private void OnNextButtonClicked() => NextButtonClicked?.Invoke();
    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
}
