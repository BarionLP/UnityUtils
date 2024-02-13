using System;
using System.IO;

namespace Ametrin.Utils{
    public static class DirectoryInfoExtensions{
        public static DirectoryInfo GetCopyOfPathIfExists(this DirectoryInfo directoryInfo){
            if (!directoryInfo.Exists) return directoryInfo;
            return directoryInfo.GetCopyOfPath().GetCopyOfPathIfExists();
        }

        public static DirectoryInfo GetCopyOfPath(this DirectoryInfo directoryInfo){
            return new(directoryInfo.FullName + " - Copy");
        }

        public static void CreateIfNotExists(this DirectoryInfo directoryInfo) {
            if (!directoryInfo.Exists) {
                directoryInfo.Create();
            }
        }

        public static FileInfo File(this DirectoryInfo directoryInfo, string fileName) => new(Path.Combine(directoryInfo.FullName, fileName));

        public static void ForeachFile(this DirectoryInfo directoryInfo, Action<FileInfo> action, IProgress<(float, string)> progress, SearchOption searchOption = SearchOption.AllDirectories, string pattern = "*"){
            if (progress is null){
                directoryInfo.ForeachFile(action);
                return;
            }

            var files = directoryInfo.GetFiles(pattern, searchOption);
            float totalFiles = files.Length;
            var processed = 0;
            foreach (var file in files){
                action(file);
                processed++;
                progress.Report((processed / totalFiles, file.FullName));
            }
        }

        public static void ForeachFile(this DirectoryInfo directoryInfo, Action<FileInfo> action, IProgress<float> progress, SearchOption searchOption = SearchOption.AllDirectories, string pattern = "*"){
            if (progress is null){
                directoryInfo.ForeachFile(action);
                return;
            }

            var files = directoryInfo.GetFiles(pattern, searchOption);
            float totalFiles = files.Length;
            var processed = 0;
            foreach (var file in files){
                action(file);
                processed++;
                progress.Report(processed / totalFiles);
            }
        }

        public static void ForeachFile(this DirectoryInfo directoryInfo, Action<FileInfo> action, SearchOption searchOption = SearchOption.AllDirectories, string pattern = "*"){
            foreach (var file in directoryInfo.GetFiles(pattern, searchOption)){
                action(file);
            }
        }
    }
}
