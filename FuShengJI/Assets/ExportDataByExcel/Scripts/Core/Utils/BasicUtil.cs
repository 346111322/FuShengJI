using UnityEngine;
using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Data;
using BH.Core.Configs;
using System.Xml;
using System.Text;

namespace BH.Core.Utils
{
	public class BasicUtil
	{
		public static void Destroy(GameObject obj, float time = 0)
		{
			if (null != obj)
			{
				obj.name += "[delete]";
				if (time > 0)
				{
					GameObject.Destroy(obj, time);
				}
				else
				{
					obj.SetActive(false);
					GameObject.Destroy(obj);
				}
			}
		}
		
//		public static bool IsNetworkAvailable
//		{
//			get
//			{
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE
//                try
//                {
//                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
//                    foreach (NetworkInterface nic in nics)
//                    {
//                        if ((nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
//                            && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel ) )
//                        {
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE
//                            if( nic.OperationalStatus == OperationalStatus.Up )
//                            {
//                                return true;
//                            }
//#elif UNITY_STANDALONE_OSX
//                            if( nic.OperationalStatus == OperationalStatus.Up || nic.OperationalStatus == OperationalStatus.Unknown )
//                            {
//                                return true;
//                            }
//#endif
//                        }
//                    }
//
//                    return false;
//                }
//                catch (Exception e)
//                {
//                    UnityEngine.Debug.LogException(e);
//                    return true;
//                }
//#elif UNITY_IPHONE || UNITY_ANDROID
//				return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
//						|| Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
//#endif
//				return false;	// default return;
//			}
//		}
		
		public static bool Is2GNetwork
		{
			get
			{
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE
//				return false;
//#elif UNITY_IPHONE || UNITY_ANDROID
				return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
//#endif
				return false;
			}
		}
		
		public static string GetTimeStamp()
		{
			DateTime now = DateTime.UtcNow;
			return    now.Year.ToString()
					+ now.Month.ToString()
					+ now.Day.ToString()
					+ now.Hour.ToString()
					+ now.Minute.ToString()
					+ now.Second.ToString();
		}
		
		public static DateTime TickerToDate(long ticker)
		{
			return new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(ticker);
		}

        public static DateTime MilliTickerToDate(long ticker)
        {
            return new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(ticker);
        }

		public static long DateToTicker(DateTime dt)
		{
			return (long)dt.Subtract(new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds;
		}
		public static List<string> ParseParam(string param)
		{
            if (param.Equals(""))
            {
                return null;
            }
			List<string> paramList = new List<string>();
			
			int lastSpIdx = 0;
			int spIdx = param.IndexOf(';');
			while(spIdx >= 0)
			{
				string p = param.Substring(lastSpIdx, spIdx-lastSpIdx);
				paramList.Add(p);
				lastSpIdx = spIdx+1;
				spIdx = param.IndexOf(';', lastSpIdx);
			}
			
			paramList.Add(param.Substring(lastSpIdx));
			
			return paramList;
		}
        /// <summary>
        /// 将秒转换成时间显示 00:00  天：小时
        /// </summary>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        public static string ConvertTimeToDayStr(long Seconds)
        {
            int d = (int)Seconds / 1000 / 60 / 60 / 24;
            double h =(double)Seconds / 1000 / 60 / 60 / 24;

            int hh = (int)((h - d)*24);

            string nowTime = "";
            string ho = "";
            if (hh < 10)
            {
                ho = "0" + hh;
            }
            else
            {
                ho = "" + hh;
            }
//            nowTime = string.Concat(d, Localization.Get("GJZ_T"), ho, Localization.Get("GJZ_XS"));
            return nowTime;
        }

        public static DateTime MilliSec2MicroSec_100(long milliSeconds)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1);
            return new DateTime(dt1970.Ticks + milliSeconds * 1000 * 10).ToLocalTime();

        }


        /// <summary>
        /// 将秒转换成时间显示 00:00:00
        /// </summary>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        public static string ConvertToTimeStr(int Seconds)
        {
            int h = Seconds / 3600;
            int m = (Seconds % 3600) / 60;
            int s = Seconds % 60;

            string nowTime = "";

            string sc = "";
            if (s < 10)
            {
                sc = "0" + s;
            }
            else
            {
                sc = "" + s;
            }
            string mc = "";
            if (m < 10)
            {
                mc = "0" + m;
            }
            else
            {
                mc = "" + m;
            }
            string ho = "";
            if (h < 10)
            {
                ho = "0" + h;
            }
            else
            {
                ho = "" + h;
            }
            nowTime = ho + ":" + mc + ":" + sc;
            return nowTime;
        }

        /// <summary>
        /// 秒显示成分钟：00:00
        /// </summary>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        public static string ConvertScondsToMinTimeStr(int Seconds)
        {
            if (Seconds <= 0)
                return "00:00";
            int m = (Seconds % 3600) / 60;
            int s = Seconds % 60;

            string nowTime = "";

            string sc = "";
            if (s < 10)
            {
                sc = "0" + s;
            }
            else
            {
                sc = "" + s;
            }
            string mc = "";
            if (m < 10)
            {
                mc = "0" + m;
            }
            else
            {
                mc = "" + m;
            }
            
            nowTime = mc + ":" + sc;
            return nowTime;
        }

        /// <summary>
        /// 秒显示成分钟：XX小时XX分钟
        /// </summary>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        public static string ConvertToMinTimeStr(int Seconds)
        {
            int h = Seconds / 3600;
            int m = (Seconds % 3600) / 60;
            string nowTime = "";
            if (h > 0)
            {
                nowTime = h + "小時" + m + "分鐘";
            }
            else if (m > 0)
            {
                nowTime = m + "分鐘";
            }
            else
            {
                nowTime = "0分鐘";
            }
            return nowTime;
        }

        /// <summary>
        /// 分钟显示成分钟：XX小时XX分钟
        /// </summary>
        /// <param name="Min"></param>
        /// <returns></returns>
        public static string ConvertToMinTimeMinStr(int Min)
        {
            int h = Min / 60;
            int m = (Min % 60);
            string nowTime ="";
            if (h > 0)
            {
                nowTime = h + "小時" + m + "分鐘";
            }
            else if (m > 0)
            {
                nowTime = m + "分鐘";
            }
            else
            {
                nowTime = "0分鐘";
            }
            return nowTime;
        }

        /// <summary>
        /// 秒显示成天：XX天XX小时XX分钟
        /// </summary>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static string ConvertSecondToDay(int Second)
        {
            int d = Second / 86400;
            int h = (Second % 86400) / 3600;
            int m = (Second % 3600) / 60;
            string nowTime = "";
            if (d > 0)
            {
                nowTime = d + "天" + h + "小時" + m + "分鐘";
            }
            else if (h > 0)
            {
                nowTime = h + "小時" + m + "分鐘";
            }
            else if (m > 0)
            {
                nowTime = m + "分鐘";
            }
            else
            {
                nowTime = "0分鐘";
            }
            return nowTime;
        }
        public enum TimeStrType
        {
            DayAndClock=0,//类似：“2天前”； “今天：12:30”
            DayAndHours,//类似:x天，x小时前
        }
        /// <summary>
        /// 根据传递过来的毫秒数计算出相差的时间。
        /// </summary>
        /// <param name="timeInMillisecond"></param>
        /// <returns></returns>
        public static string GetRequiredTimeString(long timeInMillisecond,TimeStrType strType=TimeStrType.DayAndClock)
        {
            DateTime dt0 = new DateTime(1970, 1, 1, 0, 0, 0);//
            DateTime dt1 = new DateTime();
            dt1 = DateTime.UtcNow;
            dt1=dt1.AddSeconds(8 * 60 * 60);
            DateTime dt2 = dt0.AddMilliseconds(timeInMillisecond + 8 * 60 * 60*1000);

            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);

            TimeSpan delta = ts1.Subtract(ts2).Duration();

            int days = delta.Days;
            int hours = delta.Hours;
            int mins = delta.Minutes;
            mins = Math.Max(mins, 1);
            string timeStr = "";
            if (strType == TimeStrType.DayAndClock)
            {
                if (days > 0)
                {
                    timeStr += days + "天前";
                }

                else
                {
                    timeStr += "今日" + ts2.Hours + ":" + ts2.Minutes;
                }
            }
            else if (strType == TimeStrType.DayAndHours)
            {
                if (days > 0)
                {
                    timeStr += days + "天";
                }

                else
                {
                    hours = Math.Max(1,hours);
                    timeStr += hours + "小時前";
                }
            }

            return timeStr;
        }
		public static void ResetTransform(Transform tran)
        {
			tran.localPosition = new Vector3(0.2f,0,0);
			tran.localScale = Vector3.one;
			tran.localRotation = Quaternion.identity;
		}

		public static void setParent(Transform parent,Transform child){
			child.parent = parent;
			ResetTransform (child);
		}

     /// <summary>
         /// 十六进制换算为十进制
          /// </summary>
          /// <param name="strColorValue"></param>
          /// <returns></returns>
          public static int GetHexadecimalValue(String strColorValue)
          {
              char[] nums = strColorValue.ToCharArray();
              int total = 0;
             try
             {
                 for (int i = 0; i < nums.Length; i++)
                 {
                     String strNum = nums[i].ToString().ToUpper();
                     switch (strNum)
                     {
                         case "A":
                             strNum = "10";
                             break;
                         case "B":
                             strNum = "11";
                             break;
                         case "C":
                             strNum = "12";
                             break;
                         case "D":
                             strNum = "13";
                             break;
                         case "E":
                             strNum = "14";
                             break;
                         case "F":
                             strNum = "15";
                             break;
                         default:
                             break;
                     }
                     double power = Math.Pow(16, Convert.ToDouble(nums.Length - i - 1));
                     total += Convert.ToInt32(strNum) * Convert.ToInt32(power);
                 }
 
             }
             catch (System.Exception ex)
             {
                 String strErorr = ex.ToString();
                 return 0;
             }
 
 
             return total;
         }
        /// Get the hero's rank
        static public int GetColorRank(int tableRank)
        {
            /// 白
            if (tableRank <= 1)
            {
                return 0;
            }
            /// 绿，绿+1
            else if (tableRank <= 3)
            {
                return 1;
            }
            /// 蓝，蓝+1，蓝+2
            else if (tableRank <= 6)
            {
                return 2;
            }
            /// 紫，紫+1，紫+2，紫+3
            else if (tableRank <= 10)
            {
                return 3;
            }
            /// 橙，橙+1，橙+2，橙+3，橙+4
            else if (tableRank <= 15)
            {
                return 4;
            }
            return 0;
        }

        /// <summary>
        /// 替换字符串{xxx}中内容的方法
        /// </summary>
        /// <param name="primaryString"></原始字符串>
        /// <param name="replaceStringList"></替换各个花括号中内容的数组>
        /// <param name="colorArray"></各个新内容的字体颜色>
        /// <returns></returns>
        static public string ReplacementString(string primaryString, List<string> replaceStringList, List<string> colorArray)
        {
            string colorMarkEnd = "[-]";
            string normalColorStart = "[541109]";
            int colorindex = 0;
            char[] primaryCharArray = primaryString.ToCharArray();
            List<char> resultCharList = new List<char>();
            int replaceChar = 0;
            bool normalChatColorStart = true;
            for (int idx = 0; idx < primaryCharArray.Length;idx++)
            {
                if (normalChatColorStart)
                {
                    resultCharList.AddRange(normalColorStart.ToCharArray());
                    normalChatColorStart = false;
                }
                if (primaryCharArray[idx] == '{')
                {
                    resultCharList.AddRange(colorMarkEnd.ToCharArray());
                    replaceChar = 1;
                    if(colorArray[colorindex] != null){
                        resultCharList.AddRange(colorArray[colorindex].ToCharArray());
                    }
                    continue;
                }
                if (replaceChar == 1)
                {
                    replaceChar = 2;
                    resultCharList.AddRange(replaceStringList[colorindex].ToCharArray());
                    continue;
                }
                if (replaceChar == 2)
                {
                    replaceChar = 0;
                    if (colorArray[colorindex] != null)
                    {
                        resultCharList.AddRange(colorMarkEnd.ToCharArray());
                        normalChatColorStart = true;
                    }
                    colorindex++;
                    continue;
                }
                resultCharList.Add(primaryCharArray[idx]);
            }
            Char[] resultCharArray = new Char[resultCharList.Count];
            for (int idx = 0; idx < resultCharList.Count;idx++)
            {
                resultCharArray[idx] = resultCharList[idx];
            }
            return new String(resultCharArray); ;
        }

        static public Color GetColor(string colorStr, char seperator)
        {
            Color res = Color.black;
            string[] sp = colorStr.Split(seperator);
            int r = int.Parse(sp[0]);
            int g = int.Parse(sp[1]);
            int b = int.Parse(sp[2]);
            float fr = (float)r / 255.0f;
            float fg = (float)g / 255.0f;
            float fb = (float)b / 255.0f;
            if (sp.Length == 4)
            {
                int a = int.Parse(sp[3]);
                float fa = (float)a / 255.0f;
                res = new Color(fr, fg, fb, fa);
            }
            else
            {
                res = new Color(fr, fg, fb);
            }
            return res;
        }

        private static int IntCryptCode = 10000;
        private static int FloatCryptCode = 1000000;
	    private static UInt16 LowerCryptCode = 0xFFFF; 

        // 加密数值
        public static void GenerateCryptPlusValue()
        {
            UnityEngine.Random.seed = (int)DateTime.Now.Ticks;
            IntCryptCode = UnityEngine.Random.Range(1000, 10000);
            FloatCryptCode = UnityEngine.Random.Range(100000, 1000000);
        }
        // 加密数值
        public static int EncryptInt32Value(int value)
        {
            return SwapCryptCode(~(value ^ IntCryptCode));
        }
        // 解密数值
        public static int DecryptInt32Value(Int32 value)
        {
            return ~SwapCryptCode(value) ^ IntCryptCode;
        }

        // 加密数值
        public static float EncryptSingleValue(float value)
        {
            // 转成Int32
            int tmp = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            // 对转换结果进行异或处理
            int intResult = tmp ^ FloatCryptCode;
            // 取反后转成float
            return BitConverter.ToSingle(BitConverter.GetBytes(SwapCryptCode(~intResult)), 0);
        }
        // 解密数值
        public static float DecryptSingleValue(Single value)
        {
            // 转成Int32再取反
            int tmp = ~SwapCryptCode(BitConverter.ToInt32(BitConverter.GetBytes(value), 0));
            // 异或
            int intResult = tmp ^ FloatCryptCode;
            // 转成float
            return BitConverter.ToSingle(BitConverter.GetBytes(intResult), 0);
        }


        private static int SwapCryptCode(int value)
	    {
	        UInt16 lower, higher;
            lower = (UInt16)(value & LowerCryptCode);
            higher = (UInt16)(value >> 16 & LowerCryptCode);
	        return lower << 16 | higher;
	    }
	}
}