using System.ComponentModel.DataAnnotations;

namespace Siniauski.WhatIWant.WebData.Contracts.Requests
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "Введите имя!")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Введите фамилию!")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Введите логин!")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\*\.!@\$%\^&\(\){}\[\]:;<>,.\?/~_\+-=\|\\]).{8,32}$",
            ErrorMessage = "Пароль должен иметь длину от 8 до 32 символов и содержать строчную и прописную латинские буквы, цифру и специальный символ!")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Введите подтверждение пароля!")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают!")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный почтовый адрес!")]
        [Required(ErrorMessage = "Введите Email!")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Некорректный телефонный номер!")]
        public string? PhoneNumber { get; set; }

        
        [RegularExpression(@"^.*\.(jpg|jpeg|png)$",
            ErrorMessage = "Файл аватара должен иметь расширение jpg, jpeg или png!")]
        public string? Avatar { get; set; }

        public string? AvatarServerName { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}