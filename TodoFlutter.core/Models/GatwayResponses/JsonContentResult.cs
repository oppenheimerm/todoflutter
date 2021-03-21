using Microsoft.AspNetCore.Mvc;


namespace TodoFlutter.core.Models.GatwayResponses
{
    public class JsonContentResult : ContentResult
    {
        public JsonContentResult()
        {
            ContentType = "application/json";
        }
    }
}
