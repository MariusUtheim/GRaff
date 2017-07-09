using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GRaff.Randomness
{
	/// <summary>
	/// Represents static methods that provide true random number generations by querying www.random.org.
	/// </summary>
	public static class TrueRandom
	{
		private static JavaScriptSerializer serializer = new JavaScriptSerializer();
        private const string baseUrl = "https://api.random.org/json-rpc/1/invoke";
		private const string apiKey = "8abefec4-23a5-4c2f-9c05-0eb1b493cd3e";
		private const string contentType = "application/json-rpc";

		/// <summary>
		/// Generates a list of true random integers.
		/// </summary>
		/// <param name="n">The number of values to generate. Must be in the range [1, 1e4].</param>
		/// <param name="min">The inclusive lower bound of the generated values. Must be in the range [-1e9, 1e9].</param>
		/// <param name="max">The inclusive upper bound of the generated values. Must be in the range [-1e9, 1e9], and strictly greater than min.</param>
		/// <param name="replacement">Specifies whether the generated values should be selected with replacement, or if they should all be unique.</param>
		/// <returns>A System.Threading.Tasks.Task that upon completion will result in an array of n true random integers.</returns>
		public static async Task<int[]> IntegersAsync(int n, int min, int max, bool replacement)
		{
			Contract.Requires<ArgumentOutOfRangeException>(n >= 1 && n <= 1e4);
			Contract.Requires<ArgumentOutOfRangeException>(min >= -1e9 && min <= 1e9);
			Contract.Requires<ArgumentOutOfRangeException>(max >= -1e9 && max <= 1e9);
			Contract.Requires<ArgumentOutOfRangeException>(max > min);
			Contract.Requires<ArgumentOutOfRangeException>(replacement || (n > max - min));

			var requestData = _buildRequest("generateIntegers", new Dictionary<string, object>
			{
				{ "n", n },
				{ "min", min },
				{ "max", max },
				{ "replacement", replacement },
			});

			var responseString = await _makeRequestAsync(requestData);
			var response = serializer.Deserialize<_Response>(responseString);
			if (response.Error != null)
				throw new WebException("An unhandled error occurred when querying random.org: " + response.Error.Message + " (code: " + response.Error.Code.ToString() + ")");
			return response.Result.Random.Data.Cast<int>().ToArray();
		}

		/// <summary>
		/// Generates a list of true random decimals in the interval [0, 1)
		/// </summary>
		/// <param name="n">The number of values to generate. Must be in the range [1, 1e4].</param>
		/// <param name="decimalPlaces">The number of decimal places in the generated valeus. Must be in the range [1, 20].</param>
		/// <param name="replacement">Specifies whether the generated values should be selected with replacement, or if they should all be unique.</param>
		/// <returns>A System.Threading.Tasks.Task that upon completion will result in an array of n true random decimals in the range [0, 1).</returns>
		public static async Task<decimal[]> Decimals(int n, int decimalPlaces, bool replacement)
		{
			Contract.Requires<ArgumentOutOfRangeException>(n >= 1 && n <= 1e4);
			Contract.Requires<ArgumentOutOfRangeException>(decimalPlaces >= 1 && decimalPlaces <= 14);
			Contract.Requires<ArgumentOutOfRangeException>(replacement || (n <= Math.Pow(10, decimalPlaces)));

			var requestData = _buildRequest("generateDecimalFractions", new Dictionary<string, object>
			{
				{ "n", n },
				{ "decimalPlaces", decimalPlaces },
				{ "replacement", replacement }
			});

			var responseString = await _makeRequestAsync(requestData);
			var response = serializer.Deserialize<_Response>(responseString);
			if (response.Error != null)
				throw new WebException("An unhandled error occurred when querying random.org: " + response.Error.Message + " (code: " + response.Error.Code.ToString() + ")");
			return response.Result.Random.Data.Select(obj => Convert.ToDecimal(obj)).ToArray();
		}

		/// <summary>
		/// Generates a list of true random numbers from a Gaussian distribution with the specified mean and standard deviation.
		/// </summary>
		/// <param name="n">The number of values to generate. Must be in the range [1, 1e4].</param>
		/// <param name="mean">The mean of the Gaussian distribution. Must be in the range [-1e6, 1e6].</param>
		/// <param name="standardDeviation">The standard deviation of the Gaussian distribution. Must be in the range [-1e6, 1e6].</param>
		/// <param name="significantDigits">The number of significant digits in the returned values. Must be in the range [2, 20].</param>
		/// <returns>A System.Threading.Tasks.Task that upon completion will result in an array of true random normally distributed values.</returns>
		public static async Task<decimal[]> Gaussians(int n, double mean, double standardDeviation, int significantDigits)
		{
			Contract.Requires<ArgumentOutOfRangeException>(n >= 1 && n <= 1e4);
			Contract.Requires<ArgumentOutOfRangeException>(mean >= -1e6 && mean <= 1e6);
			Contract.Requires<ArgumentOutOfRangeException>(significantDigits >= 2 && significantDigits <= 20);

			var requestData = _buildRequest("generateGaussians", new Dictionary<string, object>
			{
				{ "n", n },
				{ "mean", mean },
				{ "standardDeviation", standardDeviation },
				{ "significantDigits", significantDigits }
			});

			var responseString = await _makeRequestAsync(requestData);
			var response = serializer.Deserialize<_Response>(responseString);
			if (response.Error != null)
				throw new WebException("An unhandled error occurred when querying random.org: " + response.Error.Message + " (code: " + response.Error.Code.ToString() + ")");
			return response.Result.Random.Data.Select(obj => Convert.ToDecimal(obj)).ToArray();
		}

		/// <summary>
		/// Generates a list of true random strings with the specified length, selecting from the specified characters.
		/// </summary>
		/// <param name="n">The number of strings to generate. Must be in the range [1, 1e4].</param>
		/// <param name="length">The length of each generated string. Must be in the range [1, 20].</param>
		/// <param name="characters">The characters to use in the generated strings. At most 80 characters are allowed.</param>
		/// <param name="replacement">Specifies whether the generated strings should be selected with replacement, or if they should all be unique.</param>
		/// <returns>A System.Threading.Tasks.Task that upon completion will result in an array of true random strings.</returns>
		public static async Task<string[]> Strings(int n, int length, string characters, bool replacement)
		{
			Contract.Requires<ArgumentOutOfRangeException>(n >= 1 && n <= 1e4);
			Contract.Requires<ArgumentOutOfRangeException>(length >= 1 && length <= 20);
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(characters));
			Contract.Requires<ArgumentOutOfRangeException>(characters.Length <= 80);
			//Contract.Requires(replacement || (n <= GMath.Pow(length, characters.Length)));

			var requestData = _buildRequest("generateStrings", new Dictionary<string, object>
			{
				{ "n", n },
				{ "length", length },
				{ "characters", characters },
				{ "replacement", replacement },
			});

			var responseString = await _makeRequestAsync(requestData);
			var response = serializer.Deserialize<_Response>(responseString);
			if (response.Error != null)
				throw new WebException("An unhandled error occurred when querying random.org: " + response.Error.Message + " (code: " + response.Error.Code.ToString() + ")");
			return response.Result.Random.Data.Cast<string>().ToArray();
		}

		[DataContract]
		private class _Random
		{
			[DataMember(Name = "data")]
			public object[] Data { get; set; }

			[DataMember(Name = "completionTime")]
			public string CompletionTime { get; set; }
		}

		[DataContract]
		private class _Result
		{
			[DataMember(Name = "random")]
			public _Random Random { get; set; }

			[DataMember(Name = "bitsUsed")]
			public int BitsUsed { get; set; }

			[DataMember(Name = "bitsLeft")]
			public int BitsLeft { get; set; }

			[DataMember(Name = "requestsLeft")]
			public int RequestsLeft { get; set; }

			[DataMember(Name = "advisoryDelay")]
			public int AdvisoryDelay { get; set; }
		}

		[DataContract]
		private class _Error
		{
			[DataMember(Name = "code")]
			public int Code { get; set; }

			[DataMember(Name = "message")]
			public string Message { get; set; }
		}

		[DataContract]
		private class _Response
		{
			[DataMember(Name = "jsonrpc")]
			public string JsonRpc { get; set; }

			[DataMember(Name = "result")]
			public _Result Result { get; set; }

			[DataMember(Name = "id")]
			public int Id { get; set; }

			[DataMember(Name = "error")]
			public _Error Error { get; set; }
		}



		private static async Task<string> _makeRequestAsync(string requestData)
		{
			var request = WebRequest.CreateHttp(baseUrl);
			request.ContentType = contentType;
			request.Method = "POST";

			using (var writer = new StreamWriter(request.GetRequestStream()))
				await writer.WriteAsync(requestData);

			var response = await request.GetResponseAsync();
			string responseString;
			using (var reader = new StreamReader(response.GetResponseStream()))
				responseString = await reader.ReadToEndAsync();

			return responseString;
		}

		private static string _buildRequest(string method, Dictionary<string, object> args)
		{
			Contract.Requires<ArgumentNullException>(args != null);
			StringBuilder str = new StringBuilder();
			str.Append("{\"jsonrpc\":\"2.0\",");
			str.AppendFormat("\"method\":\"{0}\",", method);
			str.Append("\"params\":{");

			str.AppendFormat("\"apiKey\":\"{0}\",", apiKey);

			foreach (var key in args.Keys)
			{
				var value = args[key];
				if (value is int || value is double || value is bool)
					str.AppendFormat(CultureInfo.InvariantCulture, "\"{0}\":{1},", key, Convert.ToString(value, CultureInfo.InvariantCulture).ToLower());
				else
					str.AppendFormat(CultureInfo.InvariantCulture, "\"{0}\":\"{1}\",", key, value);

			}
			str.Remove(str.Length - 1, 1);

			str.Append("},\"id\":0}");
			return str.ToString();
		}
	}
}
