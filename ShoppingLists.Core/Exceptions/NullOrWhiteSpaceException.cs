﻿using System;

namespace ShoppingLists.Core.Exceptions
{
    public class NullOrWhiteSpaceException : ArgumentException
    {
        public NullOrWhiteSpaceException(string parameterName) : base(
            "Parameter {0} can't be null, empty or contain only whitespace characters.",
            parameterName
        ) { }
    }
}