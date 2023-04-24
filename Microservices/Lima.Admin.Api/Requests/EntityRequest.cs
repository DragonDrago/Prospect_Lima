using System.ComponentModel.DataAnnotations;

namespace Lima.Admin.Api.Requests
{
    public class EntityRequest
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите название.")]
        public string Name { get; set; }
    }
}
