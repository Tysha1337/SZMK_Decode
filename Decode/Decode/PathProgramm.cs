using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Decode
{
    public class PathProgramm
    {
        private readonly String _ConnectApplicationPath;
        private readonly String _LogPath;
        private readonly String _TempFile;


        public PathProgramm()
        {
            _ConnectApplicationPath = Application.StartupPath + $@"\Connect\Application\connect.conf"; //Параметры подключения к приложению
            _LogPath = Application.StartupPath+$@"\Log"; //Путь к директории хранения логов
            _TempFile = Application.StartupPath + $@"\TempFile";//Путь к темповым файлам распознования
        }
        public String GetDirectory(String Path)
        {
            String[] Temp = Path.Split('\\');
            String Directory = String.Empty;

            for (Int32 i = 0; i < Temp.Length - 1; i++)
            {
                Directory += Temp[i];
            }

            return Directory;
        }
        public String LogPath
        {
            get
            {
                return _LogPath;
            }
        }

        public String ConnectApplicationPath
        {
            get
            {
                return _ConnectApplicationPath;
            }
        }
        public String TempFile
        {
            get
            {
                return _TempFile;
            }
        }
    }
}
