using System;
using System.Collections.Generic;
using UnityEngine;

public class EditBookList : MonoBehaviour
{
    [SerializeField] private EditBookListView _view;
    [SerializeField] private List<BookListBookElement> _bookElements;
    [SerializeField] private OpenBookList _openBookListScreen;

    private List<int> _availableIndexes = new List<int>();

    private string _newTitle;
    private string _newDescription;
    private int _newBooksCount;

    private BookListMainScreenPlane _bookList;

    public event Action BackButtonClicked;
    public event Action BookListDataSaved;
    public event Action<BookListMainScreenPlane> BookListDeleted;

    private void Start()
    {
        _view.Disable();
        DisableAllWindows();
    }

    private void OnEnable()
    {
        _openBookListScreen.EditButtonClicked += OpenWindow;

        _view.BackButtonClicked += OnBackButtonClicked;
        _view.DeleteButtonClicked += OnDeleteButtonClicked;
        _view.BookTitleInputed += OnTitleInputed;
        _view.BookDescriptionInputed += OnDescriptionInputed;
        _view.SaveButtonClicked += SaveButtonClicked;
    }

    private void OnDisable()
    {
        _openBookListScreen.EditButtonClicked -= OpenWindow;

        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.DeleteButtonClicked -= OnDeleteButtonClicked;
        _view.BookTitleInputed -= OnTitleInputed;
        _view.BookDescriptionInputed -= OnDescriptionInputed;
        _view.SaveButtonClicked -= SaveButtonClicked;
    }

    private void OpenWindow(BookListMainScreenPlane bookList)
    {
        _view.Enable();

        if (bookList == null)
            throw new ArgumentNullException(nameof(bookList));

        ResetAllData();

        _bookList = bookList;
        _view.SetTitleText(_bookList.BookListData.BookListTitle);
        _view.SetDescriptionText(_bookList.BookListData.BookListDescription);
        _newBooksCount = _bookList.BookListData.BooksInList;

        for (int i = 0; i < _bookList.BookListData.BookDatas.Count; i++)
        {
            if (_availableIndexes.Count > 0)
            {
                int availableIndex = _availableIndexes[0];
                _availableIndexes.RemoveAt(0);

                var currentFilledWindow = _bookElements[availableIndex];

                if (currentFilledWindow != null)
                {
                    currentFilledWindow.Enable();
                    currentFilledWindow.EnableWindow(_bookList.BookListData.BookDatas[i]);
                    currentFilledWindow.SetSelected();
                    currentFilledWindow.SelectionChanged += ValidateInput;
                }
            }
        }
    }

    private void SaveButtonClicked()
    {
        List<BookData> bookDatas = new List<BookData>();

        foreach (var bookElement in _bookElements)
        {
            if (bookElement.IsSelected)
                bookDatas.Add(bookElement.BookData);
        }

        _newBooksCount = bookDatas.Count;

        BookListData bookListData = new BookListData(_newTitle, _newBooksCount, bookDatas, _newDescription);
        _bookList.SetData(bookListData);
        _bookList.UpdateData();
        BookListDataSaved?.Invoke();
        ResetAllData();
        _view.Disable();
    }

    private void OnTitleInputed(string title)
    {
        _newTitle = title;
        ValidateInput();
    }

    private void OnDescriptionInputed(string description)
    {
        _newDescription = description;
        ValidateInput();
    }

    private void OnSelectionCountChanged(bool selected)
    {
        if (selected)
        {
            _newBooksCount++;
        }
        else
        {
            _newBooksCount--;

            if (_newBooksCount < 0)
                _newBooksCount = 0;
        }

        ValidateInput();
    }

    private void DisableAllWindows()
    {
        for (int i = 0; i < _bookElements.Count; i++)
        {
            _bookElements[i].Disable();
            _availableIndexes.Add(i);
        }
    }

    private void ValidateInput()
    {
        List<BookData> bookDatas = new List<BookData>();

        foreach (var bookElement in _bookElements)
        {
            if (bookElement.IsSelected)
                bookDatas.Add(bookElement.BookData);
        }
        
        bool validateStatus = bookDatas.Count >= 2;

        _view.SetSaveButtonInteractable(validateStatus);
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
    }

    private void OnDeleteButtonClicked()
    {
        BookListDeleted?.Invoke(_bookList);
        _view.Disable();
    }

    private void ResetAllData()
    {
        _newTitle = string.Empty;
        _newDescription = string.Empty;
        _newBooksCount = 0;

        _view.SetTitleText(_newTitle);
        _view.SetDescriptionText(_newDescription);

        DisableAllWindows();
    }
}