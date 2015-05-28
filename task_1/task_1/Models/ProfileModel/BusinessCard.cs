using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using task_1.Models.ProfileModel;

namespace task_1.Models
{
    public class BusinessCard
    {
        public int Id { get; set; }
        public string Template { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public ICollection<Field> Fields { get; set; }
        public BusinessCard()
        {
            Fields = new List<Field>();
        }
    }
}