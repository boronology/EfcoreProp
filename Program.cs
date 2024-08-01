using EfcoreProp.Repository;

using Microsoft.EntityFrameworkCore;

namespace EfcoreProp;

public class EfcorePropConsole
{
    public static async Task Main()
    {
        await TruncateDatabase();
        await SeedDatabase();

        await GetOneBook();
        await GetOneBookIncludePublisher();
        await GetOneBookAfterGetPublisher();
        await GetAllBooksAfterGetOnePublisher();
        await GetOneBookWithoutTrackingAfterGetPublisher();

        await GetOneBookByPublisher();
    }

    private static async Task TruncateDatabase()
    {
        using var context = new DatabaseContext();
        await context.Books.ExecuteDeleteAsync();
        await context.Publishers.ExecuteDeleteAsync();
    }

    private static void ShowBook(DbBook dbBook)
    {
        Console.WriteLine($"BookId        :{dbBook.BookId}");
        Console.WriteLine($"Title         :{dbBook.Title}");
        Console.WriteLine($"PublisherId   :{dbBook.PublisherId}");
        Console.WriteLine($"PublisherName :{dbBook?.Publisher?.Name ?? "(NULL)"}");
    }

    private static async Task GetOneBook()
    {
        Console.WriteLine("* DBからDbBookを1件取得");
        using var context = new DatabaseContext();
        var book = await context.Books.FirstOrDefaultAsync(e => e.BookId == Guid.Parse("8bedf209-39fe-4bfa-8bec-86cf54925a76"));
        ShowBook(book);
    }

    private static async Task GetOneBookIncludePublisher()
    {
        Console.WriteLine("* DBからPublisherつきでDbBookを1件取得");
        using var context = new DatabaseContext();
        var book = await context.Books.Include(e => e.Publisher)
                        .FirstOrDefaultAsync(e => e.BookId == Guid.Parse("8bedf209-39fe-4bfa-8bec-86cf54925a76"));
        ShowBook(book);
    }

    private static async Task GetOneBookAfterGetPublisher()
    {
        Console.WriteLine("* DBからまずDbPublisherを取得し、そのあとそのDbPublisherに紐づくDbBookを取得");
        using var context = new DatabaseContext();

        _ = await context.Publishers.FirstOrDefaultAsync(e => e.PublisherId == Guid.Parse("10cf6487-563a-4e54-9a88-5eeb55623faa"));
        var book = await context.Books.FirstOrDefaultAsync(e => e.BookId == Guid.Parse("8bedf209-39fe-4bfa-8bec-86cf54925a76"));
        ShowBook(book);
    }

    private static async Task GetAllBooksAfterGetOnePublisher()
    {
        Console.WriteLine("* DBからまずDbPublisherを1件取得し、そのあとそのDbBookをすべて取得");
        using var context = new DatabaseContext();

        _ = await context.Publishers.FirstOrDefaultAsync(e => e.PublisherId == Guid.Parse("10cf6487-563a-4e54-9a88-5eeb55623faa"));
        var books = await context.Books.ToArrayAsync();
        foreach (var book in books)
        {
            ShowBook(book);
        }
    }

    private static async Task GetOneBookWithoutTrackingAfterGetPublisher()
    {
        Console.WriteLine("* DBからまずDbPublisherを取得し、そのあとそのDbPublisherに紐づくDbBookをAsNoTrackingで取得");
        using var context = new DatabaseContext();

        _ = await context.Publishers.FirstOrDefaultAsync(e => e.PublisherId == Guid.Parse("10cf6487-563a-4e54-9a88-5eeb55623faa"));
        var book = await context.Books.AsNoTracking().FirstOrDefaultAsync(e => e.BookId == Guid.Parse("8bedf209-39fe-4bfa-8bec-86cf54925a76"));
        ShowBook(book);
    }

    private static async Task GetOneBookByPublisher()
    {
        Console.WriteLine("* 番外編 : Publisher.Nameを経由してBookを取得");
        using var context = new DatabaseContext();

        var book = await context.Books.FirstOrDefaultAsync(e => e.Publisher.Name == "翔泳社");
        ShowBook(book);
    }

    private static async Task SeedDatabase()
    {
        using var context = new DatabaseContext();
        context.Publishers.AddRange(
        [
            new DbPublisher
            {
                PublisherId = Guid.Parse("c872a122-73f8-4720-8d08-65814dc3ecfc"),
                Name = "技術評論社",
            },
            new DbPublisher
            {
                PublisherId = Guid.Parse("acb75290-ef7d-44f8-b271-daebe35f62ee"),
                Name = "翔泳社",
            },
            new DbPublisher
            {
                PublisherId = Guid.Parse("10cf6487-563a-4e54-9a88-5eeb55623faa"),
                Name = "オーム社"
            }
        ]);
        context.Books.AddRange(
        [
            new DbBook
            {
                BookId = Guid.Parse("8bedf209-39fe-4bfa-8bec-86cf54925a76"),
                Title = "達人プログラマー",
                PublisherId = Guid.Parse("10cf6487-563a-4e54-9a88-5eeb55623faa"),
            },
            new DbBook
            {
                BookId = Guid.Parse("96b1c2fc-1372-4359-a0f1-bdff9c6443f4"),
                Title = "正規表現技術入門",
                PublisherId = Guid.Parse("c872a122-73f8-4720-8d08-65814dc3ecfc"),
            },
            new DbBook
            {
                BookId = Guid.Parse("6b83ccf1-ffe7-4153-9816-cb009e710009"),
                Title = "暗号技術のすべて",
                PublisherId = Guid.Parse("acb75290-ef7d-44f8-b271-daebe35f62ee"),
            }
        ]);
        await context.SaveChangesAsync();
    }
}