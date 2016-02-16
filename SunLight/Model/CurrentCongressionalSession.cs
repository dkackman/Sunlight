using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using Windows.Storage;

using Newtonsoft.Json;

namespace Sunlight.Model
{
    sealed class CurrentCongressionalSession : ICurrentCongressionalSession
    {
        private SessionData _data;
        private CongressionalSession _current;

        public CurrentCongressionalSession()
        {
            Load();
        }

        private async Task Load()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/Sessions.json"));
            var json = await FileIO.ReadTextAsync(file);

            _data = JsonConvert.DeserializeObject<SessionData>(json);

            var now = DateTime.UtcNow;
            var current = (from session in _data.Sessions
                          where session.Start <= now && session.End >= now
                          select session).FirstOrDefault();

            if(current == null)
            {
                throw new InvalidDataException("Congressional session data is out of date");
            }

            Interlocked.Exchange<CongressionalSession>(ref _current, current);
        }

        public CongressionalSession Current { get { return _current; } }
    }
}
