using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace task_1.Models.ProfileModel
{
    public class BusinessCardToField
    {
        public int Id { get; set; }
        [Required]
        public int BusinessCardId { get; set; }
        [Required]
        public int FieldId { get; set; }
    }
}