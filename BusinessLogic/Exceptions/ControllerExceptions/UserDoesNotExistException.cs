﻿namespace BusinessLogic.Exceptions.ControllerExceptions;

public class UserDoesNotExistException : Exception
{
    public UserDoesNotExistException(string message) : base(message) {} 
}