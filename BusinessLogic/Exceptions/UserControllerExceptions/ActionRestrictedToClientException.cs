﻿namespace BusinessLogic.Exceptions.UserControllerExceptions;

public class ActionRestrictedToClientException : Exception
{
    public ActionRestrictedToClientException(string message) : base(message) {} 
}