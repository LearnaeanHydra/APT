using System;

namespace BusinessLogic.Base
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
