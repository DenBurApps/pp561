using System;
using UnityEngine;

[Serializable]
public class BookData
{
    public Genres Genre;
    public Status Status;
    public string BookTitle;
    public string ImagePath;
    public bool FavouriteSelected;
    public string Author;
    public string PublicationDate;
    public string Description;
    public int Stars;
    public string Review;

    public BookData(Genres genre, string bookTitle, string author, string description, string publicationYear)
    {
        Genre = genre;
        BookTitle = bookTitle;
        Author = author;
        PublicationDate = publicationYear;
        Description = description;
        Status = Status.None;
        ImagePath = string.Empty;
        Review = string.Empty;
        Stars = default;

        FavouriteSelected = false;
    }
}

public enum Genres
{
    Fiction,
    Comics,
    ForeignLanguages,
    Philology,
    ChildrenBook,
    Education,
    Esoterica,
    MedicineAndHealth,
    HomeAndHobbies,
    ScienceTechnologyIT,
    ReligionAndPhilosophy,
    Psychology,
    Art,
    Economics,
    HistorySociety,
    Law
}

public enum Status
{
    Planned,
    InProcess,
    Read,
    None
}

public static class GenreExtensions
{
    public static string ToString(this Genres genre)
    {
        switch (genre)
        {
            case Genres.Fiction: return "Fiction";
            case Genres.Comics: return "Comics/Manga";
            case Genres.ForeignLanguages: return "Foreign Languages";
            case Genres.Philology: return "Philology";
            case Genres.ChildrenBook: return "Children's Book";
            case Genres.Education: return "Education";
            case Genres.Esoterica: return "Esoterica";
            case Genres.MedicineAndHealth: return "Medicine and Health";
            case Genres.HomeAndHobbies: return "Home and Hobbies";
            case Genres.ScienceTechnologyIT: return "Science. Technology. IT";
            case Genres.ReligionAndPhilosophy: return "Religion and Philosophy";
            case Genres.Psychology: return "Psychology";
            case Genres.Art: return "Art";
            case Genres.Economics: return "Economics";
            case Genres.HistorySociety: return "History. Society";
            case Genres.Law: return "Law";
            default: return genre.ToString();
        }
    }
}
