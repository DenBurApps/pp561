using System;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenStatusScroll : MonoBehaviour
{
    [SerializeField] private Sprite _selectedImage;
    [SerializeField] private Sprite _unselectedImage;

    [SerializeField] private Image _favouritesSelectedImage;
    [SerializeField] private Image _favouritesUnselectedImage;

    [SerializeField] private Button _allButton;
    [SerializeField] private Button _favouritesButton;
    [SerializeField] private Button _plannedButton;
    [SerializeField] private Button _inTheProcessButton;
    [SerializeField] private Button _readButton;

    private Button _currentButton;

    public event Action AllButtonClicked;
    public event Action FavouritesButtonClicked;
    public event Action PlannedButtonClicked;
    public event Action InTheProcessButtonClicked;
    public event Action ReadButtonClicked;

    private void Start()
    {
        SetCurrentButton(_allButton);
    }

    private void OnEnable()
    {
        _allButton.onClick.AddListener(() => ProcessButtonSelected(_allButton));
        _favouritesButton.onClick.AddListener(() => ProcessButtonSelected(_favouritesButton));
        _plannedButton.onClick.AddListener(() => ProcessButtonSelected(_plannedButton));
        _inTheProcessButton.onClick.AddListener(() => ProcessButtonSelected(_inTheProcessButton));
        _readButton.onClick.AddListener(() => ProcessButtonSelected(_readButton));
        
        
    }

    private void OnDisable()
    {
        _allButton.onClick.RemoveAllListeners();
        _favouritesButton.onClick.RemoveAllListeners();
        _plannedButton.onClick.RemoveAllListeners();
        _inTheProcessButton.onClick.RemoveAllListeners();
        _readButton.onClick.RemoveAllListeners();
    }

    private void ProcessButtonSelected(Button button)
    {
        if (button == _allButton)
        {
            AllButtonClicked?.Invoke();
            SetCurrentButton(_allButton);
        }
        else if (button == _favouritesButton)
        {
            FavouritesButtonClicked?.Invoke();
            SetCurrentButton(_favouritesButton);
        }
        else if (button == _plannedButton)
        {
            PlannedButtonClicked?.Invoke();
            SetCurrentButton(_plannedButton);
        }
        else if (button == _inTheProcessButton)
        {
            InTheProcessButtonClicked?.Invoke();
            SetCurrentButton(_inTheProcessButton);
        }
        else if (button == _readButton)
        {
            ReadButtonClicked?.Invoke();
            SetCurrentButton(_readButton);
        }
    }

    private void SetCurrentButton(Button selectedButton)
    {
        if (_currentButton != null)
            ResetButtonImage(_currentButton);

        _currentButton = selectedButton;
        _currentButton.image.sprite = _selectedImage;

        if (_currentButton == _favouritesButton)
        {
            _favouritesSelectedImage.enabled = true;
            _favouritesUnselectedImage.enabled = false;
        }
        else
        {
            _favouritesUnselectedImage.enabled = true;
            _favouritesSelectedImage.enabled = false;
        }
    }

    private void ResetButtonImage(Button button)
    {
        button.image.sprite = _unselectedImage;
    }
}