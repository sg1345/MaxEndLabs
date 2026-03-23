using System.Net.Http.Json;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.Services.Core.Models.Configuration;
using Microsoft.Extensions.Options;

namespace MaxEndLabs.Services.Core
{
	public class ReCaptchaService : IReCaptchaService
	{
		private readonly HttpClient _http;
		private readonly GoogleReCaptchaSettings _captchaSettings;

		public ReCaptchaService(HttpClient http, IOptions<GoogleReCaptchaSettings> captchaSettings)
		{
			this._http = http;
			this._captchaSettings = captchaSettings.Value;
		}
		private sealed class ReCaptchaResponse
		{
			public bool Success { get; set; }
			public List<string> ErrorCodes { get; set; } = new();
		}

		public async Task<bool> VerifyAsync(string? token, CancellationToken ct = default)
		{
			if (string.IsNullOrWhiteSpace(token)) return false;

			var url = $"https://www.google.com/recaptcha/api/siteverify?secret={_captchaSettings.SecretKey}&response={token}";
			using var res = await _http.PostAsync(url, content: null, ct);
			if (!res.IsSuccessStatusCode) return false;

			var body = await res.Content.ReadFromJsonAsync<ReCaptchaResponse>(cancellationToken: ct);
			return body?.Success == true;
		}

	}
}
