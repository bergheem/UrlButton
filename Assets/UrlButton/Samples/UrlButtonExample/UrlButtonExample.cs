namespace UrlButton.Samples
{
	using UnityEngine;

	public class UrlButtonExample : MonoBehaviour
	{
		[SerializeField, UrlButton("https://github.com/bergheem/UrlButton")] string exampleField;
	}
}
