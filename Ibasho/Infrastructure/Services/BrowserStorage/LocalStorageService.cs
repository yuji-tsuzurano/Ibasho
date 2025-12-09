using Ibasho.Application.Services.BrowserStorage;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Ibasho.Infrastructure.Services.BrowserStorage;

/// <summary>
/// ローカルストレージサービスの実装
/// ProtectedLocalStorageを使用して暗号化されたローカルストレージアクセスを提供
/// ブラウザを閉じてもデータは永続化される
/// </summary>
/// <param name="localStorage">Blazorの暗号化ローカルストレージ</param>
public sealed class LocalStorageService(ProtectedLocalStorage localStorage) : ILocalStorageService
{
    private readonly ProtectedLocalStorage _localStorage = localStorage;

    /// <summary>
    /// ローカルストレージから値を取得する
    /// プリレンダリング時やストレージアクセス失敗時はdefaultを返す
    /// </summary>
    /// <typeparam name="T">取得する値の型</typeparam>
    /// <param name="key">ストレージのキー</param>
    /// <returns>値（存在しない場合やエラー時はdefault）</returns>
    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var result = await _localStorage.GetAsync<T>(key);
            return result.Success ? result.Value : default;
        }
        catch
        {
            // プリレンダリング時やJavaScript未初期化時は例外が発生するため無視
            return default;
        }
    }

    /// <summary>
    /// ローカルストレージに値を保存する
    /// 値がnullの場合は何もしない
    /// </summary>
    /// <typeparam name="T">保存する値の型</typeparam>
    /// <param name="key">ストレージのキー</param>
    /// <param name="value">保存する値</param>
    public async Task SetAsync<T>(string key, T value)
    {
        if (value is null) return;

        try
        {
            await _localStorage.SetAsync(key, value);
        }
        catch
        {
            // プリレンダリング時やJavaScript未初期化時は例外が発生するため無視
        }
    }

    /// <summary>
    /// ローカルストレージから値を削除する
    /// </summary>
    /// <param name="key">ストレージのキー</param>
    public async Task RemoveAsync(string key)
    {
        try
        {
            await _localStorage.DeleteAsync(key);
        }
        catch
        {
            // プリレンダリング時やJavaScript未初期化時は例外が発生するため無視
        }
    }
}
