namespace Ibasho.Application.DTOs.Likes
{
    /// <summary>
    /// いいね切替結果DTO
    /// </summary>
    public class ToggleLikeResultDto
    {
        /// <summary>
        /// 切替後のいいね状態（true:いいね false:未いいね）
        /// </summary>
        public bool IsLiked { get; set; }

        /// <summary>
        /// 投稿のいいね数
        /// </summary>
        public int LikeCount { get; set; }
    }
}
