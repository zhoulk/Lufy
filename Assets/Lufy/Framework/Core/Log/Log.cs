
namespace LF
{
    /// <summary>
    /// 日志工具
    /// </summary>
    public static class Log
    {
        public static void Debug(object message)
        {
            LufyLog(LufyLogLevel.Debug, message);
        }

        public static void Debug(string format, object arg0)
        {
            LufyLog(LufyLogLevel.Debug, Utility.Text.Format(format, arg0));
        }

        public static void Debug(string format, params object[] arg0)
        {
            LufyLog(LufyLogLevel.Debug, Utility.Text.Format(format, arg0));
        }

        public static void Info(object message)
        {
            LufyLog(LufyLogLevel.Info, message);
        }

        public static void Info(string format, object arg0)
        {
            LufyLog(LufyLogLevel.Info, Utility.Text.Format(format, arg0));
        }

        public static void Warn(object message)
        {
            LufyLog(LufyLogLevel.Warning, message);
        }

        public static void Warn(string format, object arg0)
        {
            LufyLog(LufyLogLevel.Warning, Utility.Text.Format(format, arg0));
        }

        public static void Error(object message)
        {
            LufyLog(LufyLogLevel.Error, message);
        }

        public static void Error(string format, object arg0)
        {
            LufyLog(LufyLogLevel.Error, Utility.Text.Format(format, arg0));
        }

        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="level">日志等级。</param>
        /// <param name="message">日志内容。</param>
        private static void LufyLog(LufyLogLevel level, object message)
        {
            switch (level)
            {
                case LufyLogLevel.Debug:
                    UnityEngine.Debug.Log(Utility.Text.Format("<color=#000000>{0}</color>", message.ToString()));
                    break;

                case LufyLogLevel.Info:
                    UnityEngine.Debug.Log(Utility.Text.Format("<color=#0000ff>{0}</color>", message.ToString()));
                    break;

                case LufyLogLevel.Warning:
                    UnityEngine.Debug.Log(Utility.Text.Format("<color=#ff6600>{0}</color>", message.ToString()));
                    break;

                case LufyLogLevel.Error:
                    UnityEngine.Debug.Log(Utility.Text.Format("<color=#ff0000>{0}</color>", message.ToString()));
                    break;

                default:
                    throw new LufyException(message.ToString());
            }
        }
    }
}

