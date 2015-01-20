using System;
using System.Collections.Generic;
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
			if (n < 1 || n > 1e4) throw new ArgumentOutOfRangeException("n", n, "must be in the range [1, 1e4]");
			if (min < -1e9 || 1e9 < min) throw new ArgumentOutOfRangeException("min", min, "must be in the range [-1e9, 1e9]");
			if (max < -1e9 || 1e9 < max) throw new ArgumentOutOfRangeException("max", max, "must be in the range [-1e9, 1e9]");
			if (min >= max) throw new ArgumentException("Parameter 'min' must be less than parameter 'max'");
			if (!replacement && (n > max - min)) throw new ArgumentException(String.Format("When requesting values with replacement, it is not possible to request more values than there are values in the domain (you requested {0} values in the range [{1}, {2}])", n, min, max));

			var requestData = _buildRequest("generateIntegers", new Dictionary<string, object>
			{
				{ "n", n },
				{ "min", min },
				{ "max", max },
				{ "replacement", replacement },
			});

			var responseString = await _makeRequestAsync(requestData);
			var response = serializer.Deserialize<_Response>(responseString);
			if (response.error != null)
				throw new WebException("An unhandled error occurred when querying random.org: " + response.error.message + " (code: " + response.error.code.ToString() + ")");
			return response.result.random.data.Cast<int>().ToArray();
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
			if (n < 1 || n > 1e4) throw new ArgumentOutOfRangeException("n", n, "must be in the range [1, 1e4]");
			if (decimalPlaces < 1 || decimalPlaces > 20) throw new ArgumentOutOfRangeException("decimalPlaces", decimalPlaces, "must be in the range [1, 20]");
			if (!replacement && (n > GMath.Pow(10, decimalPlaces))) throw new ArgumentException(String.Format("When requesting values with replacement, it is not possible to request more values than there are values in the domain (you requested {0} values, but the domain only contains {1} values)", n, GMath.Pow(10, decimalPlaces)));

#warning Doesn't work for decimalPlaces > 14
			var requestData = _buildRequest("generateDecimalFractions", new Dictionary<string, object>
			{
				{ "n", n },
				{ "decimalPlaces", decimalPlaces },
				{ "replacement", replacement }
			});

			var responseString = await _makeRequestAsync(requestData);
			var response = serializer.Deserialize<_Response>(responseString);
			if (response.error != null)
				throw new WebException("An unhandled error occurred when querying random.org: " + response.error.message + " (code: " + response.error.code.ToString() + ")");
			return response.result.random.data.Select(obj => Convert.ToDecimal(obj)).ToArray();
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
			if (n < 1 || n > 1e4) throw new ArgumentOutOfRangeException("n", n, "must be in the range [1, 1e4]");
			if (mean < -1e6 || mean > 1e6) throw new ArgumentOutOfRangeException("mean", mean, "must be in the range [-1e6, 1e6]");
			if (standardDeviation < -1e6 || standardDeviation > 1e6) throw new ArgumentOutOfRangeException("standardDeviation", standardDeviation, "must be in the range [-1e6, 1e6]");
			if (significantDigits < 2 || significantDigits > 20) throw new ArgumentOutOfRangeException("significantDigits", significantDigits, "must be in the range [2, 20]");

			var requestData = _buildRequest("generateGaussians", new Dictionary<string, object>
			{
				{ "n", n },
				{ "mean", mean },
				{ "standardDeviation", standardDeviation },
				{ "significantDigits", significantDigits }
			});

			var responseString = await _makeRequestAsync(requestData);
			var response = serializer.Deserialize<_Response>(responseString);
			if (response.error != null)
				throw new WebException("An unhandled error occurred when querying random.org: " + response.error.message + " (code: " + response.error.code.ToString() + ")");
			return response.result.random.data.Select(obj => Convert.ToDecimal(obj)).ToArray();
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
			if (n < 1 || n > 1e4) throw new ArgumentOutOfRangeException("n", n, "n must be in the range [1, 1e4]");
			if (length < 1 || n > 20) throw new ArgumentOutOfRangeException("length", length, "must be in the range [1, 20]");
			if (characters == null) throw new ArgumentNullException("characters");
			if (characters.Length == 0) throw new ArgumentException("characters", "must have at least one character");
			if (characters.Length > 80) throw new ArgumentException("Cannot specify more than 80 characters", "characters"); /*C#6.0*/
			if (!replacement && (n > GMath.Pow(length, characters.Length)))
				throw new ArgumentException(string.Format("When requesting strings with replacement, it is not possible to request more strings than there are possible strings in the domain (you requested {0} values, but the domain only contains {1} values)", n, GMath.Pow(length, characters.Length)));

			var requestData = _buildRequest("generateStrings", new Dictionary<string, object>
			{
				{ "n", n },
				{ "length", length },
				{ "characters", characters },
				{ "replacement", replacement },
			});

			var responseString = await _makeRequestAsync(requestData);
			var response = serializer.Deserialize<_Response>(responseString);
			if (response.error != null)
				throw new WebException("An unhandled error occurred when querying random.org: " + response.error.message + " (code: " + response.error.code.ToString() + ")");
			return response.result.random.data.Cast<string>().ToArray();
		}

		[DataContract]
		private class _Random
		{
			[DataMember]
			public object[] data;

			[DataMember]
			public string completionTime;
		}

		[DataContract]
		private class _Result
		{
			[DataMember]
			public _Random random;

			[DataMember]
			public int bitsUsed;

			[DataMember]
			public int bitsLeft;

			[DataMember]
			public int requestsLeft;

			[DataMember]
			public int advisoryDelay;
		}

		[DataContract]
		private class _Error
		{
			[DataMember]
			public int code;

			[DataMember]
			public string message;
		}

		[DataContract]
		private class _Response
		{
			[DataMember]
			public string jsonrpc;

			[DataMember]
			public _Result result;

			[DataMember]
			public int id;

			[DataMember]
			public _Error error;
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
