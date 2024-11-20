using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviewWindowToEdit : MonoBehaviour
{
    [SerializeField] private Color _selectedButtonColor;
    [SerializeField] private Color _unselectedButtonColor;
    [SerializeField] private Color _selectedTextColor;
    [SerializeField] private Color _unselectedTextColor;

    [SerializeField] private Sprite _selectedStarSprite;
    [SerializeField] private Sprite _defaultStarSprite;
    
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private TMP_Text _saveButtonText;
    [SerializeField] private TMP_InputField _reviewInputField;
    [SerializeField] private Button[] _starsButtons = new Button[5];

    private bool _starClicked = false;
    private int _selectedStarCount = 0;
    private string _review;

    public event Action<int,string> SaveButtonClicked;

    private void Start()
    {
        Disable();
    }

    private void OnEnable()
    {
        _reviewInputField.onValueChanged.AddListener(OnReviewInputed);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
        _saveButton.onClick.AddListener(OnSaveButtonClicked);

        for (int i = 0; i < _starsButtons.Length; i++)
        {
            int index = i;
            _starsButtons[index].onClick.AddListener(() => ProcessStarButtonClicked(_starsButtons[index]));
        }
    }

    private void OnDisable()
    {
        _reviewInputField.onValueChanged.RemoveListener(OnReviewInputed);
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        _saveButton.onClick.RemoveListener(OnSaveButtonClicked);

        for (int i = 0; i < _starsButtons.Length; i++)
        {
            int index = i;
            _starsButtons[index].onClick.RemoveListener(() => ProcessStarButtonClicked(_starsButtons[index]));
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        ClearData();
        gameObject.SetActive(false);
    }

    public void ClearData()
    {
        _review = string.Empty;
        _selectedStarCount = 0;
        _starClicked = false;
        _reviewInputField.text = _review;

        foreach (var star in _starsButtons)
        {
            star.image.sprite = _defaultStarSprite;
        }
        
        ValidateInput();
    }
    
    private void ValidateInput()
    {
        bool isInputValid = _starClicked && !string.IsNullOrEmpty(_reviewInputField.text);

        SetSaveButtonInteractable(isInputValid);
    }
    
    private void SetSaveButtonInteractable(bool status)
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

    private void ProcessStarButtonClicked(Button button)
    {
        _starClicked = true;

        for (int i = 0; i < _starsButtons.Length; i++)
        {
            if (i <= System.Array.IndexOf(_starsButtons, button))
            {
                _starsButtons[i].image.sprite = _selectedStarSprite;
            }
            else
            {
                _starsButtons[i].image.sprite = _defaultStarSprite;
            }
        }

        _selectedStarCount = System.Array.IndexOf(_starsButtons, button) + 1;
        ValidateInput();
    }

    public void EnableStarButtons(int count)
    {
        _starClicked = true;
        
        count = Mathf.Clamp(count, 0, _starsButtons.Length);
        
        for (int i = 0; i < _starsButtons.Length; i++)
        {
            if (i < count)
            {
                _starsButtons[i].image.sprite = _selectedStarSprite;
            }
            else
            {
                _starsButtons[i].image.sprite = _defaultStarSprite;
            }
        }
        
        _selectedStarCount = count;
        ValidateInput();
    }

    public void SetReviewText(string text)
    {
        _reviewInputField.text = text;
        ValidateInput();
    }

    private void OnReviewInputed(string review)
    {
        _review = review;
        ValidateInput();
    }

    private void OnCloseButtonClicked()
    {
        Disable();
    }

    private void OnSaveButtonClicked()
    {
        SaveButtonClicked?.Invoke(_selectedStarCount, _review);
        Disable();
    }
}
