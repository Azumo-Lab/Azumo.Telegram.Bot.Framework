using System;

namespace Azumo.Lang
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Token
    {
        /// <summary>
        /// 
        /// </summary>
        public static Token EOF { get; } = new EOFToken();

        /// <summary>
        /// 
        /// </summary>
        public static Token EOL { get; } = new EOLToken();

        /// <summary>
        /// 
        /// </summary>
        public virtual string Value { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public virtual int Line { get; set; } = -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Line"></param>
        protected Token(int Line)
        {
            this.Line = Line;
        }

        public override string ToString()
        {
            return $"Token : ";
        }

        #region 运算符重载

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Token operator |(Token a, Token b)
        {
            return EOF;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Token operator &(Token a, Token b)
        {
            return EOL;
        }
        #endregion

        #region 内置的类 (EOL 和 EOF)

        /// <summary>
        /// 
        /// </summary>
        private class EOFToken : Token
        {
            /// <summary>
            /// 
            /// </summary>
            public EOFToken() : base(-2) { }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return $"{base.ToString()}End Of File Token";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class EOLToken : Token
        {
            /// <summary>
            /// 
            /// </summary>
            public EOLToken() : base(-1) 
            {
                Value = Environment.NewLine;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return $"{base.ToString()}End Of Line Token";
            }
        }

        #endregion
    }
}
