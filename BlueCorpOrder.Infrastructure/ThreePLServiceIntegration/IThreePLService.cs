using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Infrastructure.ThreePLServiceIntegration
{
    public interface IThreePLService
    {
        Task<bool> UploadFileToSftpAsync(string csvDataString);
    }
}
