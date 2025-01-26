using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGoidaList.Models
{
    public class EditTitleViewModel
    {
        public int UserId { get; set; }
        public int TitleId { get; set; }
        public string Status { get; set; }
        public int Episodes { get; set; }
        public int Score { get; set; }
        public int MaxEpisodes { get; set; }
    }
}