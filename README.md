# UrlButton
Add external link buttons to your inspector's fields in Unity.

## Installation

### Unity Package
You can add the code directly to the project:

1. Clone the repo or download the latest release.
2. Add the UrlButton folder to your Unity project or import the .unitypackage

## How To Use
1. Add the UrlButton attribute to a field.

   ```csharp
   using UrlButton; // 1. Import the namespace
   using System;
   using UnityEngine;
   
   public class ButtonExample : MonoBehaviour
   {
       // 2. Add the Button attribute to any field.
		[Serializable, UrlButton("https://github.com/bergheem/UrlButton")] string field; // 3. Add the url you want to open as attribute parameter
		// 4. Make sure the field is displayable in the inspector (Serializable or public)
   }
   ```
   
2. You should now see a button on the right side of the field:

   ![Button in the inspector](/Images/example.png)
