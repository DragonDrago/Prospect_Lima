using System.ComponentModel.DataAnnotations;

namespace Lima.Dictionaries.Api.Requests
{
    public class EntityRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название.", AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}
