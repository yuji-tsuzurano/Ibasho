using System.ComponentModel.DataAnnotations;

namespace Ibasho.Application.DTOs.Account
{
    /// <summary>
    /// メール確認再送の入力情報
    /// </summary>
    public class ResendEmailConfirmationInputDto
    {
        /// メールアドレス
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
