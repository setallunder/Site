using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace task_1.Models.ProfileModel
{
    public class Field
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int OffsetTop { get; set; }
        public int OffsetLeft { get; set; }
    }
}