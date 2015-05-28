using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace task_1.Models.ProfileModel
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<BusinessCard> BusinessCards { get; set; }
        public virtual ICollection<Field> Fields { get; set; }

        public Profile()
        {
            BusinessCards = new List<BusinessCard>();
            Fields = new List<Field>();
        }
    }
}