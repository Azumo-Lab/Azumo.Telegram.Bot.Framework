namespace MyChannel.DataBaseContext.DBModels
{
    internal class FileIDInfoEntity : DBBase
    {
        /// <summary>
        /// 文件JSON的路径
        /// </summary>
        public string? JSONPath { get; set; }
    }
}
