﻿using System;

namespace Obskurnee
{
    public class PermissionException : Exception
    {
        public PermissionException() : base()
        {
        }

        public PermissionException(string message) : base(message)
        {
        }

        public PermissionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}