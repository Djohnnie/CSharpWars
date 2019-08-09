using System;

namespace CSharpWars.Web.Api.Validation.DtoModel
{
    public class ScriptValidationMessage
    {
        public String Message { get; set; }

        public Int32 LocationStart { get; set; }

        public Int32 LocationEnd { get; set; }
    }
}