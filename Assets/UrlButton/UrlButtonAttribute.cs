namespace UrlButton
{
	using System;
	using UnityEngine;

	[AttributeUsage(AttributeTargets.Field)]
	public class UrlButtonAttribute : PropertyAttribute
	{
		public readonly string url;

		/// <summary>
		/// Adds a Button that opens the specified URL.
		/// </summary>
		/// <param name="url">URL to open.</param>
		public UrlButtonAttribute(string url)
		{
			this.url = url;
		}
	}
}
