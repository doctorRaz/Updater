/*
//think обработка исключений везде
//добавить логгер класс логера, чторбы проще цепляь к модулям





*/
using NLog;
using NLog.Layouts;
using NLog.Targets;

using System;


namespace drz.Updater
{
    /// <summary>
    ///Распаковка пакета обновления
    ///чтение в XML и распаковка
    /// </summary>
    public partial class UnWrapper : IDisposable
    //! Logger
    {
        #region Logger
        //think разобраться с форматированием лога, размером файла, по дате или размером файла и вообще с настройками и инициализацией логгера
        //https://www.codeproject.com/Articles/10631/Introduction-to-NLog
        //https://github.com/nlog/nlog/wiki/Tutorial
        //https://stackoverflow.com/questions/20198211/how-to-create-a-text-file-in-my-current-directory-with-nlog
        static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Файл лога
        /// </summary>
        /// <value>
        /// The s logname.
        /// </value>
        static string sLogName => logger.Name + "_" + DateTime.Now.ToString("yyyy.MM.dd") + ".log";

        /// <summary>
        /// путь имя файла лога
        /// </summary>
        /// <value>
        /// The s log name full.
        /// </value>

#if DEBUG
          static string sLogNameFull => sLogName;
        //private static string sLogNameFull =>  Path .Combine(sDirTemp,sLogName);
#else

        static string sLogNameFull => Path.Combine(sDirTemp, sLogName);
#endif

        /// <summary>
        /// Gets the name of the log file.
        /// <br>
        /// https://stackoverflow.com/questions/6635419/how-do-i-get-the-name-of-the-file-nlog-is-writing-to
        /// </br>
        ///<br>
        ///https://geoffhudik.com/tech/2012/01/25/quick-nlog-tip-getting-the-log-filename-html/
        ///</br>
        /// </summary>
        /// <param name="targetName">Name of the target.</param>
        /// <returns></returns>
        public string GetLogFileName(string targetName)
        {
            string rtnVal = string.Empty;

            if (LogManager.Configuration != null && LogManager.Configuration.ConfiguredNamedTargets.Count != 0)
            {
                Target t = LogManager.Configuration.FindTargetByName(targetName);
                if (t != null)
                {
                    Layout layout = (t as FileTarget).FileName;
                    rtnVal = layout.Render(LogEventInfo.CreateNullEvent());
                }
            }

            return rtnVal;
        }


        #region Log Config

        void LoggerConfig()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new FileTarget("logfile") { FileName = sLogNameFull };
            var logconsole = new ConsoleTarget("logconsole");

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
            logger.Info("Hello world");
            logger.Info("Hello {Name}", "Earth");
            logger.Info("Hello {0}", "Earth");
                      
        }
        #endregion

        #endregion

    }
}

