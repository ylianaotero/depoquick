﻿namespace BusinessLogic.Exceptions.ControllerExceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message) : base(message) {}   
}