using System.ComponentModel.DataAnnotations;

namespace Ibasho.Application.DTOs.Account
{
    /// <summary>
    /// 2要素認証ログインの入力情報
    /// </summary>
    public class LoginWith2faInputDto
    {
        /// 認証コード
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string? TwoFactorCode { get; set; }

        /// この端末を記憶するか
        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }
    }
}
