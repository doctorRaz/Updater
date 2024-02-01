/*
//todo код только начат
из основного приложения передаем:
    путь имя файла к вызывающего
    корневой путь к узлу обновлений
по ним получаем имя приложения и версию до мажор **plotspds_1.1**
подтыкаем расширение sXMLext = ".xmlpack";

дергаем метод Update
    забираем с узла файл хмл
         в зависимости от настроек (сообщать об обновлении, обновлять молча) все сообщения про обновления (если нужны) генерирует updater
         вызывающий генерит месадж, только если были ошибки или другие косяки обновления
    читаем сравниваем версию вызывающего и ту что в хмл
    если в хмл свежее
    из хмл берем имя zip для скачивания

качаем
        убиваем все найденные bak (try cath) при запуске updater
распаковываем в темп (с проверкой существования каталога,
    если есть пытаемся убить
        убить не дал, делаем рядом с другим именем
начинаем обновлять с файлов не имеющих версии , секция Module Packaje
     по относительным путям без затей перезаписываем из zip существующие lsp cfg packaje, кроме
    //think dll проверить библиотеки ресурсов дают ли себя перезаписать (возможно, какие то файлы то же не дадут себя перезаписать)
затем по относительным путям ищем файлы с версией, и переименовываем их в bak (с проверкой имени если сущ, то другое имя plot(2).bak
    копируем распакованное вместо переименованного
последним обновляем файл который инициировал обновление
 убиваем каталог в темпе (распакованный zip)
сообщаем пользователю, что нужна перезагрузка





*/

using drz.XMLSerialize;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace drz.Updater
{
    /// <summary>
    ///Распаковка пакета обновления
    ///чтение в XML и распаковка
    /// </summary>
    public partial class UnWrapper : IDisposable
    //? Заготовка, отсюда инит класса дальше все автоматически
    {

        #region INIT CLASS

        #region Пути имена

        /// <summary>
        /// Список файлов для архивации
        /// </summary>
        internal List<string> arrFiletoZIP { get; set; }

        /// <summary> директория приложения 
        /// <br> задаем один раз потом не меняем</br>
        /// <br> относительно этого пути будет все собираться</br>
        /// </summary>
        internal string sDirFiles { get; set; }

        /// <summary>
        /// директория над каталогом приложения
        /// </summary>
        internal string sDirParent { get; set; }

        /// <summary>
        /// полный путь к обновляемому модулю
        /// </summary>
        /// <value>
        /// The s asm full path.
        /// </value>
        public string sMainAssemblyFullPatch { get; set; }

        /// <summary>
        /// адрес ресурса обновления
        /// </summary>
        /// <value>
        /// The s URL domain.
        /// </value>
        public string sUrlDomain { get; set; }

        /// <summary>
        /// папочка куда предварительно копируем компоненты приложения
        /// </summary>
        internal static string sDirTemp => Path.GetTempPath();


        /// <summary>файлы приложения, библиотек и прочих файлов</summary>
        string[] sFilePrgs { get; set; }

        #endregion

        #region XML

        /// <summary>
        /// расширение XML
        /// </summary>
        const string sXMLext = ".xmlpack";

        /// <summary>
        /// Имя по умолчанию для сохранения XML
        /// </summary>
        internal string sUrlXML => Path.Combine(sUrlDomain, sProductName, sShortName + sXMLext);// { get; set; }

        #endregion

        #region Product

        /// <summary>
        /// название продукта
        /// </summary>
        internal string sProductName { get; set; }

        /// <summary>
        /// Версия главного файла мажор.минор
        /// </summary>
        internal string sMajorMinorVersion
        {
            get
            {
                //до минора версия
                return vVersion.Major + "." + vVersion.Minor;
            }
        }

        /// <summary>
        /// полная версия главного файла
        /// </summary>
        Version vVersion { get; set; }


        //think при запросе на сервер приложение само собирает из своих ProductName и MajorMinorVersion
        //и запрашивает такой файл на сервере
        /// <summary>
        /// имя файла для XLS
        /// </summary>
        internal string sShortName => sProductName + "_" + sMajorMinorVersion;

        #endregion

        #region  Err report

        /// <summary>Сообщения о ошибках</summary>
        public string sErr { get; set; }

        /// <summary>Обновление успешно</summary>
        public bool isUpdateSuccess { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is error.
        /// Если ошибка инит
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is error; otherwise, <c>false</c>.
        /// </value>
        public bool isOk { get; set; }

        #endregion






        /// <summary>
        /// Initializes a new instance of the <see cref="UnWrapper"/> class.
        /// </summary>
        public UnWrapper(string sAsmFullPath)
        {
            try
            {


                //отсюда дергаем приложением запрос на обновление
                //получаем имя файла сборки
                sMainAssemblyFullPatch = sAsmFullPath;

                //заполнили свойства главной сборки
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(sMainAssemblyFullPatch);

                //продукт
                sProductName = fileVersionInfo.ProductName.ToLower();
                //версия
                vVersion = new Version(fileVersionInfo.FileVersion);


                //versionInfPrj.FileVersion;

                //получаем имя приложения, версию сборки


                //инит Ок
                isOk = true;
            }

            catch (Exception ex)
            {
                logger.Error(ex, (sMainAssemblyFullPatch));

                sErr = ex.Message
                     + "\n"
                    + "Обновление невозможно"
                    + "\n"
                  + "Логфайл находится по пути: "//think предлагать открыть файл с ошибками
                  + sLogNameFull
                  + "\n"
                  + "Отправьте файл разработчику"
                    ;
                //logger.Error(sErr);
            }
            /*
            ROOT = new root();
            //!модуль проектов
            Projects = new List<rootProject>();
            //пристегиваем к рут
            ROOT.Projects = Projects;

            //!модуль прочих файлов
            Modules = new List<rootModule>();
            //пристегиваем к рут
            ROOT.Modules = Modules;

            //!package
            Packages = new List<rootPackage>();
            //пристегиваем к рут
            ROOT.Packages = Packages;

            //!описание
            Descriptions = new List<rootDescription>();
            //пристегиваем
            ROOT.Descriptions = Descriptions;

            ofd = new OpenFileDialog();
            arrFiletoZIP = new List<string>();//пустой массив

            #region ROOT IFNO

            //пароль для zip default
            ROOT.PasswordZIP = "12345";

            //date int Wrapped
            ROOT.DateCreate = (int)DateTime.Today.ToOADate();
            //дата время создания
            ROOT.DateTime = DateTime.Now.ToString("g");
            
            #endregion

            */
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose");
            //throw new NotImplementedException();
        }



        #region Serialization

        /// <summary>
        /// корень коллекции
        /// </summary>
        public root ROOT { get; set; }

        /// <summary>
        /// The projects
        /// </summary>
        List<rootProject> Projects;

        /// <summary>
        /// The modules
        /// </summary>
        List<rootModule> Modules;

        /// <summary>
        /// The package
        /// </summary>
        List<rootPackage> Packages;

        /// <summary>
        /// The description
        /// </summary>
        List<rootDescription> Descriptions;

        #endregion

        #region ZIP

        /// <summary>
        /// Имя по умолчанию для сохранения XML
        /// </summary>
        internal string sFullNameZIP => Path.Combine(sDirParent, sShortName + ".zip");// { get; set; }

        #endregion

        /* #region OFD

         /// <summary>файлы приложения</summary>
         internal OpenFileDialog ofd { get; set; }


         #endregion
       */
        /* #region Маски

       /// <summary>
       /// Маски включаемых файлов, которые брать
       /// </summary>
       List<string> arrIncludedSupportedExt
       {
           get
           {
               return new List<string>
                {
                    "*.exe",
                    "*.dll",
                    "*.cfg",
                    "*.cuix",
                    "*.lsp",
                    "*.xml",
                    "*.json"
                };
           }
       }

#endregion
      */

        #endregion


        #region Modules        

        /// <summary>
        /// Updaters this instance.
        /// </summary>
        /// <returns></returns>
        bool updater()
        {
            return true;
        }



        #endregion

    }
}

