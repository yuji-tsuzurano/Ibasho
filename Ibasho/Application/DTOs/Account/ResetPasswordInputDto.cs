using System.ComponentModel.DataAnnotations;

namespace Ibasho.Application.DTOs.Account
{
    /// <summary>
    /// パスワードリセットの入力情報
    /// </summary>
    public class ResetPasswordInputDto
    {
        /// メールアドレス
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// 新しいパスワード
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        /// 新しいパスワード（確認）
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        /// リセットコード
        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
