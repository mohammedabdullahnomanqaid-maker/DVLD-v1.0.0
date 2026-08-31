using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Course19
{
    class clsUtil
    {
        private static string _GenerateGUID()
        {
            //generate new GUID
            Guid newGUID = Guid.NewGuid();

            return newGUID.ToString();
        }

        private static bool _CreateFolderIfDoesNotExist(string FolderPath)
        {
            // if is not exist create it,and folderPath is the name of folder
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
             catch(Exception ex)
                {
                    MessageBox.Show("Error Creating Folder : " + ex.Message);
                    return false;
                }
            }
            return true;
        }

        private static string _ReplaceFileNameWithGUID(string sourceFileName)
        {
            //change file name to new file name
            FileInfo info = new FileInfo(sourceFileName);
            string ext = info.Extension;
            return _GenerateGUID() + ext;
        }

        public static bool CopyImageToProjectImagesFolder(ref string SourceFile)
        {
            // this funciton will copy the image to the
            // project images foldr after renaming it
            // with GUID with the same extention, then it will update the sourceFileName with the new name.

            string DestinationFolder = @"C:\DVLD-People-Images\";
            if (!_CreateFolderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }
            string DestinationFile = DestinationFolder + _ReplaceFileNameWithGUID(SourceFile);
            try
            {                                      // true just to be sure if image is exist doesnot copy it
                File.Copy(SourceFile, DestinationFile, true);
            }
            catch(IOException Iox)
            {
                MessageBox.Show(Iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            SourceFile = DestinationFile;
            return true;
        }
    }
}
