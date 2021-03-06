﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class OutOfRangeException : ServiceException
    {
        public OutOfRangeException(string parameterName, Type parameterType) : base(
            "Parameter {0} value out of range of parameter type {1}.",
            parameterName,
            parameterType.Name
        ) { }
    }
}