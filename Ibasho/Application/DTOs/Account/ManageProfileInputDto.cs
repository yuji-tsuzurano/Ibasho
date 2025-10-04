using System.ComponentModel.DataAnnotations;

namespace Ibasho.Application.DTOs.Account
{
    /// <summary>
    /// プロファイル編集の入力情報
    /// </summary>
    public class ManageProfileInputDto
    {
        /// 電話番号
        [Phone]
        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }
    }
}
