using System;

namespace TabuleiroExceptions
{
    class TabuleiroException : Exception
    {
        public TabuleiroException(string msg) : base(msg){
        }
    }
}
