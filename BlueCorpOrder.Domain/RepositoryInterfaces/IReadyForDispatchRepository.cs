using BlueCorpOrder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Domain.RepositoryInterfaces
{
    public interface IReadyForDispatchRepository
    {
        Task<ReadyForDispatch?> GetReadyForDispatchByControlNumberAsync(int controlNumber);
        Task<int> CreateAsync(ReadyForDispatch readyForDispatch);
    }
}
