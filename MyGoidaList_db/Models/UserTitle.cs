using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyGoidaList.Models
{
    [PrimaryKey(nameof(UserId), nameof(TitleId))]
    public class UserTitle
    {
        public int UserId { get; set; }

        public int TitleId { get; set; }

        public string Status { get; set; }
        public int Episodes { get; set; }
        public int Score { get; set; }

        public virtual User User { get; set; }
        public virtual Title Title { get; set; }
    }
}