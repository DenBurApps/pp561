using System;
using UnityEngine;

public class AddBookFirstScreen : MonoBehaviour
{
    [SerializeField] private ScreenStateManager _screenStateManager;
    [SerializeField] private AddBookFirstScreenView _view;
    [SerializeField] private BookImage _currentImage;
    [SerializeField] private AddBookSecondScreen _secondScreen;

    private string _bookTitle;
    private string _author;
    private string _imagePath;
    private string _yearOfPublication;
    private string _description;
    
    public string BookTitle => _bookTitle;
    public string Author => _author;
    public string ImagePath => _imagePath;
    public string YearOfPublication => _yearOfPublication;
    public string Description => _description;

    public event Action NextButtonClicked;
    public event Action BackButtonClicked;

    private void Start()
    {
        _view.Disable();
        _currentImage.Disable();
    }

    private void OnEnable()
    {
        _screenStateManager.AddBookFirstScreenOpen += OpenScreen;

        _currentImage.DeleteButtonClicked += OnImageDeleted;

        _secondScreen.BackButtonClicked += _view.Enable;
        _secondScreen.ReturnToMainMenu += ResetData;

        _view.AuthorInputed += OnAuthorChanged;
        _view.BookTitleInputed += OnBookTitleChanged;
        _view.YearOfPublicationInputed += OnYearOfPublicationChanged;
        _view.DescriptionInputed += OnDescriptionChanged;
        _view.AddPhotoClicked += TrySpawnPhoto;
        _view.NextButtonClicked += OnNextButtonClicked;
        _view.BackButtonClicked += OnBackButtonClicked;
    }

    private void OnDisable()
    {
        _screenStateManager.AddBookFirstScreenOpen -= OpenScreen;

        _currentImage.DeleteButtonClicked -= OnImageDeleted;
        
        _secondScreen.BackButtonClicked -= _view.Enable;
        _secondScreen.ReturnToMainMenu -= ResetData;
        
        _view.AuthorInputed -= OnAuthorChanged;
        _view.BookTitleInputed -= OnBookTitleChanged;
        _view.YearOfPublicationInputed -= OnYearOfPublicationChanged;
        _view.DescriptionInputed -= OnDescriptionChanged;
        _view.AddPhotoClicked -= TrySpawnPhoto;
        _view.NextButtonClicked -= OnNextButtonClicked;
        _view.BackButtonClicked -= OnBackButtonClicked;
    }

    private void OnBookTitleChanged(string title)
    {
        _bookTitle = title;
        ValidateInputs();
    }

    private void OnAuthorChanged(string author)
    {
        _author = author;
        ValidateInputs();
    }

    private void OnDescriptionChanged(string description)
    {
        _description = description;
        ValidateInputs();
    }

    private void OnYearOfPublicationChanged(string year)
    {
        _yearOfPublication = year;
        ValidateInputs();
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

            _imagePath = str;
        }
        else
        {
            _currentImage.Disable();
            _view.ToggleAddPhotoButton(true);
        }
    }

    private void OnNextButtonClicked()
    {
        NextButtonClicked?.Invoke();
        _view.Disable();
    }

    private void ValidateInputs()
    {
        bool allInputsValid = !string.IsNullOrEmpty(_bookTitle) && !string.IsNullOrEmpty(_author) &&
                              !string.IsNullOrEmpty(_yearOfPublication) && !string.IsNullOrEmpty(_description);

        _view.SetNextButtonInteractable(allInputsValid);
    }

    private void ResetData()
    {
        _bookTitle = string.Empty;
        _author = string.Empty;
        _imagePath = null;
        _yearOfPublication = string.Empty;
        _description = string.Empty;
        
        _view.SetAuthorText(_author);
        _view.SetTitleText(_bookTitle);
        _view.SetDescriptionText(_description);
        _view.SetYearText(_yearOfPublication);
        _currentImage.Disable();
        _view.ToggleAddPhotoButton(true);
    }

    private void OpenScreen()
    {
        ResetData();
        _view.Enable();
        ValidateInputs();
    }

    private void OnBackButtonClicked()
    {
        ResetData();
        BackButtonClicked?.Invoke();
        _view.Disable();
    }

    private void OnImageDeleted()
    {
        _currentImage.Disable();
        _imagePath = null;
        _view.ToggleAddPhotoButton(true);
    }
    
    
}