using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FleetTracker.Services
{
	public class OrsRoutingService
	{
		private readonly HttpClient _http;
		private readonly string _apiKey;

		public OrsRoutingService(IConfiguration config, HttpClient http)
		{
			_http = http;
			_apiKey = config["Ors:ApiKey"];

			if (string.IsNullOrWhiteSpace(_apiKey))
				throw new InvalidOperationException(
				  "Chiave ORS non configurata: controlla la sezione Ors:ApiKey in appsettings.json");
		}


		/// Geocoding: da indirizzo a [lon, lat]
		public async Task<double[]> GeocodeAsync(string address)
		{
			var url = $"geocode/search?api_key={_apiKey}&text={Uri.EscapeDataString(address)}&size=1";
			using var res = await _http.GetAsync(url);

			var body = await res.Content.ReadAsStringAsync();
			if (!res.IsSuccessStatusCode)
			{
				// ORS restituisce sempre un JSON con "error"
				using var errDoc = JsonDocument.Parse(body);
				var msg = errDoc.RootElement
								.GetProperty("error")
								.GetProperty("message")
								.GetString();
				throw new Exception($"Geocoding fallito ({(int)res.StatusCode}): {msg}");
			}

			using var doc = JsonDocument.Parse(body);
			var features = doc.RootElement.GetProperty("features");
			if (features.GetArrayLength() == 0)
				throw new Exception($"Nessun risultato di geocoding per “{address}”");

			var coords = features[0]
						 .GetProperty("geometry")
						 .GetProperty("coordinates");
			return new[] { coords[0].GetDouble(), coords[1].GetDouble() };
		}

		public async Task<JsonElement> GetRouteAsync(List<double[]> coords)
		{
			const int maxRetries = 3;
			var payload = JsonSerializer.Serialize(new { coordinates = coords });

			for (int attempt = 0; attempt <= maxRetries; attempt++)
			{
				// 1) Ricrea ogni volta la request (non è ri-inviabile)
				using var req = new HttpRequestMessage(
					HttpMethod.Post,
					"v2/directions/driving-car/geojson")
				{
					Content = new StringContent(payload, Encoding.UTF8, "application/json")
				};
				req.Headers.Authorization =
					new AuthenticationHeaderValue("Bearer", _apiKey);

				// 2) Invio
				using var res = await _http.SendAsync(req);
				var body = await res.Content.ReadAsStringAsync();

				// 3) Se OK, esci dal loop e parsifica
				if (res.IsSuccessStatusCode)
				{
					try
					{
						using var doc = JsonDocument.Parse(body);
						var root = doc.RootElement;
						if (!root.TryGetProperty("features", out var features)
							|| features.GetArrayLength() == 0)
							throw new Exception("ORS: nessun percorso restituito");

						return features[0]
									   .GetProperty("geometry")
									   .Clone();
					}
					catch (JsonException je)
					{
						throw new Exception($"Errore parsing GeoJSON: {je.Message}\nBody:\n{body}");
					}
				}

				// 4) Se è un transient server error (5xx), e non ho superato i retry
				if ((int)res.StatusCode >= 500 && (int)res.StatusCode < 600 && attempt < maxRetries)
				{
					// prova a leggere Retry-After
					TimeSpan delay = res.Headers.RetryAfter?.Delta
									  ?? TimeSpan.FromSeconds(Math.Pow(2, attempt + 1));

					Console.Error.WriteLine(
						$"ORS returned {(int)res.StatusCode}, retry in {delay.TotalSeconds}s (attempt {attempt + 1})");

					await Task.Delay(delay);
					continue;
				}

				// 5) Altrimenti è un errore definitivo: prova a parsificare JSON di errore
				if (res.Content.Headers.ContentType?.MediaType?.Contains("json") == true)
				{
					using var errDoc = JsonDocument.Parse(body);
					var msg = errDoc.RootElement
									.GetProperty("error")
									.GetProperty("message")
									.GetString();
					throw new Exception($"Routing fallito ({(int)res.StatusCode}): {msg}");
				}
				else
				{
					throw new Exception(
						$"Routing fallito ({(int)res.StatusCode}): risposta non JSON:\n{body}");
				}
			}

			// Non dovrebbe mai arrivare qui
			throw new Exception("Routing fallito: troppe richieste eccessive");
		}
	}
}
