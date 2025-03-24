using BlueCorpOrder.Application.Dtos;
using BlueCorpOrder.Domain.Entities;
using BlueCorpOrder.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Infrastructure.Repository
{
    public class ReadyForDispatchRepository : IReadyForDispatchRepository
    {
        private static readonly List<ReadyForDispatch> _items = [];
        public Task<int> CreateAsync(ReadyForDispatch readyForDispatch)
        {
            _items.Add(readyForDispatch);
            return Task.FromResult(readyForDispatch.Id);
        }

        public Task<ReadyForDispatch?> GetReadyForDispatchByControlNumberAsync(int controlNumber)
        {
            return Task.FromResult(_items.Where(x => x.ControlNumber == controlNumber).FirstOrDefault());
        }
    }
}
