using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGoidaList.Models
{
    public class AddTitleListViewModel
    {
        public int UserId { get; set; }
        public List<Title> Titles { get; set; }
    }
}