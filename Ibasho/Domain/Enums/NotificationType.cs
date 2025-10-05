namespace Ibasho.Domain.Enums
{
    /// <summary>
    /// 通知の種類を表す列挙型
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// いいね通知（投稿にいいねされた時）
        /// </summary>
        Like = 1,
        
        /// <summary>
        /// フォロー通知（ユーザーにフォローされた時）
        /// </summary>
        Follow = 2,
        
        /// <summary>
        /// リプライ通知（投稿にリプライされた時）
        /// </summary>
        Reply = 3,
        
        /// <summary>
        /// メンション通知（投稿で@メンションされた時）
        /// </summary>
        Mention = 4
    }
}