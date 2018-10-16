using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeFileNames
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileDir = @"E:\Documents\Projects\Xamarin\TestRecyclerView\TestRecyclerView\Resources\drawable";
            const string subStrInFileNameToBeReplaced = @"bar-";
            const string strForReplacement = @"scrollBar_new_";
            const string fileIdxIdentifierFromEnd = @"-";
            const int numOfDigitsInFileIdx = 5;

            // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-enumerate-directories-and-files
            try
            {
                List<string> files = new List<string>(Directory.EnumerateFiles(fileDir));

                foreach (var file in files)
                {
                    string oldFilePath = Path.Combine(fileDir, file);
                    string newFilePath = GetNewFileName(oldFilePath, fileIdxIdentifierFromEnd, numOfDigitsInFileIdx, 
                        subStrInFileNameToBeReplaced, strForReplacement);
                    Console.WriteLine(newFilePath);
                    File.Move(oldFilePath, newFilePath);
                }               
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.ToString());
            }
            catch (PathTooLongException PathEx)
            {
                Console.WriteLine(PathEx.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();
        }

        private static string GetFileIdxStrFromFileName(string fileName, string fileIdxIdentifierFromEnd, int numOfDigitsInFileIdx, out string oldFileIdxStr)
        {
            int fileIdxStartPos = fileName.LastIndexOf(fileIdxIdentifierFromEnd) + 1;

            if (fileIdxStartPos < 0)
            {
                throw new Exception("fileIdxIdentifierFromEnd: '" + fileIdxIdentifierFromEnd + "' not found in fileName: " + fileName);
            }

            int extensionStartPos = fileName.LastIndexOf(".");

            if (extensionStartPos < 0)
            {
                throw new Exception("fileExtensionIdentifierFromEnd: '" + "." + "' not found in fileName: " + fileName);
            }

            oldFileIdxStr = fileName.Substring(fileIdxStartPos, extensionStartPos - fileIdxStartPos);
            int fileIdx = Convert.ToInt32(oldFileIdxStr);

            return fileIdx.ToString("D" + numOfDigitsInFileIdx.ToString());
        }

        private static string GetNewFileName(string oldFileName, string fileIdxIdentifierFromEnd, int numOfDigitsInFileIdx, 
            string subStrToBeReplaced, string strForReplacement)
        {
            string oldFileIdxStr;
            string newFileIdxStr = GetFileIdxStrFromFileName(oldFileName, fileIdxIdentifierFromEnd, numOfDigitsInFileIdx, out oldFileIdxStr);

            return oldFileName.Replace(subStrToBeReplaced, strForReplacement).Replace(oldFileIdxStr, newFileIdxStr);
        }
    }
}
