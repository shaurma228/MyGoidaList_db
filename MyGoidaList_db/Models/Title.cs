using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyGoidaList.Models
{
    public class Title
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Episodes { get; set; }
        public int Score { get; set; }

        public virtual ICollection<UserTitle> UserTitles { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
    }
}