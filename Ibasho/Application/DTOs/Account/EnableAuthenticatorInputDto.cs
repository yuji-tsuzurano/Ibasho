using System.ComponentModel.DataAnnotations;

namespace Ibasho.Application.DTOs.Account
{
    /// <summary>
    /// 認証アプリ有効化の入力情報
    /// </summary>
    public class EnableAuthenticatorInputDto
    {
        /// 検証コード
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; } = string.Empty;
    }
}
