using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AddBookListMainScreen : MonoBehaviour
{
    [SerializeField] private EditBookList _editBookListScreen;
    [SerializeField] private AddBooksMainScreenView _view;
    [SerializeField] private List<BookListMainScreenPlane>  _bookListPlanes;
    [SerializeField] private AddBookListScreen _addBookListScreen;
    [SerializeField] private ScreenStateManager _screenStateManager;
    [SerializeField] private OpenBookList _openBookList;

    private List<int> _availableIndexes = new List<int>();
    
    private string _filePath;
    
    public event Action AddBookListClicked;
    public event Action BackButtonClicked;
    public event Action<BookListMainScreenPlane> OpenBookListClicked; 
    
    private void Start()
    {
        _view.Disable();
        
        _filePath = Path.Combine(Application.persistentDataPath, "booklists.json");
        
        DisableAllWindows();
        LoadBookLists();
    }

    private void OnEnable()
    {
        _view.AddBookListClicked += OnAddBookListClicked;
        _view.BackButtonClicked += OnBackButtonClicked;
        _screenStateManager.AddBookListMainScreenOpen += OpenScreen;
        _openBookList.BackButtonClicked += OpenScreen;
        _editBookListScreen.BookListDataSaved += OpenScreen;
        _editBookListScreen.BookListDeleted += DeleteBookListPlane;

        _addBookListScreen.BookListDataSaved += ActivateBookListPlane;
    }

    private void OnDisable()
    {
        _view.AddBookListClicked -= OnAddBookListClicked;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _screenStateManager.AddBookListMainScreenOpen -= OpenScreen;
        _openBookList.BackButtonClicked -= OpenScreen;
        _editBookListScreen.BookListDataSaved -= OpenScreen;
        _editBookListScreen.BookListDeleted -= DeleteBookListPlane;
        
        _addBookListScreen.BookListDataSaved -= ActivateBookListPlane;
    }

    private void OpenScreen()
    {
        if (_availableIndexes.Count == _bookListPlanes.Count)
        {
            _view.ToggleEmptyBookListPlane(true);
        }
        else
        {
            _view.ToggleEmptyBookListPlane(false);
        }
        
        _view.Enable();
    }

    private void OnAddBookListClicked()
    {
        AddBookListClicked?.Invoke();
        _view.Disable();
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
    }

    private void DisableAllWindows()
    {
        for (int i = 0; i < _bookListPlanes.Count; i++)
        {
            _bookListPlanes[i].Disable();
            _availableIndexes.Add(i);
        }
    }

    private void ActivateBookListPlane(BookListData bookListData)
    {
        if (_availableIndexes.Count > 0)
        {
            int index = _availableIndexes[0];
            _bookListPlanes[index].Enable();
            _bookListPlanes[index].SetData(bookListData);
            _bookListPlanes[index].BookListOpened += OnOpenBookList;
            _bookListPlanes[index].DataUpdated += SaveBookLists;
            _availableIndexes.RemoveAt(0);
            
            SaveBookLists();
        }
        
        if (_availableIndexes.Count < _bookListPlanes.Count)
        {
            _view.ToggleEmptyBookListPlane(false);
        }
        else
        {
            _view.ToggleEmptyBookListPlane(true);
        }
    }
    
    private void DeleteBookListPlane(BookListMainScreenPlane bookListMainScreenPlane)
    {
        if (bookListMainScreenPlane == null)
            throw new ArgumentNullException(nameof(bookListMainScreenPlane));
        
        _view.Enable();

        int index = _bookListPlanes.IndexOf(bookListMainScreenPlane);

        if (index >= 0)
        {
            _availableIndexes.Add(index);
            bookListMainScreenPlane.BookListOpened -= OnOpenBookList;
            bookListMainScreenPlane.DataUpdated -= SaveBookLists;
            bookListMainScreenPlane.ResetData();
            bookListMainScreenPlane.Disable();
            SaveBookLists();
        }

        if (_availableIndexes.Count == _bookListPlanes.Count)
        {
            _view.ToggleEmptyBookListPlane(true);
        }
    }

    private void OnOpenBookList(BookListMainScreenPlane bookList)
    {
        OpenBookListClicked?.Invoke(bookList);
        _view.Disable();
    }
    
    private void OnBookListDataSaved(BookListData bookListData)
    {
        ActivateBookListPlane(bookListData);
    }
    
    private void SaveBookLists()
    {
        List<BookListData> activeBookLists = new List<BookListData>();
        
        foreach (var plane in _bookListPlanes)
        {
            if (plane.IsActive)
            {
                activeBookLists.Add(plane.BookListData);
            }
        }

        BookListCollection collection = new BookListCollection { BookLists = activeBookLists };
        string json = JsonUtility.ToJson(collection, true);
        File.WriteAllText(_filePath, json);
    }
    
    private void LoadBookLists()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            
            BookListCollection collection = JsonUtility.FromJson<BookListCollection>(json);
            
            foreach (var bookListData in collection.BookLists)
            {
                ActivateBookListPlane(bookListData);
            }

        }
    }
}
