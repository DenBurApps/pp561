using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditBookFirstScreen : MonoBehaviour
{
    [SerializeField] private Sprite _defaultImageSprite;
    [SerializeField] private EditBookFirstScreenView _view;
    [SerializeField] private BookImage _currentImage;
    [SerializeField] private EditBookSecondScreen _secondScreen;
    [SerializeField] private OpenBookScreen _openBookScreen;

    private FilledBookPlane _filledBookPlane;
    private string _newBookTitle;
    private string _newAuthor;
    private string _newImagePath;
    private string _newYearOfPublication;
    private string _newDescription;

    public event Action<FilledBookPlane> DeleteButtonClicked;
    public event Action<FilledBookPlane> NextButtonClicked;
    public event Action BackButtonClicked;
    public event Action Deleted;
    
    public string NewBookTitle => _newBookTitle;
    public string NewAuthor => _newAuthor;
    public string NewImagePath => _newImagePath;
    public string NewYearOfPublication => _newYearOfPublication;
    public string NewDescription => _newDescription;
    
    private void Start()
    {
        _view.Disable();
    }

    private void OnEnable()
    {
        _view.BookTitleInputed += OnBookTitleChanged;
        _view.AuthorInputed += OnAuthorChanged;
        _view.DescriptionInputed += OnDescriptionChanged;
        _view.AddPhotoClicked += TrySpawnPhoto;
        _view.YearOfPublicationInputed += OnYearOfPublicationChanged;
        _view.DeleteButtonClicked += OnDeleteButtonClicked;
        _view.NextButtonClicked += OnNextButtonClicked;
        _view.BackButtonClicked += OnBackButtonClicked;
        _openBookScreen.EditButtonClicked += OpenWindow;
        _secondScreen.BackButtonClicked += _view.Enable;
    }

    private void OnDisable()
    {
        _view.BookTitleInputed -= OnBookTitleChanged;
        _view.AuthorInputed -= OnAuthorChanged;
        _view.DescriptionInputed -= OnDescriptionChanged;
        _view.AddPhotoClicked -= TrySpawnPhoto;
        _view.YearOfPublicationInputed -= OnYearOfPublicationChanged;
        _view.DeleteButtonClicked -= OnDeleteButtonClicked;
        _view.NextButtonClicked -= OnNextButtonClicked;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _openBookScreen.EditButtonClicked -= OpenWindow;
        _secondScreen.BackButtonClicked -= _view.Enable;
        _currentImage.DeleteButtonClicked -= OnImageDeleted;
    }

    private void OpenWindow(FilledBookPlane bookPlane)
    {
        if (bookPlane == null)
            throw new ArgumentNullException(nameof(bookPlane));

        _view.Enable();

        _filledBookPlane = bookPlane;
        _view.SetAuthorText(bookPlane.CurrentData.Author);
        _view.SetTitleText(bookPlane.CurrentData.BookTitle);
        _view.SetYearText(bookPlane.CurrentData.PublicationDate);
        _view.SetDescriptionText(bookPlane.CurrentData.Description);

        if (!string.IsNullOrEmpty(bookPlane.CurrentData.ImagePath))
        {
            _currentImage.Enable();
            _currentImage.ImagePicker.Init(bookPlane.CurrentData.ImagePath);
            _currentImage.DeleteButtonClicked += OnImageDeleted;
            _view.ToggleAddPhotoButton(false);
        }
        else
        {
            _currentImage.ImagePicker.Image.sprite = _defaultImageSprite;
        }
    }

    private void OnBookTitleChanged(string title)
    {
        _newBookTitle = title;
        ValidateInputs();
    }

    private void OnAuthorChanged(string author)
    {
        _newAuthor = author;
        ValidateInputs();
    }

    private void OnDescriptionChanged(string description)
    {
        _newDescription = description;
        ValidateInputs();
    }

    private void OnYearOfPublicationChanged(string year)
    {
        _newYearOfPublication = year;
        ValidateInputs();
    }
    
    private void OnImageDeleted()
    {
        _currentImage.Disable();
        /*_filledBookPlane.CurrentData.ImagePath = string.Empty;
        _filledBookPlane.UpdateText();*/
        _view.ToggleAddPhotoButton(true);
    }

    private void TrySpawnPhoto()
    {
        if (_currentImage != null)
        {
            _currentImage.enabled = true;
            GetImageFromGallery.PickImage(TakePhoto);
        }
    }

    private void TakePhoto(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            if (_currentImage != null)
            {
                _currentImage.Enable();
                _currentImage.ImagePicker.Init(str);
                _view.ToggleAddPhotoButton(false);
            }

            _newImagePath = str;
            /*_filledBookPlane.CurrentData.ImagePath = _newImagePath;
            _filledBookPlane.ChangeData();
            _filledBookPlane.UpdateText();*/
        }
        else
        {
            _currentImage.Disable();
            _view.ToggleAddPhotoButton(true);
        }
    }

    private void ValidateInputs()
    {
        _view.SetNextButtonInteractable(true);
    }

    private void OnDeleteButtonClicked()
    {
        DeleteButtonClicked?.Invoke(_filledBookPlane);
        Deleted?.Invoke();
        _view.Disable();
    }

    private void OnNextButtonClicked()
    {
        NextButtonClicked?.Invoke(_filledBookPlane);
        _view.Disable();
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
    }
}