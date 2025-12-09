namespace Ibasho.Application.Services.BrowserStorage;

/// <summary>
/// ローカルストレージサービスのインターフェース
/// ブラウザのローカルストレージにデータを保存・取得するためのサービス
/// ブラウザを閉じてもデータは永続化される
/// </summary>
public interface ILocalStorageService
{
    /// <summary>
    /// ローカルストレージから値を取得する
    /// </summary>
    /// <typeparam name="T">取得する値の型</typeparam>
    /// <param name="key">ストレージのキー</param>
    /// <returns>値（存在しない場合やエラー時はdefault）</returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// ローカルストレージに値を保存する
    /// </summary>
    /// <typeparam name="T">保存する値の型</typeparam>
    /// <param name="key">ストレージのキー</param>
    /// <param name="value">保存する値</param>
    Task SetAsync<T>(string key, T value);

    /// <summary>
    /// ローカルストレージから値を削除する
    /// </summary>
    /// <param name="key">ストレージのキー</param>
    Task RemoveAsync(string key);
}
