﻿namespace InnoClinic.Authorization.Application.Exceptions;
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
