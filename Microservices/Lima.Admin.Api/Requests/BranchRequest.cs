using System.ComponentModel.DataAnnotations;

namespace Lima.Admin.Api.Requests
{
    public class BranchRequest
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите название филиала.")]
        public string Name { get; set; }
        public string Address { get; set; }
        public int ChiefUserId { get; set; }
    }
}
