using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyGoidaList.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        public string Nickname { get; set; }
        public int TitlesSum { get; set; }

        public virtual ICollection<UserTitle> UserTitles { get; set; }
    }
}