using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EfcoreProp.Repository;

[Table("books")]
[PrimaryKey(nameof(BookId))]
class DbBook
{
    [Column("book_id")]
    public Guid BookId { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("publisher_id")]
    [ForeignKey(nameof(DbBook))]
    public Guid PublisherId { get; set; }


    public DbPublisher Publisher { get; set; }
}