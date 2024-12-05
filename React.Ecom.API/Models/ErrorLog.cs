using System.ComponentModel.DataAnnotations;

namespace React.Ecom.API.Models
{
    public class ErrorLog
    {
        [Key]
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
        public string Error { get; set; }
    }
}
