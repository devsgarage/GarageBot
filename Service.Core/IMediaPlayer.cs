using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public interface IMediaPlayer
    {
        bool Disabled { get; set; }

        Task Play(string fileToPlay);
    }
}
