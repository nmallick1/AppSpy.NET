using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppSpy.NET.Models
{    
    public class ParsedMethodName
    {
        public string OriginalMethodName { get; }
        public string NormalizedMethodName { get; }
        public string DllName { get; }
        public string ModuleName { get; }
        public string MethodName { get; }
        public string HashedMethodName { get; }
        public string FullMethodName { get; }
        public string FullHashedMethodName { get; }
        public string FullMethodNameWithoutParams { get; }
        public string FullTypeName { get; }
        public string SourceType { get; }
        public string[] NestedTypes { get; }

        public ParsedMethodName(string originalMethodName, string normalizedMethodName, string dllName, string moduleName, string methodName, string hashedMethodName,
            string fullMethodName, string fullHashedMethodName, string fullMethodNameWithoutParams, string fullTypeName, string sourceType, string[] nestedTypes)
        {
            if(string.IsNullOrWhiteSpace(originalMethodName) || string.IsNullOrWhiteSpace(normalizedMethodName) || string.IsNullOrWhiteSpace(dllName) 
                || string.IsNullOrWhiteSpace(moduleName) || string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentNullException("One of the parameters passed in was either NULL or empty");
            }
            this.OriginalMethodName = originalMethodName;
            this.NormalizedMethodName = normalizedMethodName;
            this.DllName = dllName;
            this.ModuleName = moduleName;
            this.MethodName = methodName;
            this.HashedMethodName = hashedMethodName;
            this.FullMethodName = fullMethodName;
            this.FullHashedMethodName = fullHashedMethodName;
            this.FullMethodNameWithoutParams = fullMethodNameWithoutParams;
            this.FullTypeName = fullTypeName;
            this.SourceType = sourceType;
            this.NestedTypes = nestedTypes;
        }
    }
}