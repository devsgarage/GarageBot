using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public static class Helpers
    {
        // Shout out to Coding for showing us this technique
        public static async void FireAndForget(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                // log errors
            }
        }
    }
}
