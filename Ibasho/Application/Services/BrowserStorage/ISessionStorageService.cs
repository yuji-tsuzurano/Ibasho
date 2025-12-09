namespace Ibasho.Application.Services.BrowserStorage;

/// <summary>
/// セッションストレージサービスのインターフェース
/// ブラウザのセッションストレージにデータを保存・取得するためのサービス
/// タブを閉じるとデータは消失する
/// </summary>
public interface ISessionStorageService
{
    /// <summary>
    /// セッションストレージから値を取得する
    /// </summary>
    /// <typeparam name="T">取得する値の型</typeparam>
    /// <param name="key">ストレージのキー</param>
    /// <returns>値（存在しない場合やエラー時はdefault）</returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// セッションストレージに値を保存する
    /// </summary>
    /// <typeparam name="T">保存する値の型</typeparam>
    /// <param name="key">ストレージのキー</param>
    /// <param name="value">保存する値</param>
    Task SetAsync<T>(string key, T value);

    /// <summary>
    /// セッションストレージから値を削除する
    /// </summary>
    /// <param name="key">ストレージのキー</param>
    Task RemoveAsync(string key);
}
