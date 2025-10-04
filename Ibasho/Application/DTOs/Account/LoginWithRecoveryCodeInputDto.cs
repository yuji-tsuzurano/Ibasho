using System.ComponentModel.DataAnnotations;

namespace Ibasho.Application.DTOs.Account
{
    /// <summary>
    /// リカバリーコードログインの入力情報
    /// </summary>
    public class LoginWithRecoveryCodeInputDto
    {
        /// リカバリーコード
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Recovery Code")]
        public string RecoveryCode { get; set; } = string.Empty;
    }
}
