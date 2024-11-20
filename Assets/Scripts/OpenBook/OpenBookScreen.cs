using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBookScreen : MonoBehaviour
{
    [SerializeField] private EditBookFirstScreen _editBookFirstScreen;
    
    [SerializeField] private ReviewWindowToEdit _reviewWindowToEdit;
    [SerializeField] private ReviewWindowToEdit _reviewWindowToEditWithoutCongratulation;
    [SerializeField] private CompleteReviewPlane _completeReviewPlane;

    [SerializeField] private Sprite _noImageSprite;

    [SerializeField] private OpenBookScreenView _view;
    [SerializeField] private ImagePicker _imagePicker;
    [SerializeField] private MainScreen _mainScreen;

    private FilledBookPlane _currentBookPlane;
    private bool _favouriteClicked = false;
    private bool _reviewInputed;

    public event Action BackButtonClicked;
    public event Action<FilledBookPlane> EditButtonClicked; 

    private void Start()
    {
        _view.Disable();
    }

    private void OnEnable()
    {
        _view.FavouiriteButtonClicked += OnFavouriteButtonClicked;
        _mainScreen.EditBookPlane += OpenWindow;
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.PlannedButtonClicked += OnPlannedClicked;
        _view.InTheProcessButtonClicked += OnInTheProcessClicked;
        _view.ReadButtonClicked += OnReadButtonClicked;
        _view.EditButtonClicked += OnEditButtonClicked;
        _reviewWindowToEdit.SaveButtonClicked += ReviewInputed;

        _reviewWindowToEditWithoutCongratulation.SaveButtonClicked += ReviewInputed;
        
        _completeReviewPlane.EditButtonClicked += EditReview;

        _editBookFirstScreen.BackButtonClicked += _view.Enable;
        _editBookFirstScreen.Deleted += ResetData;
    }

    private void OnDisable()
    {
        _view.FavouiriteButtonClicked -= OnFavouriteButtonClicked;
        _mainScreen.EditBookPlane -= OpenWindow;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.PlannedButtonClicked -= OnPlannedClicked;
        _view.InTheProcessButtonClicked -= OnInTheProcessClicked;
        _view.ReadButtonClicked -= OnReadButtonClicked;
        _view.EditButtonClicked -= OnEditButtonClicked;
        _reviewWindowToEdit.SaveButtonClicked -= ReviewInputed;

        _reviewWindowToEditWithoutCongratulation.SaveButtonClicked -= ReviewInputed;
        
        _completeReviewPlane.EditButtonClicked -= EditReview;
        
        _editBookFirstScreen.BackButtonClicked -= _view.Enable;
        _editBookFirstScreen.Deleted -= ResetData;
    }

    private void OpenWindow(FilledBookPlane bookPlane)
    {
        if (bookPlane == null)
            throw new ArgumentNullException(nameof(bookPlane));

        _currentBookPlane = bookPlane;

        _view.SetAuthorText(bookPlane.CurrentData.Author);
        _view.SetBookTitleText(bookPlane.CurrentData.BookTitle);
        _view.SetDescriptionText(bookPlane.CurrentData.Description);
        _view.SetYearText(bookPlane.CurrentData.PublicationDate);
        _view.SetGenreText(GenreExtensions.ToString(bookPlane.CurrentData.Genre));

        if (!string.IsNullOrEmpty(bookPlane.CurrentData.ImagePath))
        {
            _imagePicker.Init(bookPlane.CurrentData.ImagePath);
        }
        else
        {
            _imagePicker.Image.sprite = _noImageSprite;
        }

        _favouriteClicked = bookPlane.CurrentData.FavouriteSelected;
        _view.ToggleFavouriteSprite(_favouriteClicked);

        if (!string.IsNullOrEmpty(_currentBookPlane.CurrentData.Review))
        {
            _reviewInputed = true;
            _completeReviewPlane.Enable();

            if (!string.IsNullOrEmpty(_currentBookPlane.CurrentData.Review))
            {
                _completeReviewPlane.SetReviewText(_currentBookPlane.CurrentData.Review);
            }

            if (_currentBookPlane.CurrentData.Stars > 0)
            {
                _completeReviewPlane.SetStarsActive(_currentBookPlane.CurrentData.Stars);
            }
        }
        else
        {
            _reviewInputed = false;
        }

        if (_currentBookPlane.CurrentData.Status == Status.Planned)
        {
            _view.SetPlannedCurrentButton();
            OnPlannedClicked();
        }
        else if(_currentBookPlane.CurrentData.Status == Status.Read)
        {
            _view.SetReadCurrentButton();
            OnReadButtonClicked();
        }
        else if(_currentBookPlane.CurrentData.Status == Status.InProcess)
        {
            _view.SetInProcessCurrentButton();
            OnInTheProcessClicked();
        }

        _view.Enable();
    }
    
    public void ResetData()
    {
        _currentBookPlane = null;
        
        _view.SetAuthorText(string.Empty);
        _view.SetBookTitleText(string.Empty);
        _view.SetDescriptionText(string.Empty);
        _view.SetYearText(string.Empty);
        _view.SetGenreText(string.Empty);
        
        _imagePicker.Image.sprite = _noImageSprite;
        
        _favouriteClicked = false;
        _view.ToggleFavouriteSprite(_favouriteClicked);
        
        _reviewInputed = false;
        _reviewWindowToEdit.Disable();
        _reviewWindowToEditWithoutCongratulation.Disable();
        _completeReviewPlane.Disable();
        
        _view.ResetAllButtons();
        
        _view.Disable();
    }

    private void OnPlannedClicked()
    {
        if (_currentBookPlane == null)
            return;

        _currentBookPlane.SetStatus(Status.Planned);
        _currentBookPlane.ChangeData();
    }

    private void OnInTheProcessClicked()
    {
        if (_currentBookPlane == null)
            return;

        _currentBookPlane.SetStatus(Status.InProcess);
        _currentBookPlane.ChangeData();
    }

    private void OnReadButtonClicked()
    {
        if (!_reviewInputed)
        {
            _reviewWindowToEdit.Enable();
        }
        else
        {
            _completeReviewPlane.Enable();

            if (!string.IsNullOrEmpty(_currentBookPlane.CurrentData.Review))
            {
                _completeReviewPlane.SetReviewText(_currentBookPlane.CurrentData.Review);
            }

            if (_currentBookPlane.CurrentData.Stars > 0)
            {
                _completeReviewPlane.SetStarsActive(_currentBookPlane.CurrentData.Stars);
            }
            
            _currentBookPlane.SetStatus(Status.Read);
            _currentBookPlane.ChangeData();
        }
    }

    private void ReviewInputed(int stars, string review)
    {
        _currentBookPlane.CurrentData.Stars = stars;
        _currentBookPlane.CurrentData.Review = review;
        _currentBookPlane.ChangeData();

        _completeReviewPlane.Enable();
        _completeReviewPlane.OpenWindow(stars, review);
        _reviewInputed = true;
    }

    private void EditReview( string review, int stars)
    {
        _reviewWindowToEditWithoutCongratulation.Enable();
        _reviewWindowToEditWithoutCongratulation.SetReviewText(review);
        _reviewWindowToEditWithoutCongratulation.EnableStarButtons(stars);
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        ResetData();
        _view.Disable();
    }

    private void OnFavouriteButtonClicked()
    {
        if (_currentBookPlane == null)
            return;

        if (_favouriteClicked == false)
        {
            _favouriteClicked = true;
        }
        else
        {
            _favouriteClicked = false;
        }

        _currentBookPlane.SetFavouriteStatus(_favouriteClicked);
        _view.ToggleFavouriteSprite(_favouriteClicked);
    }

    private void OnEditButtonClicked()
    {
        EditButtonClicked?.Invoke(_currentBookPlane);
        _view.Disable();
    }
}