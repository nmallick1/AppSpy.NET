<%@ Page language="C#" %>

<%

foreach(DictionaryEntry e in System.Environment.GetEnvironmentVariables())
{
    Response.Write(e.Key  + ":" + e.Value + "<br/>");
}

%>
