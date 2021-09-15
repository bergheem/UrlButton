namespace UrlButton
{
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(UrlButtonAttribute))]
	public class UrlButtonAttributeDrawer : PropertyDrawer
	{
		public GUIStyle BlankStyle
		{
			get
			{
				GUIStyle s = EditorStyles.miniButton;
				s.alignment = TextAnchor.MiddleCenter;
				s.clipping = TextClipping.Overflow;
				s.padding = new RectOffset(0, 0, 0, 0);
				//s.imagePosition = ImagePosition.ImageOnly;
				if(t == null)
				{
					t = new Texture2D(1, 1);
					t.SetPixel(0, 0, Color.clear);
					t.Apply();
				}
				s.normal.background = t;
				s.margin = new RectOffset(0, 0, 0, 0);
				return s;
			}
		}

		UrlButtonAttribute Attribute { get { return (UrlButtonAttribute)attribute; } }

		#region ATTRIBUTES_COMPATIBILITY

		/// <summary>
		/// A helper property to check for RangeAttribute.
		/// </summary>
		public RangeAttribute RangeAttribute { get { return GetAttribute<RangeAttribute>(); } }

		/// <summary>
		/// A helper property to check for MultiLineAttribute.
		/// </summary>
		public MultilineAttribute MultilineAttribute { get { return GetAttribute<MultilineAttribute>(); } }

		/// <summary>
		/// Custom added height for drawing text area which has the MultilineAttribute.
		/// </summary>
		public float AddedHeight { get; private set; } = 0;

		T GetAttribute<T>() where T : class
		{
			object[] attributes = fieldInfo.GetCustomAttributes(typeof(T), true);
			return attributes != null && attributes.Length > 0 ? (T)attributes[0] : null;
		}

		#endregion


		Texture2D t;


		public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
		{
			float width = EditorGUIUtility.singleLineHeight;
			pos.width -= width;

			DrawDefaultProperty(this, pos, prop, label);

			pos.x += pos.width;
			pos.width = width;
			Rect box1 = pos;
			Rect box2 = pos;
			box2.x += 1;
			box2.y += 2;
			if(GUI.Button(box1, new GUIContent("\u2610"), BlankStyle)
				|| GUI.Button(box2, new GUIContent("\u2197", "Open link"), BlankStyle))
				Application.OpenURL(Attribute.url);
		}

		void DrawDefaultProperty(UrlButtonAttributeDrawer drawer, Rect pos, SerializedProperty prop, GUIContent label)
		{
			// We get a local reference to the MultilineAttribute as we use it twice in this method and it
			// saves calling the logic twice for minimal optimization, etc...
			MultilineAttribute multiline = drawer.MultilineAttribute;
			// If we have a RangeAttribute on our field, we need to handle the PropertyDrawer differently to
			// keep the same style as Unity's default.
			RangeAttribute range = drawer.RangeAttribute;

			// If we have a RangeAttribute on our field, we need to handle the PropertyDrawer differently to
			// keep the same style as Unity's default.
			if(drawer.RangeAttribute != null)
			{
				if(prop.propertyType == SerializedPropertyType.Float)
				{
					EditorGUI.Slider(pos, prop, drawer.RangeAttribute.min, drawer.RangeAttribute.max, label);
				}
				else if(prop.propertyType == SerializedPropertyType.Integer)
				{
					EditorGUI.IntSlider(pos, prop, (int)drawer.RangeAttribute.min, (int)drawer.RangeAttribute.max, label);
				}
				else
				{
					// Not numeric so draw standard property field as punishment for adding RangeAttribute to
					// a property which can not have a range :P
					EditorGUI.PropertyField(pos, prop, label);
				}
			}
			else if(multiline != null)
			{
				// Here's where we handle the PropertyDrawer differently if we have a MultiLineAttribute, to try
				// and keep some kind of multiline text area. This is not identical to Unity's default but is
				// better than nothing...
				if(prop.propertyType == SerializedPropertyType.String)
				{
					var style = GUI.skin.label;
					var size = style.CalcHeight(label, EditorGUIUtility.currentViewWidth);

					EditorGUI.LabelField(pos, label);

					pos.y += size;
					pos.height += drawer.AddedHeight - size;

					// Fixed text dissappearing thanks to: http://answers.unity3d.com/questions/244043/textarea-does-not-work-text-dissapears-solution-is.html
					prop.stringValue = EditorGUI.TextArea(pos, prop.stringValue);
				}
				else
				{
					// Again with a MultilineAttribute on a non-text field deserves for the standard property field
					// to be drawn as punishment :P
					EditorGUI.PropertyField(pos, prop, label);
				}
			}
			else
			{
				// If we get to here it means we're drawing the default property field below the HelpBox. More custom
				// and built in PropertyDrawers could be implemented to enable HelpBox but it could easily make for
				// hefty else/if block which would need refactoring!
				EditorGUI.PropertyField(pos, prop, label, prop.isExpanded);
			}
		}
	}
}
