using System;
using System.ComponentModel.DataAnnotations;

namespace Lima.Admin.Api.Requests
{
    public class UserRequest
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите полное имя пользователя")]
        public string FullName { get; set; }
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите логин.")]
        [RegularExpression("([a-z]+_)([a-z]+)", ErrorMessage = "Отсутствует префикс.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Логин должен содержать минимум 6 и максимум 50 символов")]
        public string Login { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите пароль.")]
        public string Password { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Выберите роль.")]
        public int RoleId { get; set; }
        public DateTime Birthday { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
    }
}
