using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localStorage
{
    public interface ILocalStorage
    {
        Task<string?> GetItemAsync(string key);
        Task SetItemAsync(string key, string value, DateTime? expired = null);
    }
}
