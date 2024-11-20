using System;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private EditBookSecondScreen _editBookSecondScreen;
    [SerializeField] private EditBookFirstScreen _editBookFirstScreen;
    [SerializeField] private MainScreenView _view;
    [SerializeField] private MainScreenStatusScroll _statusScroll;
    [SerializeField] private List<FilledBookPlane> _bookPlanes = new List<FilledBookPlane>();
    [SerializeField] private ScreenStateManager _screenStateManager;
    [SerializeField] private AddBookSecondScreen _addBookSecondScreen;

    private List<int> _availableIndexes = new List<int>();
    private const string SaveKey = "BookDataList";

    public event Action AddBookClicked;
    public event Action AddBookListClicked;
    public event Action SettingsClicked;
    public event Action<FilledBookPlane> EditBookPlane;

    public List<FilledBookPlane> BookPlanes => _bookPlanes;

    private void Start()
    {
        _availableIndexes.Clear();
        _view.Enable();
        LoadBooks();
    }

    private void OnEnable()
    {
        _view.AddBookButtonClicked += OnAddBookClicked;
        _view.AddBookListButtonClicked += OnAddBookListClicked;
        _view.SettingsButtonClicked += OnSettingsClicked;

        _screenStateManager.MainScreenOpen += _view.Enable;
        _screenStateManager.SettingsDisabled += _view.Disable;
        _screenStateManager.SettingsOpened += _view.SetTransperent;

        _addBookSecondScreen.BookSaved += AddNewBook;
        _view.SearchTextInputed += SearchBook;

        _statusScroll.AllButtonClicked += EnableAllWindows;
        _statusScroll.PlannedButtonClicked += (() => EnableStatusBook(Status.Planned));
        _statusScroll.InTheProcessButtonClicked += () => EnableStatusBook(Status.InProcess);
        _statusScroll.ReadButtonClicked += () => EnableStatusBook(Status.Read);
        _statusScroll.FavouritesButtonClicked += EnableFavouriteBooks;

        _editBookFirstScreen.DeleteButtonClicked += DeleteBook;
        _editBookSecondScreen.DeleteButtonClicked += DeleteBook;
        _editBookSecondScreen.ReturnToMainMenu += _view.Enable;
    }

    private void OnDisable()
    {
        _view.AddBookButtonClicked -= OnAddBookClicked;
        _view.AddBookListButtonClicked -= OnAddBookListClicked;
        _view.SettingsButtonClicked -= OnSettingsClicked;
        _screenStateManager.MainScreenOpen -= OpenScreen;
        _screenStateManager.MainScreenOpen -= _view.SetTransperent;
        _addBookSecondScreen.BookSaved -= AddNewBook;
        _view.SearchTextInputed -= SearchBook;

        _statusScroll.AllButtonClicked -= EnableAllWindows;
        _statusScroll.PlannedButtonClicked -= (() => EnableStatusBook(Status.Planned));
        _statusScroll.InTheProcessButtonClicked -= (() => EnableStatusBook(Status.InProcess));
        _statusScroll.ReadButtonClicked -= (() => EnableStatusBook(Status.Read));
        _statusScroll.FavouritesButtonClicked -= EnableFavouriteBooks;

        _editBookFirstScreen.DeleteButtonClicked -= DeleteBook;
        _editBookSecondScreen.DeleteButtonClicked -= DeleteBook;
        _editBookSecondScreen.ReturnToMainMenu -= OpenScreen;

        SaveBooks();
    }

    private void OpenScreen()
    {
        EnableAllWindows();
        _view.Enable();
    }

    public void SaveBooks()
    {
        List<BookData> booksToSave = new List<BookData>();

        foreach (var bookPlane in _bookPlanes)
        {
            if (bookPlane.IsActive)
            {
                booksToSave.Add(bookPlane.CurrentData);
            }
        }

        string json = JsonHelper.ToJson(booksToSave.ToArray(), true);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadBooks()
    {
        _availableIndexes.Clear();
        DisableAllWindows();

        if (!PlayerPrefs.HasKey(SaveKey))
        {
            return;
        }

        string json = PlayerPrefs.GetString(SaveKey);
        BookData[] loadedBooks = JsonHelper.FromJson<BookData>(json);

        for (int i = 0; i < loadedBooks.Length; i++)
        {
            if (_availableIndexes.Count > 0)
            {
                int availableIndex = _availableIndexes[0];
                _availableIndexes.RemoveAt(0);

                var currentBookDataWindow = _bookPlanes[availableIndex];

                if (!currentBookDataWindow.IsActive)
                {
                    currentBookDataWindow.Enable();
                    currentBookDataWindow.EditButtonClicked += EditBookData;
                    currentBookDataWindow.DataChanged += SaveBooks;
                    currentBookDataWindow.SetBookData(loadedBooks[i]);
                }
            }
        }

        if (_availableIndexes.Count < _bookPlanes.Count)
        {
            _view.ToggleEmptyPlane(false);
        }
        else
        {
            _view.ToggleEmptyPlane(true);
        }
    }

    private void EnableAllWindows()
    {
        foreach (var bookPlane in _bookPlanes)
        {
            if (!bookPlane.IsActive)
                continue;

            bookPlane.Enable();
            bookPlane.EnablePicture();
        }
    }

    private void EnableFavouriteBooks()
    {
        foreach (var bookPlane in _bookPlanes)
        {
            if (!bookPlane.IsActive)
                continue;

            if (bookPlane.CurrentData.FavouriteSelected)
            {
                bookPlane.Enable();
                bookPlane.EnablePicture();
            }
            else
            {
                bookPlane.Disable();
            }
        }
    }

    private void EnableStatusBook(Status status)
    {
        foreach (var bookPlane in _bookPlanes)
        {
            if (!bookPlane.IsActive)
                continue;

            if (bookPlane.CurrentData.Status == status)
            {
                bookPlane.Enable();
                bookPlane.EnablePicture();
            }
            else
            {
                bookPlane.Disable();
            }
        }
    }

    private void SearchBook(string search)
    {
        string adaptedSearch = search.ToLower();

        foreach (var bookPlane in _bookPlanes)
        {
            if (!bookPlane.IsActive)
                continue;

            if (bookPlane.CurrentData.BookTitle.ToLower() == adaptedSearch ||
                bookPlane.CurrentData.Author.ToLower() == adaptedSearch)
            {
                bookPlane.Enable();
                bookPlane.EnablePicture();
            }
            else
            {
                bookPlane.Disable();
            }
        }
    }

    private void AddNewBook(BookData bookData)
    {
        if (bookData == null)
            throw new ArgumentNullException(nameof(bookData));

        if (string.IsNullOrEmpty(bookData.BookTitle) || string.IsNullOrEmpty(bookData.Author) ||
            string.IsNullOrEmpty(bookData.PublicationDate)
            || string.IsNullOrEmpty(bookData.Description))
            return;

        if (_availableIndexes.Count > 0)
        {
            int availableIndex = _availableIndexes[0];
            _availableIndexes.RemoveAt(0);

            var currentBookDataWindow = _bookPlanes[availableIndex];

            if (!currentBookDataWindow.IsActive)
            {
                currentBookDataWindow.Enable();
                currentBookDataWindow.EditButtonClicked += EditBookData;
                currentBookDataWindow.DataChanged += SaveBooks;
                currentBookDataWindow.SetBookData(bookData);
            }

            EnableAllWindows();
            SaveBooks();
        }

        if (_availableIndexes.Count < _bookPlanes.Count)
        {
            _view.ToggleEmptyPlane(false);
        }
        else
        {
            _view.ToggleEmptyPlane(true);
        }
    }

    private void DeleteBook(FilledBookPlane bookPlane)
    {
        if (bookPlane == null)
            throw new ArgumentNullException(nameof(bookPlane));

        _view.Enable();

        int index = _bookPlanes.IndexOf(bookPlane);

        if (index >= 0 && !_availableIndexes.Contains(index))
        {
            _availableIndexes.Add(index);
        }

        bookPlane.EditButtonClicked -= EditBookData;
        bookPlane.DataChanged -= SaveBooks;
        bookPlane.DeleteData();
        bookPlane.Disable();
        SaveBooks();

        if (_availableIndexes.Count < _bookPlanes.Count)
        {
            _view.ToggleEmptyPlane(false);
        }
        else
        {
            _view.ToggleEmptyPlane(true);
        }
    }

    private void EditBookData(FilledBookPlane bookPlane)
    {
        if (bookPlane == null)
            throw new ArgumentNullException(nameof(bookPlane));

        EditBookPlane?.Invoke(bookPlane);
        _view.Disable();
    }

    private void DisableAllWindows()
    {
        for (int i = 0; i < _bookPlanes.Count; i++)
        {
            _bookPlanes[i].Disable();
            _availableIndexes.Add(i);
        }

        if (_availableIndexes.Count < _bookPlanes.Count)
        {
            _view.ToggleEmptyPlane(false);
        }
        else
        {
            _view.ToggleEmptyPlane(true);
        }
    }

    private void OnAddBookClicked()
    {
        AddBookClicked?.Invoke();
        _view.Disable();
    }

    private void OnAddBookListClicked()
    {
        AddBookListClicked?.Invoke();
        _view.Disable();
    }

    private void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
        _view.SetTransperent();
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint = false)
    {
        Wrapper<T> wrapper = new Wrapper<T> { Items = array };
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}