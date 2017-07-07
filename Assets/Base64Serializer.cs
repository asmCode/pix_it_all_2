using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Base64Serializer
{
	public static string Encode(object obj)
	{
		if (obj == null)
			return null;

		var formatter = new BinaryFormatter();
		var memoryStream = new System.IO.MemoryStream();

		formatter.Serialize(memoryStream, obj);

		return System.Convert.ToBase64String(memoryStream.ToArray());
	}

	public static T Decode<T>(string base64String)
		where T : class
	{
		if (string.IsNullOrEmpty(base64String))
			return null;
		
		var raw = System.Convert.FromBase64String(base64String);
		if (raw == null)
			return null;

		var formatter = new BinaryFormatter();
		var memoryStream = new System.IO.MemoryStream(raw);

		return formatter.Deserialize(memoryStream) as T;
	}
}
