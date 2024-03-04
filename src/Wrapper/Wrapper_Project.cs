using drz.XMLSerialize;

using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace drz.Updater
{
    /// <summary>
    ///Подготовка пакета обновления
    ///описание в XML и упаковка
    /// </summary>
    public partial class Wrapper

    {
        #region FilePropWriter

        /// <summary>
        /// Gets a value indicating whether [wrapper PRJ or modules].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [wrapper PRJ or modules]; otherwise, <c>false</c>.
        /// </value>
        internal bool WrapperProjectAndModules
        {
            get
            {
                #region OFD Prop
                ofd.Multiselect = true;
                ofd.Title = "Выбери главные файлы проекта";
                ofd.Filter = "Все файлы (*.*)|*.*|"
                             + ".NET assemblies (*.exe;*.dll)|"
                             + "*.exe;*.dll";
                ofd.FilterIndex = 2;
                ofd.RestoreDirectory = true;

                //!получим файлы приложения            
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    sErr = "Файл DLL не загружен! Пользователь отказался!";
                    return false;
                }
                sFilePrgs = ofd.FileNames;
                sFilePrg = ofd.FileName;

                #endregion

                #region проверка годится ли файл есть ли там FileVersion
 
                foreach (string item in sFilePrgs)
                {
                    sFilePrg = item;
                    if (string.IsNullOrEmpty(versionInfPrj.FileVersion))
                    {
                        sErr = "Неподходящий файл для главного файла проекта!"
                            + "\n"
                            + sFilePrg;
                        return false;
                    }
                }
             
                #endregion

                #region Set general properties /прибито гвоздями/

                //!+ корневая директория приложения
                sDirFiles = Directory.GetParent(sFilePrg).FullName;

                //!+директория над каталогом приложения
                sDirParent = Directory.GetParent(sDirFiles).FullName;

                //!+продукт 
                sProductName = versionInfPrj.ProductName.ToLower();

                //!+полная версия файла
                sVersion = versionInfPrj.FileVersion;
                               
                #endregion
                
                //***

                //фильтр включаемых масок
                string sIncludedSupportedExt = string.Join(",", arrIncludedSupportedExt);

                //список файлов для перебора
                string[] sSetFiles;

                if (isWrapAllFile)//писать все файлы
                {
                    sSetFiles = Directory.GetFiles(sDirFiles, "*.*", SearchOption.AllDirectories).Where(s => sIncludedSupportedExt.Contains(Path.GetExtension(s).ToLower())).ToArray();
                }
                else//только выбранные
                {
                    sSetFiles = sFilePrgs;
                }

                //!пошли перебирать файлы
                foreach (string file in sSetFiles)
                {
                    sFilePrg = file;

                    //!+ если файл имеет FileVersion то в Project иначе в модули
                    if (!string.IsNullOrEmpty(versionInfPrj.FileVersion))
                    {
                        #region Project

                        rootProject Project = new rootProject
                        {
                            Project = versionInfPrj.FileDescription,

                            //! атрибуты пока тянем все подряд из доступных
#if NF
                            RefPath = PathNetCore.GetRelativePath(sDirFiles, sFilePrg),//NF не умеет GetRelativePath
#else
                            RefPath = Path.GetRelativePath(sDirFiles, sFilePrg),//! NF не умеет GetRelativePath
#endif
                            FileName = Path.GetFileName(sFilePrg),
                            ProductName = versionInfPrj.ProductName,
                            FileDescription = versionInfPrj.FileDescription,
                            OriginalFilename = versionInfPrj.OriginalFilename,
                            InternalName = versionInfPrj.InternalName,
                            FileVersion = versionInfPrj.FileVersion,
                            ProductVersion = versionInfPrj.ProductVersion,
                            ////может когда пригодиться 
                            //LegalTrademarks = versionInfPrj.LegalTrademarks,
                            //LegalCopyright = versionInfPrj.LegalCopyright,
                            //CompanyName = versionInfPrj.CompanyName,
                            //Comments = versionInfPrj.Comments,
                        };
                        //добавим к root
                        Projects.Add(Project);

                        //!если файл выбран разработчиком проекта
                        if (sFilePrgs.Contains(file))
                        {
                            //признак, что файл в корне каталога и был выбран разработчиком, пока не знаю нафига
                            Project.root = true;
                        }
                        else
                        {
                            Project.root = false;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Module
                        rootModule Module = new rootModule
                        {
#if NF
                            RefPath = PathNetCore.GetRelativePath(sDirFiles, sFilePrg),//заглушка для фрэмворка, относительный путь
#else
                            RefPath = Path.GetRelativePath(sDirFiles, sFilePrg),
#endif
                            FileName = Path.GetFileName(sFilePrg),
                        };
                        Modules.Add(Module);
                        #endregion
                    }
                    //! список вех файлов для упаковки
                    arrFiletoZIP.Add(sFilePrg);
                }

                return true;
            }
        }

        #endregion
    }
}

