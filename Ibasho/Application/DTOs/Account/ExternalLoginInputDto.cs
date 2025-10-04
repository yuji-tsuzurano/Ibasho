using System.ComponentModel.DataAnnotations;

namespace Ibasho.Application.DTOs.Account
{
    /// <summary>
    /// 外部ログイン連携時の入力情報
    /// </summary>
    public class ExternalLoginInputDto
    {
        /// メールアドレス
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
