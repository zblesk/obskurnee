﻿namespace Obskurnee;

public abstract class ObskurneeException : Exception
{
    public virtual int StatusCode { get; set; } = 500;

    public ObskurneeException() : base()
    {
    }

    public ObskurneeException(string message) : base(message)
    {
    }

    public ObskurneeException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class PermissionException : ObskurneeException
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

public class ForbiddenException : ObskurneeException
{
    public ForbiddenException() : base()
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class DatastoreException : ObskurneeException
{
    public DatastoreException() : base()
    {
    }

    public DatastoreException(string message) : base(message)
    {
    }

    public DatastoreException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class ValidationException : ObskurneeException
{
    public override int StatusCode { get; set; } = 400;

    public ValidationException() : base()
    {
    }

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
