namespace Workspaces
{
    using System.IO;

    public class Util
    {
        public static string GetProperDirectoryCapitalization(DirectoryInfo dirInfo)
        {
            DirectoryInfo parentDirInfo = dirInfo.Parent;
            if (null == parentDirInfo)
            {
                return dirInfo.Name.ToUpper();
            }

            return Path.Combine(GetProperDirectoryCapitalization(parentDirInfo),
                                parentDirInfo.GetDirectories(dirInfo.Name)[0].Name);
        }

        public static string GetProperFilePathCapitalization(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            if (!fileInfo.Exists) return filename;
            DirectoryInfo dirInfo = fileInfo.Directory;
            if (dirInfo.GetFiles(fileInfo.Name).Length == 0) return filename;
            return Path.Combine(GetProperDirectoryCapitalization(dirInfo),
                            dirInfo.GetFiles(fileInfo.Name)[0].Name);
        }
    }
}
