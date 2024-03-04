using drz.XMLSerialize;

using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace drz.Updater
{
    /// <summary>
    ///Подготовка пакета обновления
    ///описание в XML и упаковка
    /// </summary>
    public partial class Wrapper

    {
        #region DescriptorWriter

        #endregion
        /// <summary>
        /// писатель обновления, что нового
        /// </summary>
        internal bool WrapperDescription
        {
            get
            {
                #region OFD

                ofd.Multiselect = true;
                ofd.Title = "Выбери файл описания";
                ofd.Filter = "Все файлы (*.*)|*.*|"
                             + ".txt (*.txt)|"
                             + "*.txt";
                ofd.FilterIndex = 2;
                ofd.RestoreDirectory = false;

                //!получим файл описания         
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    sErr = "Файл Обновления не загружен!\nПользователь отказался!";
                    return false;
                }

                #endregion
                sFilePrg = ofd.FileName;

                using (StreamReader reader = new StreamReader(sFilePrg, Encoding.Default))
                {
                    string line;
                    int iRow = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        iRow++;
                        Debug.WriteLine(iRow + " " + line);
                        rootDescription Description = new rootDescription
                        {
                            Row=iRow,
                            Counter=iRow.ToString(),
                            Content=line
                        };
                        Descriptions.Add(Description);
                    }
                }

                return true;
            }
        }

    }
}

