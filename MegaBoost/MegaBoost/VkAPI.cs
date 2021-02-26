using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MegaBoost
{
	public class VKAPI
	{
		/* VK Client*/
		public VKAPI(string token, string botCommunityId, Action<string, string, string, string, string, string> onMessageReceivedCallback, IWebProxy webProxy = null)
		{
			Token = token;
			BotCommunityId = botCommunityId;
			OnMessageReceivedCallback = onMessageReceivedCallback;
			ReceiverWebClient = new WebClient() { Proxy = webProxy, Encoding = Encoding.UTF8 };
			SenderWebClient = new WebClient() { Proxy = webProxy, Encoding = Encoding.UTF8 };

			Init();
			StartLongPoolAsync();
		}
		private WebClient ReceiverWebClient { get; set; }
		private WebClient SenderWebClient { get; set; }
		private string Token { get; set; }
		private int LastTs { get; set; }
		private string Server { get; set; }
		private string Key { get; set; }
		private Action<string, string, string, string, string, string> OnMessageReceivedCallback { get; set; }
		private string BotCommunityId { get; set; }
		private Random rnd = new Random();

		/* Utils */
		public string Utils_GetShortLink(string url)
		{
			if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
			{
				string json = CallVkMethod("utils.getShortLink", "url=" + url);
				var j = JsonConvert.DeserializeObject(json) as JObject;
				var link = j["response"]["short_url"].ToString();
				if (link == "")
					return Utils_GetShortLink(url);
				else
					return link;
			}
			else
				return "Invalid link format";
		}
		/* Docs */
		public string Docs_GetMessagesUploadServer(string peer_id, string type, string file)
		{
			string string1 = CallVkMethod("docs.getMessagesUploadServer", "peer_id=" + peer_id + "&type=" + type);
			if (string1 != "")
			{
				string uploadurl = Regex.Match(string1, "\"upload_url\":\"(.*)\"").Groups[1].Value.Replace(@"\/", "/");
				return uploadurl;
			}
			else
				return Docs_GetMessagesUploadServer(peer_id, type, file);
		}
		public string Docs_Upload(string url, string file)
		{
			var c = new WebClient();
			var r2 = Encoding.UTF8.GetString(c.UploadFile(url, "POST", file));
			if (r2 != "") return r2;
			else return Docs_Upload(url, file);
		}
		public string Docs_Save(string file, string title)
		{
			var j2 = JsonConvert.DeserializeObject(file) as JObject;
			if (j2 != null)
			{
				string json = CallVkMethod("docs.save", "&file=" + j2["file"].ToString() + "&title=" + title);
				if (json != "")
					return json;
				else return "";
			}
			else return "";
		}
		public string Docs_Get_Send_Attachment(string file)
		{
			var j3 = JsonConvert.DeserializeObject(file) as JObject;
			var at = "doc" + j3["response"]["doc"]["owner_id"].ToString() + "_" + j3["response"]["doc"]["id"].ToString();
			return at;
		}
		/* Groups */
		public void Groups_Online(bool enable = true)
		{
			if (enable)
				CallVkMethod("groups.enableOnline", "group_id=" + BotCommunityId);
			else
				CallVkMethod("groups.disableOnline", "group_id=" + BotCommunityId);
		}
		public string Groups_GetById_GetName(string group_id)
		{
			try
			{
				string js = CallVkMethod("groups.getById", "group_id=" + group_id);
				if (js != "")
				{
					var j3 = JsonConvert.DeserializeObject(js) as JObject;
					string name = j3["response"][0]["name"].ToString();
					return name;
				}
				else return "";
			}
			catch { return ""; }
		}
		/* Users */
		public string Users_Get_FirstName(string user_id)
		{
			string js = CallVkMethod("users.get", "user_ids=" + user_id);
			if (js != "")
			{
				var j3 = JsonConvert.DeserializeObject(js) as JObject;
				string name = j3["response"][0]["first_name"].ToString();
				return name;
			}
			else return "";
		}
		/* Messages */
		public void Messages_Kick_Group(string chat_id, string user_id)
		{
			if (user_id != BotCommunityId)
			{
				string json = CallVkMethod("messages.removeChatUser", "chat_id=" + chat_id + "&member_id=" + "-" + user_id);
			}
		}
		public void Messages_Kick_User(string chat_id, string user_id)
		{
			string json = CallVkMethod("messages.removeChatUser", "chat_id=" + chat_id + "&user_id=" + user_id + "&member_id=" + user_id);
		}
		public void Messages_SetActivity(string chatId, string type = "typing")
		{
			string id3 = chatId;
			id3 = id3.Substring(1);
			int ind = Convert.ToInt32(id3);
			CallVkMethod("messages.setActivity", "user_id=" + BotCommunityId + "&peer_id=" + chatId + "&group_id=" + "&type=" + type + "&group_id=" + BotCommunityId);
			CallVkMethod("messages.setActivity", "user_id=" + BotCommunityId + "&peer_id=" + ind + "&type=" + type + "&group_id=" + BotCommunityId);
		}
		public string Messages_GetInviteLink(string chatId, bool reset)
		{
			try
			{
				string json = CallVkMethod("messages.getInviteLink", "peer_id=" + chatId + "&group_id=" + BotCommunityId);
				if (json != "")
				{
					var j = JsonConvert.DeserializeObject(json) as JObject;
					var link = j["response"]["link"].ToString();
					if (link == "")
						return "";
					else return link;
				}
				else return "";
			}
			catch { return ""; }
		}
		/* Messages Send */
		public void Messages_Send_Text(string chatId, string text, int disable_mentions = 0)
		{
			string reply = CallVkMethod("messages.send", "peer_id=" + chatId + "&random_id=" + rnd.Next() + "&message=" + text + "&disable_mentions=" + disable_mentions);
		}
		public void Messages_Send_Keyboard(string chatId, Keyboard keyboard)
		{
			string kb = keyboard.GetKeyboard();
			string reply = CallVkMethod("messages.send", "peer_id=" + chatId + "&random_id=" + rnd.Next() + "&keyboard=" + kb);
		}
		public void Messages_Send_TextAndKeyboard(string chatId, string text, Keyboard keyboard)
		{
			string kb = keyboard.GetKeyboard();
			string reply = CallVkMethod("messages.send", "peer_id=" + chatId + "&random_id=" + rnd.Next() + "&message=" + text + "&keyboard=" + kb);
		}
		public void Messages_Send_Sticker(string chatId, int sticker_id)
		{
			string reply = CallVkMethod("messages.send", "peer_id=" + chatId + "&random_id=" + rnd.Next() + "&sticker_id=" + sticker_id);
		}
		public void Messages_Send_TextAndDocument(string chatId, string text, string file, string title)
		{
			string u2 = Docs_GetMessagesUploadServer(chatId, "doc", file);
			string r2 = Docs_Upload(u2, file);
			string r3 = Docs_Save(r2, title);
			string at = Docs_Get_Send_Attachment(r3);
			string reply = CallVkMethod("messages.send", "peer_id=" + chatId + "&random_id=" + rnd.Next() + "&message=" + text + "&attachment=" + at);
		}
		public void Messages_Send_Custom(string chatId, string custom)
		{
			string reply = CallVkMethod("messages.send", "peer_id=" + chatId + "&random_id=" + rnd.Next() + custom);
		}
		/* Messages GetConversationMembers*/
		public int Messages_GetConversationMembers_GetCount(string chatId)
		{
			var json = CallVkMethod("messages.getConversationMembers", "peer_id=" + chatId + "&group_id=" + BotCommunityId);
			if (json != "")
			{
				var j = JsonConvert.DeserializeObject(json) as JObject;
				int u2 = int.Parse(j["response"]["count"].ToString());
				return u2;
			}
			else return -1;
		}
		public string Messages_GetConversationMembers_GetProfiles(string chatId)
		{
			var json = CallVkMethod("messages.getConversationMembers", "peer_id=" + chatId + "&group_id=" + BotCommunityId);
			if (json != "")
			{
				string ids = "";
				JObject json1 = JObject.Parse(json);
				IList<JToken> results = json1["response"]["profiles"].Children().ToList();
				foreach (JToken result in results)
				{
					string id = result["id"].ToString();
					if (!id.Contains("-"))
						ids += id + ", ";
				}
				return ids;
			}
			else return "";
		}
		public string Messages_GetConversationMembers_GetItems_member_id(string chatId)
		{
			var json = CallVkMethod("messages.getConversationMembers", "peer_id=" + chatId + "&group_id=" + BotCommunityId);
			if (json != "")
			{
				string ids = "";
				JObject json1 = JObject.Parse(json);
				IList<JToken> results = json1["response"]["items"].Children().ToList();
				foreach (JToken result in results)
				{
					string id = result["member_id"].ToString();
					if (!id.Contains("-"))
						ids += id + ", ";
				}
				return ids;
			}
			return "";	
		}

		public class Keyboard
		{
			public enum Color
			{
				Negative,
				Positive,
				Primary,
				Secondary
			}

			public bool one_time = false;
			public List<List<object>> buttons = new List<List<object>>();
			public bool inline = false;
			public Keyboard(bool one_time2, bool line = false)
			{
				if (line == true && one_time2 == true)
					one_time2 = false;

				one_time = one_time2;
				inline = line;
			}

			public void AddButton(string label, string payload, Color color)
			{
				string color2 = "";
				if (color == Color.Negative)
					color2 = "Negative";
				else if (color == Color.Positive)
					color2 = "Positive";
				else if (color == Color.Primary)
					color2 = "Primary";
				else
					color2 = "Secondary";
				Buttons button = new Buttons(label, payload, color2);
				buttons.Add(new List<object>() { button });
			}
			public string GetKeyboard()
			{
				return JsonConvert.SerializeObject(this, Formatting.Indented); ;
			}
			public class Buttons
			{
				public Action action;
				public string color;
				public Buttons(string labe11, string payload1, string color2)
				{
					action = new Action(labe11, payload1);
					color = color2;
				}

				public class Action
				{
					public string type;
					public string payload;
					public string label;
					public Action(string label3, string payload3)
					{
						type = "text";
						payload = "{\"button\": \"" + payload3 + "\"}";
						label = label3;
					}
				}
			}
		}

		/* LoongPool */
		private void Init()
		{
			var jsonResult = CallVkMethod("groups.getLongPollServer", "group_id=" + BotCommunityId);
			var data = Json.ParseJson(jsonResult);

			Key = data.Properties["response"].Properties["key"].StringValue;
			Server = data.Properties["response"].Properties["server"].StringValue;
			LastTs = Convert.ToInt32(data.Properties["response"].Properties["ts"].StringValue);
		}
		private void StartLongPoolAsync()
		{
			Task.Factory.StartNew(() =>
			{
				while (true)
				{
					try
					{
						string baseUrl = String.Format("{0}?act=a_check&version=2&wait=25&key={1}&ts=", Server, Key);
						var data = ReceiverWebClient.DownloadString(baseUrl + LastTs);
						var messages = ProcessResponse(data);

						foreach (var message in messages)
						{
							OnMessageReceivedCallback(message.Item1, message.Item2, message.Item3, message.Item4, message.Item5, message.Item6);
						}
					}
					catch { }
				}
			});
		}

		private IEnumerable<Tuple<string, string, string, string, string, string>> ProcessResponse(string jsonData)
		{
			var j = JsonConvert.DeserializeObject(jsonData) as JObject;
			var data = Json.ParseJson(jsonData);
			if (data.Properties.ContainsKey("failed"))
			{
				Init();
			}
			LastTs = Convert.ToInt32(data.Properties["ts"].StringValue);
			var updates = data.Properties["updates"].DataArray;
			List<Tuple<string, string, string, string, string, string>> messages = new List<Tuple<string, string, string, string, string, string>>();
			foreach (var str in updates)
			{
				if (str.Properties["type"].StringValue != "message_new") continue;

				var msgData = str.Properties["object"].Properties;

				var id = msgData["from_id"].StringValue;
				var userId = msgData["from_id"].StringValue;
				var peer_id = msgData["peer_id"].StringValue;
				string event_id = "";
				var msgText = msgData["text"].StringValue;
				var conversation_message_id = msgData["conversation_message_id"].StringValue;

				messages.Add(new Tuple<string, string, string, string, string, string>(userId, peer_id, msgText, conversation_message_id, id, event_id));
			}

			return messages;
		}

		public string CallVkMethod(string methodName, string data)
		{
			try
			{
				var url = String.Format("https://api.vk.com/method/{0}?v=5.122&access_token={1}&{2}", methodName, Token, data);
				var jsonResult = SenderWebClient.DownloadString(url);

				return jsonResult;
			}
			catch { return String.Empty; }
		}
	}
	public static class Json
	{
		/// <summary>
		/// Parse some JSON and return the corresponding JSON object
		/// </summary>
		public static JSONData ParseJson(string json)
		{
			int cursorpos = 0;
			return String2Data(json, ref cursorpos);
		}

		/// <summary>
		/// The class storing unserialized JSON data
		/// The data can be an object, an array or a string
		/// </summary>
		public class JSONData
		{
			public enum DataType { Object, Array, String };
			private DataType type;
			public DataType Type { get { return type; } }
			public Dictionary<string, JSONData> Properties;
			public List<JSONData> DataArray;
			public string StringValue;
			public JSONData(DataType datatype)
			{
				type = datatype;
				Properties = new Dictionary<string, JSONData>();
				DataArray = new List<JSONData>();
				StringValue = String.Empty;
			}
		}

		/// <summary>
		/// Parse a JSON string to build a JSON object
		/// </summary>
		/// <param name="toparse">String to parse</param>
		/// <param name="cursorpos">Cursor start (set to 0 for function init)</param>
		private static JSONData String2Data(string toparse, ref int cursorpos)
		{
			try
			{
				JSONData data;
				switch (toparse[cursorpos])
				{
					//Object
					case '{':
						data = new JSONData(JSONData.DataType.Object);
						cursorpos++;
						while (toparse[cursorpos] != '}')
						{
							if (toparse[cursorpos] == '"')
							{
								JSONData propertyname = String2Data(toparse, ref cursorpos);
								if (toparse[cursorpos] == ':') { cursorpos++; } else { /* parse error ? */ }
								JSONData propertyData = String2Data(toparse, ref cursorpos);
								data.Properties[propertyname.StringValue] = propertyData;
							}
							else cursorpos++;
						}
						cursorpos++;
						break;

					//Array
					case '[':
						data = new JSONData(JSONData.DataType.Array);
						cursorpos++;
						while (toparse[cursorpos] != ']')
						{
							if (toparse[cursorpos] == ',') { cursorpos++; }
							JSONData arrayItem = String2Data(toparse, ref cursorpos);
							data.DataArray.Add(arrayItem);
						}
						cursorpos++;
						break;

					//String
					case '"':
						data = new JSONData(JSONData.DataType.String);
						cursorpos++;
						while (toparse[cursorpos] != '"')
						{
							if (toparse[cursorpos] == '\\')
							{
								try //Unicode character \u0123
								{
									if (toparse[cursorpos + 1] == 'u'
										&& isHex(toparse[cursorpos + 2])
										&& isHex(toparse[cursorpos + 3])
										&& isHex(toparse[cursorpos + 4])
										&& isHex(toparse[cursorpos + 5]))
									{
										//"abc\u0123abc" => "0123" => 0123 => Unicode char n°0123 => Add char to string
										data.StringValue += char.ConvertFromUtf32(int.Parse(toparse.Substring(cursorpos + 2, 4), System.Globalization.NumberStyles.HexNumber));
										cursorpos += 6; continue;
									}
									else if (toparse[cursorpos + 1] == 'n')
									{
										data.StringValue += '\n';
										cursorpos += 2;
										continue;
									}
									else if (toparse[cursorpos + 1] == 'r')
									{
										data.StringValue += '\r';
										cursorpos += 2;
										continue;
									}
									else if (toparse[cursorpos + 1] == 't')
									{
										data.StringValue += '\t';
										cursorpos += 2;
										continue;
									}
									else cursorpos++; //Normal character escapement \"
								}
								catch (IndexOutOfRangeException) { cursorpos++; } // \u01<end of string>
								catch (ArgumentOutOfRangeException) { cursorpos++; } // Unicode index 0123 was invalid
							}
							data.StringValue += toparse[cursorpos];
							cursorpos++;
						}
						cursorpos++;
						break;

					//Number
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
					case '.':
						data = new JSONData(JSONData.DataType.String);
						StringBuilder sb = new StringBuilder();
						while ((toparse[cursorpos] >= '0' && toparse[cursorpos] <= '9') || toparse[cursorpos] == '.')
						{
							sb.Append(toparse[cursorpos]);
							cursorpos++;
						}
						data.StringValue = sb.ToString();
						break;

					//Boolean : true
					case 't':
						data = new JSONData(JSONData.DataType.String);
						cursorpos++;
						if (toparse[cursorpos] == 'r') { cursorpos++; }
						if (toparse[cursorpos] == 'u') { cursorpos++; }
						if (toparse[cursorpos] == 'e') { cursorpos++; data.StringValue = "true"; }
						break;

					//Boolean : false
					case 'f':
						data = new JSONData(JSONData.DataType.String);
						cursorpos++;
						if (toparse[cursorpos] == 'a') { cursorpos++; }
						if (toparse[cursorpos] == 'l') { cursorpos++; }
						if (toparse[cursorpos] == 's') { cursorpos++; }
						if (toparse[cursorpos] == 'e') { cursorpos++; data.StringValue = "false"; }
						break;

					//Unknown data
					default:
						cursorpos++;
						return String2Data(toparse, ref cursorpos);
				}
				while (cursorpos < toparse.Length
					&& (char.IsWhiteSpace(toparse[cursorpos])
					|| toparse[cursorpos] == '\r'
					|| toparse[cursorpos] == '\n'))
					cursorpos++;
				return data;
			}
			catch (IndexOutOfRangeException)
			{
				return new JSONData(JSONData.DataType.String);
			}
		}

		/// <summary>
		/// Small function for checking if a char is an hexadecimal char (0-9 A-F a-f)
		/// </summary>
		/// <param name="c">Char to test</param>
		/// <returns>True if hexadecimal</returns>
		private static bool isHex(char c) { return ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')); }
	}
}
