namespace WebDauThauOnline.Models
{
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    public class EmpFileModel
    {
        [DataType(DataType.Upload)]
        public HttpPostedFileBase[] fileUpload { get; set; }
    }
}