using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CrudWithImage1.ViewModel
{
    public class UserProfile
    {
        //public int ID { get; set; }
        public string FullName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [DisplayName("Picture")]
        public string PicturePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase Pic { get; set; }
    }
}