using System;

namespace MovieAPi.Entities
{
    public class HttpRequestLog : AuditableBaseEntity
    {
        public int Id { get; set; }
        public string RequestPath { get; set; }
        public string RequestMethod { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public int ResponseStatusCode { get; set; }
        public DateTime RequestTime { get; set; }
    }
}