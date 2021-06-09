using System;
using System.Threading.Tasks;

namespace SiteInstitucional.Client.ViewModels
{
    public abstract class DataContainerBase<T> where T : class
    {
        public T Params { get; private set; }

        public event Func<Task> OnChange;

        public async Task SetParamsAsync(T @params)
        {
            Params = @params;
            await NotifyStateChangedAsync();
        }

        private async Task NotifyStateChangedAsync()
        {
            if (OnChange != null)
            {
                await OnChange.Invoke();
            }
        }
    }
}
