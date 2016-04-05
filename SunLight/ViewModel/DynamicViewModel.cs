using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sunlight.Service;

namespace Sunlight.ViewModel
{
    public class DynamicViewModel : ViewModel
    {
        public DynamicViewModel(INavigationService2 navigationService, dynamic model)
            : base(navigationService)
        {
            Model = model;
        }

        public dynamic Model {  get; private set; }

        public Task AppendModel(IDictionary<string, Func<Task<dynamic>>> functions)
        {
            var modelDictionary = Model as IDictionary<string, object>;

            var tasks = from kvp in functions
                        where !modelDictionary.ContainsKey(kvp.Key)
                        select Task.Run(async () =>
                        {
                            var property = await kvp.Value();
                            modelDictionary.Add(kvp.Key, property);
                        });

            return Task.WhenAll(tasks).ContinueWith(t => RaisePropertyChangedOnUI("Model"));
        }
    }
}
