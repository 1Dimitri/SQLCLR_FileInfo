using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System;

public partial class UserDefinedFunctions
{

    private class FileProperties
    {
        public SqlString FileName;
        public SqlInt64 FileSize;
        public SqlDateTime CreationTime;
        public FileProperties(SqlString fileName, SqlInt64 fileSize,
        SqlDateTime creationTime)
        {
            FileName = fileName;
            FileSize = fileSize;
            CreationTime = creationTime;
        }
    }


    [Microsoft.SqlServer.Server.SqlFunction(
    FillRowMethodName = "GetOneFile",
    TableDefinition = "FileName nvarchar(512), FileSize bigint, CreationTime datetime")]

    public static IEnumerable GetFileList(string targetDirectory,
    string searchPattern)
    {
        try
        {
            ArrayList FilePropertiesCollection = new ArrayList();
            DirectoryInfo dirInfo = new DirectoryInfo(targetDirectory);
            FileInfo[] files = dirInfo.GetFiles(searchPattern);
            foreach (FileInfo fileInfo in files)
            {
                FilePropertiesCollection.Add(new FileProperties(fileInfo.FullName,
                fileInfo.Length, fileInfo.CreationTime));
            }
            return FilePropertiesCollection;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static void GetOneFile(object objFileProperties, out SqlString fileName,
  out SqlInt64 fileSize, out SqlDateTime creationTime)
    {
        
        FileProperties fileProperties = (FileProperties)objFileProperties;
        fileName = fileProperties.FileName;
        fileSize = fileProperties.FileSize;
        creationTime = fileProperties.CreationTime;
    }


}

   
 