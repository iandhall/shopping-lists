using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using ShoppingLists.Web.Hubs;
using ShoppingLists.Web.Models;
using ShoppingLists.BusinessLayer.Exceptions;
using LogForMe;

namespace ShoppingLists.Web.HubPipelineModules
{
    // This module attempts to prevent potentially malicious script markup from passing between the client and the server.
    //
    // To do: Find a better way of doing all of this.
    public class ScriptDetectionPipelineModule : HubPipelineModule
    {
        // In order to prevent malicious script from getting into the database, this method compares the incoming arguments (i.e. model object, string etc.) with the encoded version of its self. If the encoded value is different then we assume that the data contains script markup and an exception gets thrown.
        //
        // Note: Only works if the Hub method argument object has a ToString() representation of all it's fields. It won't work for object graphs as it will only call the ToString() of the root object. It won't work for collection types as their ToString()s just report the type name.
        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            Logger.Debug("");
            foreach (var arg in context.Args)
            {
                if (EncodingHelper.Encode(arg.ToString()) != arg.ToString())
                {
                    context.Hub.Clients.Caller.serviceException(typeof(ScriptInjectionException).Name);
                    throw new ScriptInjectionException(arg);
                }
            }
            return base.OnBeforeIncoming(context);
        }

        // Encode outgoing data.
        //
        // Warning: not all type are supported (see below).
        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        {
            Logger.Debug("");
            var args = context.Invocation.Args;
            for (int argI = 0; argI < args.Length; argI++)
            {
                if (typeof(IEncodable).IsAssignableFrom(args[argI].GetType()))
                {
                    IEncodable model = (IEncodable)args[argI];
                    model.Encode();
                }
                else if (args[argI].GetType() == typeof(string))
                {
                    args[argI] = HttpUtility.HtmlEncode(args[argI].ToString());
                }
                else if (args[argI].GetType() == typeof(List<string>))
                {
                    var items = (List<string>)args[argI];
                    for (int itemI = 0; itemI < items.Count; itemI++)
                    {
                        items[itemI] = HttpUtility.HtmlEncode(items[itemI]);
                    }
                    //} else {
                    //    Won't get encoded!
                }
            }
            return base.OnBeforeOutgoing(context);
        }
    }
}