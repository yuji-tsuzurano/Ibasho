using System.ComponentModel.DataAnnotations;

namespace Ibasho.Application.DTOs.Account
{
    /// <summary>
    /// 個人データ削除の入力情報
    /// </summary>
    public class DeletePersonalDataInputDto
    {
        /// パスワード
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
