using Ibasho.Application.Services.BrowserStorage;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Ibasho.Infrastructure.Services.BrowserStorage;

/// <summary>
/// セッションストレージサービスの実装
/// ProtectedSessionStorageを使用して暗号化されたセッションストレージアクセスを提供
/// タブを閉じるとデータは消失する
/// </summary>
/// <param name="sessionStorage">Blazorの暗号化セッションストレージ</param>
public sealed class SessionStorageService(ProtectedSessionStorage sessionStorage) : ISessionStorageService
{
    private readonly ProtectedSessionStorage _sessionStorage = sessionStorage;

    /// <summary>
    /// セッションストレージから値を取得する
    /// プリレンダリング時やストレージアクセス失敗時はdefaultを返す
    /// </summary>
    /// <typeparam name="T">取得する値の型</typeparam>
    /// <param name="key">ストレージのキー</param>
    /// <returns>値（存在しない場合やエラー時はdefault）</returns>
    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var result = await _sessionStorage.GetAsync<T>(key);
            return result.Success ? result.Value : default;
        }
        catch
        {
            // プリレンダリング時やJavaScript未初期化時は例外が発生するため無視
            return default;
        }
    }

    /// <summary>
    /// セッションストレージに値を保存する
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
            await _sessionStorage.SetAsync(key, value);
        }
        catch
        {
            // プリレンダリング時やJavaScript未初期化時は例外が発生するため無視
        }
    }

    /// <summary>
    /// セッションストレージから値を削除する
    /// </summary>
    /// <param name="key">ストレージのキー</param>
    public async Task RemoveAsync(string key)
    {
        try
        {
            await _sessionStorage.DeleteAsync(key);
        }
        catch
        {
            // プリレンダリング時やJavaScript未初期化時は例外が発生するため無視
        }
    }
}
