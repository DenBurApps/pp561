using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AddBookListScreen : MonoBehaviour
{
    [SerializeField] private AddBookListScreenView _view;
    [SerializeField] private List<BookListBookElement> _bookElements;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private ScreenStateManager _screenStateManager;
    [SerializeField] private GameObject _noBooksPopup;

    private List<int> _availableIndexes = new List<int>();

    private string _title;
    private string _description;
    private int _booksCount;
    private bool _elementFound;

    public event Action BackButtonClicked;
    public event Action<BookListData> BookListDataSaved;

    private void Start()
    {
        _view.Disable();
        DisableAllWindows();
        _noBooksPopup.SetActive(false);
    }

    private void OnEnable()
    {
        _view.BookTitleInputed += OnTitleInputed;
        _view.BookDescriptionInputed += OnDescriptionInputed;
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.SaveButtonClicked += OnSaveButtonClicked;

        _screenStateManager.AddBooksListScreenOpen += OnWindowOpen;
    }

    private void OnDisable()
    {
        _view.BookTitleInputed -= OnTitleInputed;
        _view.BookDescriptionInputed -= OnDescriptionInputed;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.SaveButtonClicked -= OnSaveButtonClicked;

        _screenStateManager.AddBooksListScreenOpen -= OnWindowOpen;

        foreach (var bookElement in _bookElements)
        {
            bookElement.SelectionChanged -= ValidateInput;
        }
    }

    private void OnWindowOpen()
    {
        _view.Enable();
        ResetAllData();

        for (int i = 0; i < _mainScreen.BookPlanes.Count; i++)
        {
            if (_availableIndexes.Count > 0 && _mainScreen.BookPlanes[i].CurrentData != null)
            {
                int availableIndex = _availableIndexes[0];
                _availableIndexes.RemoveAt(0);

                var currentPlane = _bookElements[availableIndex];

                if (currentPlane != null)
                {
                    currentPlane.Enable();
                    currentPlane.EnableWindow(_mainScreen.BookPlanes[i].CurrentData);
                    currentPlane.SelectionChanged += ValidateInput;
                }
            }
        }

        _elementFound = _bookElements.Any(plane => plane.IsActive);

        if (!_elementFound || _bookElements.Count(element => element.IsActive) < 2)
        {
            ShowNoBooksPopup();
        }
        else
        {
            if (_noBooksPopup.activeSelf)
                _noBooksPopup.SetActive(false);
        }


        _view.ToggleEmptyListImage(!_elementFound);
    }

    private void ShowNoBooksPopup()
    {
        // Using a PopupManager
        //_popupManager.ShowPopup("Cannot Create Book List", "You need to create at least one book before creating a book list.");

        // OR, using a simple GameObject
        if (_noBooksPopup != null)
        {
            _noBooksPopup.SetActive(true);
        }
    }

    private void OnSaveButtonClicked()
    {
        List<BookData> bookDatas = new List<BookData>();

        foreach (var bookElement in _bookElements)
        {
            if (bookElement.IsSelected)
                bookDatas.Add(bookElement.BookData);
        }

        _booksCount = bookDatas.Count;

        BookListData bookListData = new BookListData(_title, _booksCount, bookDatas, _description);
        BookListDataSaved?.Invoke(bookListData);
        OnBackButtonClicked();
    }

    private void OnTitleInputed(string title)
    {
        _title = title;
        ValidateInput();
    }

    private void OnDescriptionInputed(string description)
    {
        _description = description;
        ValidateInput();
    }

    private void OnSelectionCountChanged(bool selected)
    {
        if (selected)
        {
            _booksCount++;
        }
        else
        {
            _booksCount--;

            if (_booksCount < 0)
                _booksCount = 0;
        }

        ValidateInput();
    }

    private void ValidateInput()
    {
        List<BookData> bookDatas = new List<BookData>();

        foreach (var bookElement in _bookElements)
        {
            if (bookElement.IsSelected)
                bookDatas.Add(bookElement.BookData);
        }

        bool validateStatus = !string.IsNullOrEmpty(_title) && !string.IsNullOrEmpty(_description) &&
                              bookDatas.Count >= 2;

        _view.SetSaveButtonInteractable(validateStatus);
    }

    private void DisableAllWindows()
    {
        for (int i = 0; i < _bookElements.Count; i++)
        {
            _bookElements[i].Disable();
            _availableIndexes.Add(i);
        }
    }

    private void ResetAllData()
    {
        _title = string.Empty;
        _description = string.Empty;
        _booksCount = 0;

        _availableIndexes.Clear();
        _view.SetTitleText(_title);
        _view.SetDescriptionText(_description);
        ValidateInput();
        DisableAllWindows();
        _elementFound = false;
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        ResetAllData();
        _view.Disable();
    }
}