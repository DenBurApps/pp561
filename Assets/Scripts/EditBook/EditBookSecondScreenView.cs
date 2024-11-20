using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class EditBookSecondScreenView : MonoBehaviour
{
    [SerializeField] private Color _unselectedButtonColor;
    [SerializeField] private Color _unselectedTextColor;
    [SerializeField] private Color _selectedButtonColor;
    [SerializeField] private Color _selectedTextColor;
    
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private TMP_Text _saveButtonText;

    public event Action BackButtonClicked;
    public event Action SaveButtonClicked;
    public event Action DeleteButtonClicked;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _saveButton.onClick.AddListener(OnSaveButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _deleteButton.onClick.AddListener(OnDeleteButtonClicked);
    }

    private void OnDisable()
    {
        _saveButton.onClick.RemoveListener(OnSaveButtonClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
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

    public void SetSelectedColorsToPlane(BookCategoryPlane plane)
    {
        if (plane == null)
            throw new ArgumentNullException(nameof(plane));
        
        plane.SetColor(_selectedTextColor, _selectedButtonColor);
    }

    private void OnSaveButtonClicked() => SaveButtonClicked?.Invoke();
    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
    private void OnDeleteButtonClicked() => DeleteButtonClicked?.Invoke();
}
