using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
namespace GameSave
{
	// Token: 0x0200122A RID: 4650
	[Serializable]
	public class Kv
	{
		// Token: 0x04003EED RID: 16109
		public string k;

		// Token: 0x04003EEE RID: 16110
		public object b;
	}
	// Token: 0x0200122B RID: 4651
	[Serializable]
	public class Di
	{
		// Token: 0x04003EEF RID: 16111
		public Kv[] li;
	}
	[Serializable]
	public enum SaveDataType
	{
		// Token: 0x04003EFB RID: 16123
		None,
		// Token: 0x04003EFC RID: 16124
		List,
		// Token: 0x04003EFD RID: 16125
		Dictionary,
		// Token: 0x04003EFE RID: 16126
		Int,
		// Token: 0x04003EFF RID: 16127
		String
	}
	// Token: 0x02001232 RID: 4658
	[Serializable]
	public class SaveData
	{
		public SaveData()
        {
			this._type = SaveDataType.None;
        }
		public SaveData(int value)
        {
			this._type = SaveDataType.Int;
			this._pdi = value;
		}
		public SaveData(string value)
		{
			this._type = SaveDataType.String;
			this._pds = value;
		}
		public SaveData(List<int> list)
        {
			this._type = SaveDataType.List;
			_list = new List<SaveData>(list.Count);
			foreach (int value in list)
			{
				this._list.Add(new SaveData(value));
			}
		}
		public SaveData(List<SaveData> list)
		{
			this._type = SaveDataType.List;
			_list = new List<SaveData>(list.Count);
			foreach (SaveData value in list)
			{
				this._list.Add(value);
			}
		}
		public int GetListCount()
		{
			return this._list.Count;
		}
		public int GetIntSelf()
		{
			return this._pdi;
		}
		public string GetStringSelf()
		{
			return this._pds;
		}
		public int GetInt(string key)
		{
			SaveData data = this.GetData(key);
			if (data == null)
			{
				Debug.Log("key not found : " + key);
				return 0;
			}
			return data.GetIntSelf();
		}

		public string GetString(string key)
		{
			SaveData data = this.GetData(key);
			if (data == null)
			{
				Debug.Log("key not found : " + key);
				return "";
			}
			return data.GetStringSelf();
		}
		public Dictionary<string, SaveData> GetDictionarySelf()
		{
			return this._dic;
		}
		public List<SaveData> GetListSelf()
        {
			if(_list != null)
			return this._list;
			else
			return null; 
        }
		public object GetSerializedData()
		{
			switch (_type)
			{
				case SaveDataType.List:
					List<object> list = new List<object>(this._list.Capacity);
					foreach (SaveData saveData in this._list)
					{
						list.Add(saveData.GetSerializedData());
					}
					return list.ToArray();
				case SaveDataType.Dictionary:

					Di di = new Di();
					List<Kv> list2 = new List<Kv>(this._dic.Count);
					foreach (KeyValuePair<string, SaveData> keyValuePair in this._dic)
					{
						list2.Add(new Kv
						{
							k = keyValuePair.Key,
							b = keyValuePair.Value.GetSerializedData()
						});
					}
					di.li = list2.ToArray();
					return di;
				case SaveDataType.Int:
					return _pdi;
				case SaveDataType.String:
					return _pds;
				default:
					return null;
			}
		}
		public void LoadFromSerializedData(object serialized)
		{
			if (serialized is Dictionary<string, object>)
			{
				this._type = SaveDataType.Dictionary;
				Dictionary<string, object> dictionary = serialized as Dictionary<string, object>;
				this._dic = new Dictionary<string, SaveData>(dictionary.Count);
				using (Dictionary<string, object>.Enumerator enumerator = dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, object> keyValuePair = enumerator.Current;
						SaveData saveData = new SaveData();
						saveData.LoadFromSerializedData(keyValuePair.Value);
						this._dic.Add(keyValuePair.Key, saveData);
					}
					return;
				}
			}
			if (serialized is List<object>)
			{
				this._type = SaveDataType.List;
				List<object> list = serialized as List<object>;
				this._list = new List<SaveData>(list.Count);
				using (List<object>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						object serialized2 = enumerator2.Current;
						SaveData saveData2 = new SaveData();
						saveData2.LoadFromSerializedData(serialized2);
						this._list.Add(saveData2);
					}
					return;
				}
			}
			if (serialized is Di)
			{
				this._type = SaveDataType.Dictionary;
				Di di = serialized as Di;
				this._dic = new Dictionary<string, SaveData>(di.li.Length);
				foreach (Kv kv in di.li)
				{
					SaveData saveData3 = new SaveData();
					saveData3.LoadFromSerializedData(kv.b);
					this._dic.Add(kv.k, saveData3);
				}
				return;
			}
			if (serialized is object[])
			{
				this._type = SaveDataType.List;
				object[] array = serialized as object[];
				this._list = new List<SaveData>(array.Length);
				foreach (object serialized3 in array)
				{
					SaveData saveData4 = new SaveData();
					saveData4.LoadFromSerializedData(serialized3);
					this._list.Add(saveData4);
				}
				return;
			}
			if (serialized is int)
			{
				this._type = SaveDataType.Int;
				this._pdi = (int)serialized;
				return;
			}
			if (serialized is string)
			{
				this._type = SaveDataType.String;
				this._pds = (string)serialized;
				return;
			}
			if (serialized == null)
			{
				this._type = SaveDataType.None;
				return;
			}
			Debug.LogError("invalid SaveData");
		}

		public void AddData(string key, SaveData data)
		{
			if(this._type != SaveDataType.None && this._type != SaveDataType.Dictionary)
            {
				Debug.LogError("SaveData cannot have multiple type");
            }
			this._type = SaveDataType.Dictionary;
			this._dic.Add(key, data);
		}
		public void AddToList(SaveData data)
        {
			if (this._type != SaveDataType.None && this._type != SaveDataType.List)
			{
				Debug.LogError("SaveData cannot have multiple type");
			}
			this._type = SaveDataType.List;
			_list.Add(data);
        }
		public SaveData GetData(string key)
        {
			SaveData data;
			if(this._dic.TryGetValue(key, out data))
            {
				return data;
            }
			return null;
        }
		

		List<SaveData> _list = new List<SaveData>();
		Dictionary<string, SaveData> _dic = new Dictionary<string, SaveData>();
		int _pdi;
		string _pds;
		SaveDataType _type;
	}
}
