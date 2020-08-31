using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection.Metadata;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.CSharp.OutputVisitor;
using AppSpy.NET.Models;

namespace AppSpy.NET.Decompiler
{
    public class CodeDecompiler
    {
        public CSharpDecompiler decompiler { get; }
        CSharpAmbience amb;
        private string sourceCodeBasePath = string.Empty;
        private void ConsoleWriteLine(string msg, ConsoleColor? color)
        {
            //if (color != null)
            //{
            //    Console.BackgroundColor = (ConsoleColor)color;
            //    Console.WriteLine(msg);
            //    Console.ResetColor();
            //}
            //else
            //{
            //    Console.WriteLine(msg);
            //}
        }

        private ParsedMethodName _mp = null;
        public CodeDecompiler(ParsedMethodName mp)
        {
            if (mp == null)
                throw new ArgumentNullException("Failed to instantiate CodeDecompiler due to NULL parameter.", "MethodNameParser mp");
            this._mp = mp;
            this.sourceCodeBasePath = System.IO.Path.GetFullPath($"{HttpRuntime.AppDomainAppPath.ToString()}..\\DemoWebCamp\\SourceCode");
            this.decompiler = new CSharpDecompiler($@"{this.sourceCodeBasePath}\bin\{this._mp.DllName}", new ICSharpCode.Decompiler.DecompilerSettings());
            this.amb = new CSharpAmbience();
        }

        private string FindMatchingMethodToken()
        {
            return string.Empty;
        }

        private string NormalizeSig(string FullName)
        {
            return FullName?.Replace(".#", "..").Replace("System.", string.Empty);
        }

        public string DecompileMethod()
        {
            List<string> matchedMethods = new List<string>();
            ConsoleWriteLine("********************* Enter DecompileMethod*********************", null);
            ConsoleWriteLine(JsonConvert.SerializeObject(this._mp, Formatting.Indented), null);
            if (!File.Exists($@"C:\Users\nmallick\source\repos\DecompilerTest\DemoWebCamp\SourceCode\bin\{this._mp.DllName}"))
            {
                ConsoleWriteLine("DLL not found, so cannot extract function to decompile.", ConsoleColor.DarkRed);
            }
            else
            {

                //decompiler.
                foreach (ITypeDefinition typeDef in this.decompiler.TypeSystem.GetAllTypeDefinitions())
                {
                    //ConsoleWriteLine($"Checking {typeDef.FullName}", null);
                    string typeName = String.Empty;
                    if (this._mp.NestedTypes.Length > 0)
                        typeName = this._mp.SourceType;
                    else
                        typeName = this._mp.FullTypeName;

                    if (typeName != typeDef.FullName && typeName.StartsWith(typeDef.FullName) && typeName.Length < typeDef.FullName.Length && typeName[typeDef.FullName.Length] == '.')
                    {
                        typeName = typeDef.FullName;
                    }

                    if (typeDef.FullName.Equals(typeName))
                    {
                        ConsoleWriteLine($"Matched type {typeDef.FullName} with {typeName}", ConsoleColor.DarkGreen);
                        if (this._mp.NestedTypes.Length > 0)
                        {
                            //This will be true if the supplied function is an async/member function within a nested class.
                            //Get from clrdbg.cs line#643
                            this.ConsoleWriteLine("Skipping this check for now.", null);
                        }
                        else
                        {
                            foreach (IMember m in typeDef.Members)
                            {
                                ConsoleWriteLine($"Checking member.FullName {m.FullName} or {this.NormalizeSig(m.FullName)}", null);
                                if (m.FullName == this._mp.FullMethodName || m.FullName == this._mp.HashedMethodName || NormalizeSig(m.FullName) == NormalizeSig(this._mp.FullMethodName) || NormalizeSig(m.FullName) == NormalizeSig(this._mp.HashedMethodName))
                                {
                                    matchedMethods.Clear();
                                    matchedMethods.Add(m.FullName);
                                    this.ConsoleWriteLine($"Found a single matching method {m.FullName}", ConsoleColor.DarkGreen);


                                    //Decompile with token here.
                                    EntityHandle tokenOfFirstMethod = m.MetadataToken;
                                    string codeString = decompiler.DecompileAsString(tokenOfFirstMethod);

                                    this.ConsoleWriteLine(codeString, null);
                                    return codeString;
                                }
                                else
                                {
                                    this.ConsoleWriteLine($"Also Checking member.Name {m.FullName}", null);
                                    if (m.Name == this._mp.FullMethodNameWithoutParams || m.Name.StartsWith(this._mp.FullMethodName) || m.Name.StartsWith(this._mp.HashedMethodName))
                                    {
                                        matchedMethods.Add(m.FullName);
                                        this.ConsoleWriteLine($"Found a match for method {m.FullName}", ConsoleColor.DarkGreen);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // ConsoleWriteLine($"Did not match {typeDef.FullName} with {typeName}", null);
                    }
                    //StringBuilder codeSb = new StringBuilder(string.Empty);
                    //StringWriter sw = new StringWriter(codeSb);
                    //CSharpFormattingOptions _formattingOptions = FormattingOptionsFactory.CreateSharpDevelop();
                    //var visitor = new CSharpOutputVisitor(sw, _formattingOptions);



                    //IType t = tv.VisitTypeDefinition(typeDef);


                    //var token = t.GetDefinition().Methods.First().MetadataToken;
                }

            }
            ConsoleWriteLine("********************* Exit DecompileMethod*********************", null);
            return string.Empty;
        }
    }


}
