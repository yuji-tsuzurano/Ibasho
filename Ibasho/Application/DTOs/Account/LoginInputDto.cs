using System.ComponentModel.DataAnnotations;

namespace Ibasho.Application.DTOs.Account
{
    /// <summary>
    /// ログインの入力情報
    /// </summary>
    public class LoginInputDto
    {
        /// メールアドレス
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        /// パスワード
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        /// ログイン状態を保持するか
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
