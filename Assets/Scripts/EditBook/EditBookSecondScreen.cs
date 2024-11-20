using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditBookSecondScreen : MonoBehaviour
{
    [SerializeField] private EditBookSecondScreenView _view;
    [SerializeField] private BookCategoryPlane[] _bookCategoryPlanes;
    [SerializeField] private EditBookFirstScreen _firstScreen;
    
    private BookCategoryPlane _currentPlane;
    private FilledBookPlane _filledBookPlane;

    public event Action BackButtonClicked;
    public event Action ReturnToMainMenu;
    public event Action<FilledBookPlane> DeleteButtonClicked;
    
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
        _firstScreen.NextButtonClicked += OpenWindow;
        _view.SaveButtonClicked += SaveBookData;
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.DeleteButtonClicked += OnDeleteButtonClicked;
    }

    private void OnDisable()
    {
        _firstScreen.NextButtonClicked -= OpenWindow;
        _view.SaveButtonClicked -= SaveBookData;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.DeleteButtonClicked -= OnDeleteButtonClicked;
        
        foreach (var plane in _bookCategoryPlanes)
        {
            plane.ButtonSelected -= OnPlaneSelected;
        }
    }

    private void OpenWindow(FilledBookPlane filledBookPlane)
    {
        _view.Enable();
        
        if (filledBookPlane == null)
            throw new ArgumentNullException(nameof(filledBookPlane));

        _filledBookPlane = filledBookPlane;

        foreach (var categoryPlane in _bookCategoryPlanes)
        {
            if (categoryPlane.Genre == _filledBookPlane.CurrentData.Genre)
            {
                if(_currentPlane != null)
                    _currentPlane.ReturnToDefault();

                _currentPlane = categoryPlane;
                _view.SetSelectedColorsToPlane(_currentPlane);
                return;
            }
        }
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
    
    private void ValidateButtonPressed()
    {
        bool inputValid = _currentPlane != null;
        _view.SetSaveButtonInteractable(inputValid);
    }
    
    private void SaveBookData()
    {
        if(_currentPlane == null || _filledBookPlane == null)
            return;

        BookData bookDataToSave = new BookData(_currentPlane.Genre, _firstScreen.NewBookTitle, _firstScreen.NewAuthor,
            _firstScreen.NewDescription, _firstScreen.NewYearOfPublication);
        bookDataToSave.ImagePath = _firstScreen.NewImagePath;
        
        _filledBookPlane.SetGenre(bookDataToSave.Genre);
        _filledBookPlane.CurrentData.BookTitle = bookDataToSave.BookTitle;
        _filledBookPlane.CurrentData.Author = bookDataToSave.Author;
        _filledBookPlane.CurrentData.Description = bookDataToSave.Description;
        _filledBookPlane.CurrentData.PublicationDate = bookDataToSave.PublicationDate;
        
        _filledBookPlane.SetBookData(bookDataToSave);
        _filledBookPlane.ChangeData();
        _filledBookPlane.UpdateText();
        ReturnToMainMenu?.Invoke();
        _view.Disable();
    }
    
    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
    }

    private void OnDeleteButtonClicked()
    {
        DeleteButtonClicked?.Invoke(_filledBookPlane);
        _view.Disable();
    }
}
