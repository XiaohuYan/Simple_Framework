namespace SimpleFramework.AB
{
    public class ABInfo
    {
        /// <summary>
        /// Ãû×Ö
        /// </summary>
        public readonly string name;

        /// <summary>
        /// ´óÐ¡
        /// </summary>
        public readonly long size;

        /// <summary>
        /// md5Âë
        /// </summary>
        public readonly string md5;

        public ABInfo(string name, string size, string md5)
        {
            this.name = name;
            this.size = long.Parse(size);
            this.md5 = md5;
        }
    }
}