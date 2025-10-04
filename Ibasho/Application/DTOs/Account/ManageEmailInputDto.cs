using System.ComponentModel.DataAnnotations;

namespace Ibasho.Application.DTOs.Account
{
    /// <summary>
    /// メールアドレス変更の入力情報
    /// </summary>
    public class ManageEmailInputDto
    {
        /// 新しいメールアドレス
        [Required]
        [EmailAddress]
        [Display(Name = "New email")]
        public string? NewEmail { get; set; }
    }
}
