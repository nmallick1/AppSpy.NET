using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AppSpy.NET.Models
{
    public static class MethodNameParser
    {
        public static ParsedMethodName Parse(string methodName)
        {
            if (!string.IsNullOrWhiteSpace(methodName))
            {
                if (methodName.IndexOf("!", StringComparison.InvariantCultureIgnoreCase) < 0)
                {
                    throw new ArgumentException("Supplied method name must contain a ! symbol to help identify the containing DLL.", "methodName");
                }

                if (methodName.IndexOf(".", StringComparison.InvariantCultureIgnoreCase) < 0)
                {
                    throw new ArgumentException("Supplied method name must contain a . symbol to help identify type and method name.", "methodName");
                }

                string originalMethodName = methodName;


                // Strip () and any +0x123 that might be at the end of the method.
                string regex = string.Format(@"(.*)\((.*)\)\+0x[a-fA-F0-9]+");

                Match myMatch = Regex.Match(methodName, regex, RegexOptions.IgnoreCase);
                if (myMatch.Success)
                {
                    methodName = myMatch.Groups[1].ToString();
                    if (!string.IsNullOrEmpty(myMatch.Groups[2].ToString()))
                    {
                        methodName += "(" + myMatch.Groups[2].ToString() + ")";
                    }
                }
                // Squish spaces out
                methodName = methodName.Replace(" ", string.Empty);

                int moduleSeparator = methodName.LastIndexOf("!");
                if (moduleSeparator < 0)
                {
                    throw new ArgumentException("Supplied method name must contain a ! symbol to help identify the containing DLL.", "ModuleName");
                }

                string moduleName = methodName.Substring(0, moduleSeparator);
                string dllName = $"{moduleName}.dll";
                string fullMethodName = methodName.Substring(moduleSeparator + 1);
                string fullMethodNameWithoutParams = string.Empty;

                int paramSeparator = fullMethodName.LastIndexOf("(");

                if (paramSeparator > 0)
                    fullMethodNameWithoutParams = fullMethodName.Substring(0, paramSeparator);
                else
                    fullMethodNameWithoutParams = fullMethodName;

                //Strip[]
                string regex2 = string.Format(@"(.*)\[\[.*\]\]");
                Match myMatch2 = Regex.Match(fullMethodNameWithoutParams, regex2, RegexOptions.IgnoreCase);
                if (myMatch2.Success)
                {
                    fullMethodNameWithoutParams = myMatch2.Groups[1].ToString();
                }

                int typeSeparator = fullMethodNameWithoutParams.LastIndexOf(".");
                if (typeSeparator < 0)
                {
                    throw new ArgumentException("Supplied method name must contain a . symbol to help identify type and method name.", "FullTypeName");
                }

                // Function names can begin with . (like .ctor). In that case, we just matched the last of multiple .
                //     Back off to find the first . in the sequence
                while (typeSeparator > 1 && fullMethodNameWithoutParams[typeSeparator - 1] == '.')
                {
                    typeSeparator--;
                }

                string fullTypeName = fullMethodNameWithoutParams.Substring(0, typeSeparator);
                string pMethodName = fullMethodNameWithoutParams.Substring(typeSeparator + 1);
                if (pMethodName.IndexOf("<", StringComparison.InvariantCultureIgnoreCase) > -1 && pMethodName.IndexOf(">", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    //Async and overload methods have their names inside <>
                    Regex getFunctionNameRegEx = new Regex(@"[\<]+((?<functionName>[^>]+))>");
                    pMethodName = getFunctionNameRegEx.Match(pMethodName).Groups["functionName"].Value;
                }
                string hashedMethodName = $"{fullTypeName}.{pMethodName.Replace('.', '#')}";
                string fullHashedMethodName = fullMethodName.Replace(".#", "..").Replace("System.", string.Empty);
                string normalizedMethodName = fullHashedMethodName; //Creating a new property for readability. The values here are the same.

                string sourceType = string.Empty;
                string tempNested = string.Empty;
                string[] nestedTypes = new string[0];

                int lastClassSeparator = fullTypeName.IndexOf("+");

                if (lastClassSeparator > -1)
                {
                    sourceType = fullTypeName.Substring(0, lastClassSeparator);
                    tempNested = fullTypeName.Substring(lastClassSeparator + 1);

                    nestedTypes = tempNested.Split('+');
                }

                return new ParsedMethodName(originalMethodName, normalizedMethodName, dllName, moduleName, pMethodName, hashedMethodName, fullMethodName, fullHashedMethodName,
                    fullMethodNameWithoutParams, fullTypeName, sourceType, nestedTypes);

            }
            else
            {
                throw new ArgumentNullException("methodName", "Cannot instantiate parser with null or empty method name.");
            }
        }
    }
}