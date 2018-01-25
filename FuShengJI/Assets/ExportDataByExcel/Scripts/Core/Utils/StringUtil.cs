using System.Collections.Generic;
using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

namespace BH.Core.Utils
{
	public class StringUtils
	{
		private const string INVALID_CHAR_SET = ",<.>/?;:'\"[{]}\\|`~!@#$%^&*()-=+ \r\n\t";
		public static string ToSBC(string text)
		{
			char[] c = text.ToCharArray();
			for (int i=0; i<c.Length; i++)
			{
				if (INVALID_CHAR_SET.IndexOf(c[i]) > -1)
				{
					if (32 == c[i])
					{
						c[i] = (char)12288;
					}
					else if (c[i] < 127)
					{
						c[i] = (char)(c[i] + 65248);
					}
				}
			}
			
			return new string(c);
		}
		
		public static bool ContainInvalidChar(string text)
		{
			char[] c = text.ToCharArray();
			for (int i=0; i<c.Length; i++)
			{
				if (INVALID_CHAR_SET.IndexOf(c[i]) > -1)
				{
					return true;
				}
			}
			
			return false;
		}
		
		public static int LengthOfUTF8(string str)
		{
			int length = 0;
			char[] characters = str.ToCharArray();
			foreach (char c in characters)
			{
				int cInt = (int)c;
				if (cInt < 256)
				{
					length++;
				}
				else
				{
					length += 2;
				}
			}
			return length;
		}
        public static int ToInt(string str) {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            try
            {
                return int.Parse(str);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError("Convert str to int  error ：" + str +"___"+ e.ToString());
                return 0;
            }
            return 0;
        }
		public static string ToMD5(string str)
		{
			if(string.IsNullOrEmpty(str))
			{
				return null;
			}
			
			try
			{
#if UNITY_WP8
				return CryptoNet.Md5.ComputeHash(Encoding.UTF8.GetBytes(str)).ToLower();
#else
				MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
				byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
				return System.BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
#endif
			}
			catch (System.Exception e)
			{
				UnityEngine.Debug.LogError(e.ToString());
				return null;
			}
		}

        public static long GenerateTimeStamp(DateTime dt)
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

	    public static string ToString(int date)
	    {
	        switch (date)
            {
                case 1:
                    return "一";
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
                case 7:
                    return "七";
                case 8:
                    return "八";
                case 9:
                    return "九";
                case 10:
                    return "十";
            }

	        return string.Empty;
	    }
		public static Dictionary<string, string> ReadDictionary(string str)
		{
			Dictionary<string, string> dic = new Dictionary<string, string>();
			string[] lines = str.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i=0; i<lines.Length; i++)
			{
				string line = lines[i].Trim();
				if (string.IsNullOrEmpty(line) || line.StartsWith("#") || line.StartsWith("//"))
				{
					continue;
				}
				
				string[] splites = line.Split(new char[] { '=' }, 2);
				if (2 == splites.Length)
				{
					try
					{
						dic.Add(splites[0].Trim(), splites[1].Trim());
					}
					catch (System.Exception ex)
					{
						UnityEngine.Debug.LogError("The key '" + splites[0].Trim() + "' " + ex.ToString());
					}
				}
				else
				{
					UnityEngine.Debug.LogError("Parse Error: " + line);
				}
			}
			
			return dic;
		}
		
		public static string ParseDictionary(Dictionary<string, string> dic)
		{
			string str = "";
			foreach (string key in dic.Keys)
			{
				str += key + "=" + dic[key] + "\n";
			}
			return str;
		}
		
		/**
		 * 检查是否有emoji字符
		 * @param source
		 * @return
		 */
		public static bool containEmoji(string source)
		{
			// test begin
			int len1 = source.Length;
			// test end
			int len = LengthOfUTF8(source);
			char[] szArr = source.ToCharArray();
			for (int i = 0; i < len; ++i)
			{
				char codePoint = szArr[i];
				if (!isNotEmojiCharacter(codePoint)) 
				{
					// do nothing，判断到了这里表明，确认有表情字符
					return true;
				}
			}
			return false;
		}
		
		private static bool isNotEmojiCharacter(char codePoint)
		{
			int value = (int)codePoint;
			string str = value.ToString();
			UnityEngine.Debug.Log(codePoint.ToString());
			UnityEngine.Debug.Log(str);
			// log somethint end
			return (codePoint == 0x0) ||
	
			(codePoint == 0x9) ||
	
			(codePoint == 0xA) ||
	
			(codePoint == 0xD) ||
	
			((codePoint >= 0x20) && (codePoint <= 0xD7FF)) ||
	
			((codePoint >= 0xE000) && (codePoint <= 0xFFFD)) ||
	
			((codePoint >= 0x10000) && (codePoint <= 0x10FFFF));
		}
		
		public static string filterEmoji(String source)
		{
			StringBuilder buf = new StringBuilder();
			int len = source.Length;
			char[] szArr = source.ToCharArray();
			for (int i = 0; i < len; i++) {
				char codePoint = szArr[i];
				if (isNotEmojiCharacter(codePoint)) {
					buf.Append(codePoint);
				}
			}
			return buf.ToString();
		}


        public static string HorizontalToVertical(string source)
	    {
	        StringBuilder buf = new StringBuilder();
            int len = source.Length;
            char[] szArr = source.ToCharArray();
            for (int i = 0; i < len; i++)
            {
                buf.AppendFormat("{0}\n", szArr[i]);
            }
            return buf.ToString();
	    }

        public static Color ParseColor(string colorString)
        {
            Color res = Color.white;
            string[] strAry = colorString.Split(',');
            if (strAry.Length != 4)
            {
                UnityEngine.Debug.LogError("ParseColor Error:" + colorString);
            }
            else
            {
                res = new Color(float.Parse(strAry[0]), float.Parse(strAry[1]), float.Parse(strAry[2]), float.Parse(strAry[3]));
            }
            return res;
        }
        public static Vector4 ParseVec4(string vecString)
        {
            Vector4 res = Vector4.zero;
            string[] strAry = vecString.Split(',');
            if (strAry.Length != 4)
            {
                UnityEngine.Debug.LogError("ParseVec4 Error:" + vecString);
            }
            else
            {
                res = new Vector4(float.Parse(strAry[0]), float.Parse(strAry[1]), float.Parse(strAry[2]), float.Parse(strAry[3]));
            }
            return res;
        }
        public static Vector3 ParseVec3(string vecString)
        {
            Vector3 res = Vector3.zero;
            string[] strAry = vecString.Split(',');
            if (strAry.Length != 3)
            {
                UnityEngine.Debug.LogError("ParseVec3 Error:" + vecString);
            }
            else
            {
                res = new Vector3(float.Parse(strAry[0]), float.Parse(strAry[1]), float.Parse(strAry[2]));
            }
            return res;
        }

        public static string[] ParseDateOrTime(string param, string splitor)
        {
            string[] ret = new string[3];
            int splitorIdx = param.IndexOf(splitor);
            ret[0] = param.Substring(0, splitorIdx);
            ret[1] = param.Substring(splitorIdx + 1);
            splitorIdx = ret[1].IndexOf(splitor);
            ret[2] = ret[1].Substring(splitorIdx+1);
            ret[1] = ret[1].Substring(0, splitorIdx);
            return ret;
        }
	}
}