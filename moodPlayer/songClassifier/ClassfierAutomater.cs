using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace songClassifier
{
    class ClassfierAutomater
    {
        private string m_fileName;
        private string m_rootDir;
        private string m_convertDir;

        private string convertToWAV(string filePath)
        {
            string outFileName = string.Empty;
            if (!Directory.Exists(m_convertDir))
            {
                Directory.CreateDirectory(m_convertDir);
            }

            Process p = new Process();
            FileInfo info = new FileInfo(filePath);
            string directory = info.Directory.FullName;
            string name = info.Name;
            outFileName = m_convertDir + @"\"+name+".wav";
            if (File.Exists(outFileName))
            {
                File.Delete(outFileName);
            }
            p.StartInfo.FileName = m_rootDir + @"\convertBatch.bat";
            p.StartInfo.Arguments = "\""+outFileName+"\"" + " " + "\""+filePath+"\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            return outFileName;
        }

        private void classify(string path)
        {
            FileInfo info = new FileInfo(path);
            string directory = info.Directory.FullName;
            string fileName = info.Name;

            Process p = new Process();
            p.StartInfo.FileName = m_rootDir + @"\classify.bat";
            p.StartInfo.Arguments = "\"classifierInvoker '" + directory + "'" + " " + "'" + fileName + "'\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
        }

        public ClassfierAutomater()
        {
            string path = Application.ExecutablePath;
            m_rootDir = new FileInfo(path).Directory.FullName;
            m_convertDir = m_rootDir + @"\tempConvert";
        }

        public void perFormClassification(string path)
        {
            string wavFile = convertToWAV(path);
            classify(wavFile);
        }
    }
}
