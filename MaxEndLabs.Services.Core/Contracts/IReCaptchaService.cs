using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Services.Core.Contracts
{
	public interface IReCaptchaService
	{
		Task<bool> VerifyAsync(string? token, CancellationToken ct = default);
	}
}
