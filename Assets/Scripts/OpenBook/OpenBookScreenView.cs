using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class OpenBookScreenView : MonoBehaviour
{
    [SerializeField] private Color _selectedTextColor;
    [SerializeField] private Color _unselectedTextColor;

    [SerializeField] private Color _unselectedButtonColor;
    [SerializeField] private Color _selectedButtonColor;

    [SerializeField] private Sprite _selectedFavouiriteSprite;
    [SerializeField] private Sprite _unselectedFavouiriteSprite;

    [SerializeField] private Button _editButton;
    [SerializeField] private Button _favouriteButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private TMP_Text _genreText;
    [SerializeField] private TMP_Text _bookTitleText;
    [SerializeField] private TMP_Text _authorText;
    [SerializeField] private TMP_Text _yearText;
    [SerializeField] private TMP_Text _disriptionText;
    [SerializeField] private Button _plannedButton;
    [SerializeField] private TMP_Text _plannedButtonText;

    [SerializeField] private Button _inTheProcessButton;
    [SerializeField] private TMP_Text _inTheProcessText;

    [SerializeField] private Button _readButton;
    [SerializeField] private TMP_Text _readButtonText;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    private Button _currentButton;

    public event Action BackButtonClicked;
    public event Action EditButtonClicked;
    public event Action FavouiriteButtonClicked;
    public event Action PlannedButtonClicked;
    public event Action InTheProcessButtonClicked;
    public event Action ReadButtonClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _plannedButton.onClick.AddListener(() => SetCurrentButton(_plannedButton));
        _inTheProcessButton.onClick.AddListener(() => SetCurrentButton(_inTheProcessButton));
        _readButton.onClick.AddListener(() => SetCurrentButton(_readButton));
        _favouriteButton.onClick.AddListener(OnFavouriteButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _editButton.onClick.AddListener(OnEditButtonClicked);
    }

    private void OnDisable()
    {
        _plannedButton.onClick.RemoveListener(() => SetCurrentButton(_plannedButton));
        _inTheProcessButton.onClick.RemoveListener(() => SetCurrentButton(_inTheProcessButton));
        _readButton.onClick.RemoveListener(() => SetCurrentButton(_readButton));
        _favouriteButton.onClick.RemoveListener(OnFavouriteButtonClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _editButton.onClick.RemoveListener(OnEditButtonClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void SetBookTitleText(string title)
    {
        _bookTitleText.text = title;
    }

    public void SetAuthorText(string author)
    {
        _authorText.text = author;
    }

    public void SetYearText(string year)
    {
        _yearText.text = year;
    }

    public void SetDescriptionText(string description)
    {
        _disriptionText.text = description;
    }

    public void SetGenreText(string genre)
    {
        _genreText.text = genre;
    }

    public void ToggleFavouriteSprite(bool status)
    {
        if (status)
        {
            _favouriteButton.image.sprite = _selectedFavouiriteSprite;
        }
        else
        {
            _favouriteButton.image.sprite = _unselectedFavouiriteSprite;
        }
    }

    public void SetPlannedCurrentButton()
    {
        SetCurrentButton(_plannedButton);
    }

    public void SetInProcessCurrentButton()
    {
        SetCurrentButton(_inTheProcessButton);
    }

    public void SetReadCurrentButton()
    {
        SetCurrentButton(_readButton);
    }

    public void ResetAllButtons()
    {
        ResetButton(_inTheProcessButton);
        ResetButton(_plannedButton);
        ResetButton(_readButton);
    }

    private void SetCurrentButton(Button button)
    {
        if (_currentButton != null)
            ResetButton(_currentButton);

        _currentButton = button;
        SetSelectedButton(_currentButton);
    }

    private void ResetButton(Button button)
    {
        button.image.color = _unselectedButtonColor;

        if (button == _plannedButton)
        {
            _plannedButtonText.color = _unselectedTextColor;
        }
        else if (button == _inTheProcessButton)
        {
            _inTheProcessText.color = _unselectedTextColor;
        }
        else if (button == _readButton)
        {
            _readButtonText.color = _unselectedTextColor;
        }
    }

    private void SetSelectedButton(Button button)
    {
        button.image.color = _selectedButtonColor;

        if (button == _plannedButton)
        {
            _plannedButtonText.color = _selectedTextColor;
            PlannedButtonClicked?.Invoke();
        }
        else if (button == _inTheProcessButton)
        {
            _inTheProcessText.color = _selectedTextColor;
            InTheProcessButtonClicked?.Invoke();
        }
        else if (button == _readButton)
        {
            _readButtonText.color = _selectedTextColor;
            ReadButtonClicked?.Invoke();
        }
    }

    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
    private void OnEditButtonClicked() => EditButtonClicked?.Invoke();
    private void OnFavouriteButtonClicked() => FavouiriteButtonClicked?.Invoke();
}