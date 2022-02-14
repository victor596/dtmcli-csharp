﻿using Google.Protobuf;
using Grpc.Core;

namespace Dtmgrpc.DtmGImp
{
    internal class Utils
    {
        internal static string ToJsonString(object obj)
        { 
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }

        internal static string DtmGet(ServerCallContext context, string key)
        {
            var metadataEntry = context.RequestHeaders.FirstOrDefault(m => m.Key.Equals(key));

            if (metadataEntry.Equals(default(Metadata.Entry)) || metadataEntry.Value == null)
            {
                return null;
            }

            return metadataEntry.Value;
        }

        internal static Metadata TransInfo2Metadata(string gid, string transType, string branchId, string op, string dtm)
        {
            var metadata = new Metadata();
            metadata.Add(Constant.Md.Gid, gid);
            metadata.Add(Constant.Md.TransType, transType);
            metadata.Add(Constant.Md.BranchId, branchId);
            metadata.Add(Constant.Md.Op, op);
            metadata.Add(Constant.Md.Dtm, dtm);

            return metadata;
        }

        internal static Method<TRequest, TResponse> CreateMethod<TRequest, TResponse>(MethodType methodType, string serviceName, string methodName)
            where TRequest : class, IMessage, new()
            where TResponse : class, IMessage, new()
        {
            return new Method<TRequest, TResponse>(
                methodType,
                serviceName,
                methodName,
                CreateMarshaller<TRequest>(),
                CreateMarshaller<TResponse>());
        }

        internal static Marshaller<TMessage> CreateMarshaller<TMessage>()
              where TMessage : class, IMessage, new()
        {
            return new Marshaller<TMessage>(
                m => m.ToByteArray(),
                d =>
                {
                    var m = new TMessage();
                    m.MergeFrom(d);
                    return m;
                });
        }
    }
}