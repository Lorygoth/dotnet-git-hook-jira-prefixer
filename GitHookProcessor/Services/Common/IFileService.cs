namespace GitHookProcessor.Services.Common
{
    public interface IFileService
    {
        string Read(string path);
        bool Write(string path, string text);
    }
}