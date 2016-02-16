using System;
using System.Collections.Generic;

namespace Sunlight.Model
{
    public class SessionData
    {
        public DateTime AsOf { get; set; }
        public List<CongressionalSession> Sessions { get; set; }
    }

    public class CongressionalSession
    {
        public string Congress { get; set; }
        public string Session { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public override string ToString()
        {
            return $"{Congress} Congress, {Session} session";
        }
    }
}
