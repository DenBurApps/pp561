using System;
using UnityEngine;

public class AddBookSecondScreen : MonoBehaviour
{
    [SerializeField] private AddBookSecondScreenView _view;
    [SerializeField] private BookCategoryPlane[] _bookCategoryPlanes;
    [SerializeField] private AddBookFirstScreen _firstScreen;

    private BookCategoryPlane _currentPlane;

    public event Action BackButtonClicked;
    public event Action ReturnToMainMenu;
    public event Action<BookData> BookSaved; 
    
    private void Start()
    {
        _view.Disable();

        foreach (var plane in _bookCategoryPlanes)
        {
            plane.ButtonSelected += OnPlaneSelected;
        }
    }

    private void OnEnable()
    {
        _view.BackButtonClicked += OnBackButtonClicked;
        _firstScreen.NextButtonClicked += OpenWindow;
        _view.SaveButtonClicked += SaveBookData;
    }

    private void OnDisable()
    {
        _view.BackButtonClicked -= OnBackButtonClicked;
        _firstScreen.NextButtonClicked -= OpenWindow;
        _view.SaveButtonClicked -= SaveBookData;
    }

    private void ValidateButtonPressed()
    {
        bool inputValid = _currentPlane != null;
        _view.SetSaveButtonInteractable(inputValid);
    }

    private void OnPlaneSelected(BookCategoryPlane plane)
    {
        if (plane == null)
            throw new ArgumentNullException(nameof(plane));
        
        if(_currentPlane != null)
            _currentPlane.ReturnToDefault();

        _currentPlane = plane;
        _view.SetSelectedColorsToPlane(_currentPlane);
        ValidateButtonPressed();
    }

    private void OpenWindow()
    {
        _view.Enable();
        ValidateButtonPressed();
    }

    private void SaveBookData()
    {
        if(_currentPlane == null)
            return;

        BookData bookDataToSave = new BookData(_currentPlane.Genre, _firstScreen.BookTitle, _firstScreen.Author,
            _firstScreen.Description, _firstScreen.YearOfPublication);

        if (!string.IsNullOrEmpty(_firstScreen.ImagePath))
        {
            bookDataToSave.ImagePath = _firstScreen.ImagePath;
        }
        
        BookSaved?.Invoke(bookDataToSave);
        ReturnToMainMenu?.Invoke();
        ResetToDefault();
        _view.Disable();
    }
    
    private void ResetToDefault()
    {
        _currentPlane = null;

        foreach (var plane in _bookCategoryPlanes)
        {
            plane.ReturnToDefault();
        }
        
        ValidateButtonPressed();
    }

    private void OnBackButtonClicked()
    {
        ResetToDefault();
        _view.Disable();
        BackButtonClicked?.Invoke();
    }
}
