using AppSpy.NET.Decompiler;
using AppSpy.NET.Models;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AppSpy.NET.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DecompileMethodController : ApiController
    {

        // GET: api/decompileMethod?fullyQualifiedFunctionName=mawshol!demomvp.HighCPUPage1.ForceCpuUsage(int32)
        
        [Route("api/decompileMethod")]       
        public string Get(string fullyQualifiedFunctionName)
        {
            if(string.IsNullOrWhiteSpace(fullyQualifiedFunctionName))
            {
                throw new ArgumentNullException("fullyQualifiedFunctionName", "No function name supplied. Failed to decompile without a function name.");
            }

            ParsedMethodName mp = MethodNameParser.Parse(fullyQualifiedFunctionName);
            CodeDecompiler cd = new CodeDecompiler(mp);
            return cd.DecompileMethod();
        }

        
    }
}
