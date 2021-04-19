
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models
{
    [Table("Images")]
    public class StoredImage //: HeaderData
    {
        //public StoredImage(string ownerId) : base(ownerId) { }

        [Key]
        public string FileName { get; set; }
        public byte[] FileContents { get; set; }
        public string Extension { get; set; }
    }
}
