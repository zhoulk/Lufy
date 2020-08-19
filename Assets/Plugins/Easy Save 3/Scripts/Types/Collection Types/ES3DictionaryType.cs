﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ES3Internal;

namespace ES3Types
{
	public class ES3DictionaryType : ES3Type
	{
		public ES3Type keyType;
		public ES3Type valueType;

		protected ES3Reflection.ES3ReflectedMethod readMethod = null;
		protected ES3Reflection.ES3ReflectedMethod readIntoMethod = null;

		public ES3DictionaryType(Type type) : base(type)
		{
			var types = ES3Reflection.GetElementTypes(type);
			keyType = ES3TypeMgr.GetOrCreateES3Type(types[0], false);
			valueType = ES3TypeMgr.GetOrCreateES3Type(types[1], false);

			// If either the key or value type is unsupported, make this type NULL.
			if(keyType == null || valueType == null)
				this.type = null;

			isDictionary = true;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			Write(obj, writer, writer.settings.memberReferenceMode);
		}

		public void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			var dict = (IDictionary)obj;

			writer.StartWriteDictionary(dict.Count);

			int i=0;
			foreach(System.Collections.DictionaryEntry kvp in dict)
			{
				writer.StartWriteDictionaryKey(i);
				writer.Write(kvp.Key, keyType, memberReferenceMode);
				writer.EndWriteDictionaryKey(i);
				writer.StartWriteDictionaryValue(i);
				writer.Write(kvp.Value, valueType, memberReferenceMode);
				writer.EndWriteDictionaryValue(i);
				i++;
			}

			writer.EndWriteDictionary();
		}

		public override object Read<T>(ES3Reader reader)
		{
			throw new NotImplementedException("Use ReadKVP<TKey,TVal>(reader) instead.");
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			throw new NotImplementedException("Use ReadKVP<TKey,TVal>(reader, obj) instead.");
		}

		public Dictionary<TKey,TVal> ReadKVP<TKey,TVal>(ES3Reader reader)
		{
			if(reader.StartReadDictionary())
				return null;

			var dict = new Dictionary<TKey,TVal>();

			// Iterate through each character until we reach the end of the array.
			while(true)
			{
				if(!reader.StartReadDictionaryKey())
					return dict;
				TKey key = reader.Read<TKey>(keyType);
				reader.EndReadDictionaryKey();

				reader.StartReadDictionaryValue();
				TVal value = reader.Read<TVal>(valueType);

				dict.Add(key,value);

				if(reader.EndReadDictionaryValue())
					break;
			}

			reader.EndReadDictionary();

			return dict;
		}

		public void ReadKVP<TKey,TVal>(ES3Reader reader, object obj)
		{
			if(reader.StartReadDictionary())
				throw new NullReferenceException("The Dictionary we are trying to load is stored as null, which is not allowed when using ReadInto methods.");

			var dict = (Dictionary<TKey,TVal>)obj;

			// Iterate through each character until we reach the end of the array.
			while(true)
			{
				if(!reader.StartReadDictionaryKey())
					return;
				var key = reader.Read<TKey>(keyType);
				TVal value;
				if(!dict.TryGetValue(key, out value))
					throw new KeyNotFoundException("The key \"" + key + "\" in the Dictionary we are loading does not exist in the Dictionary we are loading into");
				reader.EndReadDictionaryKey();

				reader.StartReadDictionaryValue();

				reader.ReadInto<TKey>(value, valueType);

				if(reader.EndReadDictionaryValue())
					break;
			}

			reader.EndReadDictionary();
		}

		/*
		 * 	Allows us to call the generic Read method using Reflection so we can define the generic parameter at runtime.
		 * 	It also caches the method to improve performance in later calls.
		 */
		public object Read(ES3Reader reader)
		{
			if(reader.StartReadDictionary())
				return null;

			var dict = (IDictionary)ES3Reflection.CreateInstance(type);

			// Iterate through each character until we reach the end of the array.
			while(true)
			{
				if(!reader.StartReadDictionaryKey())
					return dict;
				var key = reader.Read<object>(keyType);
				reader.EndReadDictionaryKey();

				reader.StartReadDictionaryValue();
				var value = reader.Read<object>(valueType);

				dict.Add(key,value);

				if(reader.EndReadDictionaryValue())
					break;
			}

			reader.EndReadDictionary();

			return dict;
		}

		public void ReadInto(ES3Reader reader, object obj)
		{
			if(reader.StartReadDictionary())
				throw new NullReferenceException("The Dictionary we are trying to load is stored as null, which is not allowed when using ReadInto methods.");

			var dict = (IDictionary)obj;

			// Iterate through each character until we reach the end of the array.
			while(true)
			{
				if(!reader.StartReadDictionaryKey())
					return;
				var key = reader.Read<object>(keyType);
				if(!dict.Contains(key))
					throw new KeyNotFoundException("The key \"" + key + "\" in the Dictionary we are loading does not exist in the Dictionary we are loading into");
				var value = dict[key];
				reader.EndReadDictionaryKey();

				reader.StartReadDictionaryValue();

				reader.ReadInto<object>(value, valueType);

				if(reader.EndReadDictionaryValue())
					break;
			}

			reader.EndReadDictionary();
		}
	}
}