using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBookList : MonoBehaviour
{
    [SerializeField] private EditBookList _editBookListScreen;
    [SerializeField] private OpenBookListView _view;
    [SerializeField] private List<FilledBookPlane>  _filledBookPlanes;
    [SerializeField] private AddBookListMainScreen _addBookListMainScreen;

    private List<int> _availableIndexes = new List<int>();
    
    private BookListMainScreenPlane _bookList;

    public event Action BackButtonClicked;
    public event Action<BookListMainScreenPlane> EditButtonClicked;  
    
    private void Start()
    {
        _view.Disable();
        DisableAllWindows();
    }

    private void OnEnable()
    {
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.EditButtonClicked += OnEditButtonClicked;
        _editBookListScreen.BackButtonClicked += _view.Enable;

        _addBookListMainScreen.OpenBookListClicked += OpenScreen;
    }

    private void OnDisable()
    {
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.EditButtonClicked -= OnEditButtonClicked;
        _editBookListScreen.BackButtonClicked -= _view.Enable;
        
        _addBookListMainScreen.OpenBookListClicked -= OpenScreen;
    }

    private void OpenScreen(BookListMainScreenPlane bookList)
    {
        _view.Enable();
        DisableAllWindows();
        
        if (bookList == null)
            throw new ArgumentNullException(nameof(bookList));

        _bookList = bookList;
        
        _view.SetDescriptionText(_bookList.BookListData.BookListDescription);
        _view.SetTitleText(_bookList.BookListData.BookListTitle);
        _view.SetBookCountText(_bookList.BookListData.BooksInList);

        foreach (var data in _bookList.BookListData.BookDatas)
        {
            if (data != null)
            {
                if (_availableIndexes.Count > 0)
                {
                    int availableIndex = _availableIndexes[0];
                    _availableIndexes.RemoveAt(0);

                    var currentFilledWindow = _filledBookPlanes[availableIndex];

                    if (currentFilledWindow != null)
                    {
                        currentFilledWindow.Enable();
                        currentFilledWindow.SetBookData(data);
                    }
                }
            }

        }
    }

    private void DisableAllWindows()
    {
        for (int i = 0; i < _filledBookPlanes.Count; i++)
        {
            _filledBookPlanes[i].DisableEditButton();
            _filledBookPlanes[i].Disable();
            _availableIndexes.Add(i);
        }
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
    }

    private void OnEditButtonClicked()
    {
        EditButtonClicked?.Invoke(_bookList);
        _view.Disable();
    }
}
