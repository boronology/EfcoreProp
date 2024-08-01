using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EfcoreProp.Repository;

[Table("publishers")]
[PrimaryKey(nameof(PublisherId))]
class DbPublisher
{
    [Column("publisher_id")]
    public Guid PublisherId { get; set; }

    [Column("name")]
    public string Name { get; set; }

    public List<DbBook> Books { get; set; } = [];
}