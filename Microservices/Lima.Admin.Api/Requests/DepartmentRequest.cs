using System.ComponentModel.DataAnnotations;

namespace Lima.Admin.Api.Requests
{
    public class DepartmentRequest
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите название отдела.")]
        public string Name { get; set; }
        public int BranchId { get; set; }
        public int ChiefUserId { get; set; }
    }
}
