﻿namespace DepoQuick.Exceptions.DateRangeExceptions;

public class EmptyDateRangeException : Exception
{
    public EmptyDateRangeException(string message) : base(message) {}
}